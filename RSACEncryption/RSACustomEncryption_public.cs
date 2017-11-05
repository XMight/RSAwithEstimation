/* Copyright (c) 2011 Andrei Busmachiu.
 * For questions, comments or bug reports, go to website at:
 * http://xmight.blogspot.com
 *
 * zlib Licence
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from the use of this software.
 * Permission is granted to anyone to use this software for any purpose, 
 * including commercial applications, and to alter it and redistribute it freely, 
 * subject to the following restrictions:

 * 1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 * 
 * This software was created only for teaching purposes, so it shall not be used in
 * any application without proper verification and testing.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace RSACEncryption
{
    public partial class RSACustomEncryption
    {
        /// <summary>
        /// Region including getters for the RSA variables. Return them only if the algorithm is ready
        /// </summary>
        #region Getters
        public BigInteger GetP
        {
            get
            {
                if (this.algorithmReady == true) return this.p;
                return 0;
            }
        }

        public BigInteger GetQ
        {
            get
            {
                if (this.algorithmReady == true) return this.q;
                return 0;
            }
        }

        public BigInteger GetN
        {
            get
            {
                if (this.algorithmReady == true) return this.n;
                return 0;
            }
        }

        public BigInteger GetPhi
        {
            get
            {
                if (this.algorithmReady == true) return this.phi;
                return 0;
            }
        }

        public BigInteger GetD
        {
            get
            {
                if (this.algorithmReady == true) return this.d;
                return 0;
            }
        }

        public BigInteger GetE
        {
            get
            {
                if (this.algorithmReady == true) return this.eC;
                return 0;
            }
        }

        public System.Boolean IsAlgorithmReady
        {
            get
            {
                return this.algorithmReady;
            }
        }

        /// <summary>
        /// I specified max size to 6144. Until 2020 the recommended key size is 2048, so...
        /// </summary>
        public UInt32 KeySize
        {
            get { return this.keySize; }
            // If the key will be set to 8, Generate_Primes() method will run indefinitly, because
            // the only odd numbers that will be generated will be 15 and 13 always
            set { this.keySize = value < 6144 && value > 8 ? value : 1024; }
        }

        /// <summary>
        /// Returns actions that can be performed
        /// </summary>
        public RSACustomEncryption.ActionsAvailableEnum ActionsAvailable
        {
            get { return this.actions; }
        }

        #endregion Getters

        #region PUBLIC_CONSTRUCTORS
        
        /// <summary>
        /// Constructor with one param.
        /// </summary>
        /// <param name="keyData">keyData must be null, if all the parameters will be calculated. 
        /// keyData[0] = e; keyData[1] = n; keyData[2] = d; => encrypt and decrypt to be available;
        /// keyData[0] = e; keyData[1] = n; keyData[2] = null; => encrypt only to be available;
        /// keyData[0] = null; keyData[1] = n; keyData[2] = d; => decrypt only to be available;</param>
        public RSACustomEncryption(Object[] keyData)
        {
            this.p = 0;
            this.q = 0;
            this.phi = 0;

            if (keyData != null && keyData.Length == 3)
            {
                if (keyData[0] != null && keyData[2] != null)
                {
                    this.eC = new BigInteger((String)(keyData[0]), 10);
                    this.n = new BigInteger((String)(keyData[1]), 10);
                    this.d = new BigInteger((String)(keyData[2]), 10);
                    this.actions = ActionsAvailableEnum.Decrypt | ActionsAvailableEnum.Encrypt;
                }

                if (keyData[0] != null && keyData[2] == null)
                {
                    this.eC = new BigInteger((String)(keyData[0]), 10);
                    this.n = new BigInteger((String)(keyData[1]), 10);
                    this.actions = ActionsAvailableEnum.Encrypt;
                }

                if (keyData[0] == null && keyData[2] != null)
                {
                    this.d = new BigInteger((String)(keyData[2]), 10);
                    this.n = new BigInteger((String)(keyData[1]), 10);
                    this.actions = ActionsAvailableEnum.Decrypt;
                }

                this.keySize = (UInt32)this.n.bitCount();
                this.algorithmReady = true;
            }
            else
            {
                this.algorithmReady = false;
                this.eC = 0;
                this.n = 0;
                this.keySize = 1024;
                this.actions = ActionsAvailableEnum.Decrypt | ActionsAvailableEnum.Encrypt;
            }
        }
        #endregion PUBLIC_CONSTRUCTORS

        #region PUBLIC_METHODS
        /// <summary>
        /// Must be called be called before using the algorithm
        /// </summary>
        public void PrepareAlgorithm()
        {
            algorithmReady = false;
            this.actions = ActionsAvailableEnum.Decrypt | ActionsAvailableEnum.Encrypt;
            BigInteger messageToTest = new BigInteger(77);

            while (!algorithmReady)
            {
                this.Generate_Primes();
                //this.Generate_Primes2();
                this.CalculateN();
                this.CalculatePhi();
                this.SelectE();

                // For a key of 1024 bits, the d was found in 192 miliseconds
                this.CalculateD_2();

                // This block is necessary to test if really the algorithm works(in the case when RabinMillerTest is wrong)
                {
                    BigInteger encryptedMessage = Encrypt(messageToTest);
                    BigInteger decryptedMessage = Decrypt(encryptedMessage);

                    if (decryptedMessage != messageToTest) continue;
                }
                // For a key of 1024 bits, I waited 15 minutes, and this method still was executing, I stopped it. 
                // So it's obvious the use of modulo inverse
                // this.CalculateD();
                //Int32 bC = this.n.bitCount();
                this.algorithmReady = true;
            }
        }

        /// <summary>
        /// If the algorithm is ready, then will return the public key as string
        /// </summary>
        /// <returns>Public key in the format 'Public key: {0},{1}'</returns>
        public String GetPublicKeyAsString(Int32 keyBase)
        {
            if (this.algorithmReady == true)
            {
                return String.Format("Public key: {0},{1}", this.eC.ToString(keyBase), this.n.ToString(keyBase));
            }
            return "Algorithm not ready, please use PrepareAlgorithm before...";
        }

        /// <summary>
        /// If the algorithm is ready, then will return the private key as string
        /// </summary>
        /// <returns>Private key in the format 'Private key: {0},{1}'</returns>
        public String GetPrivateKeyAsString(Int32 keyBase)
        {
            if (this.algorithmReady == true)
            {
                return String.Format("Private key: {0},{1}", this.d.ToString(keyBase), this.n.ToString(keyBase));
            }
            return "Algorithm not ready, please use PrepareAlgorithm before...";
        }

        /// <summary>
        /// Encrypts the passed Int64 number
        /// </summary>
        /// <param name="m">Number to encrypt</param>
        /// <returns>the resulting Int64</returns>
        public BigInteger Encrypt(BigInteger mToPow)
        {
            if ((Int32)(ActionsAvailable & ActionsAvailableEnum.Encrypt) == 0)
                return -1;

            BigInteger exp = new BigInteger(this.eC);
            BigInteger modN = new BigInteger(this.n);
            BigInteger modRes = mToPow.modPow(exp, modN);

            return modRes;
        }

        /// <summary>
        /// Decrypts the passed BigInteger number
        /// </summary>
        /// <param name="c">Number to decrypt</param>
        /// <returns>the resulting BigInteger</returns>
        public BigInteger Decrypt(BigInteger c)
        {
            if ((Int32)(ActionsAvailable & ActionsAvailableEnum.Decrypt) == 0)
                return -1;

            BigInteger cToPow = new BigInteger(c);
            BigInteger exp = new BigInteger(this.d);
            BigInteger modN = new BigInteger(this.n);
            BigInteger modRes = cToPow.modPow(exp, modN);

            return modRes;
        }
        #endregion PUBLIC_METHODS
    }
}
