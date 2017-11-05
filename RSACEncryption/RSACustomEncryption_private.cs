/*
 *  RSACustomEncryption Class v1.0
 *  
 * Copyright (c) 2011 Andrei Busmachiu.
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
 * 
 * 
 * 
 *  AUTHOR                  DATE(US FORMAT)                 ACTION TYPE
 *  
 *  BUSMACHIU ANDREI        x/x/2009                        CREATE
 *  BUSMACHIU ANDREI        12/27/2010                      IMPROVE, ADD COMMENTS
 *  
 *  REFERENCES:
 *  http://en.wikipedia.org/wiki/Extended_Euclidean_algorithm
 *  http://snippets.dzone.com/posts/show/4256
 *  http://everything2.com/title/Extended+Euclidean+algorithm
 *  http://primes.utm.edu/lists/small/1000.txt
 *  http://en.wikipedia.org/wiki/Miller%E2%80%93Rabin_primality_test
 *  http://www.di-mgt.com.au/rsa_alg.html
 */


using System;

namespace RSACEncryption
{
    /// <summary>
    /// The class that provides the necessary methods for preparing
    /// the RSA algorithm and for its application
    /// </summary>
    public partial class RSACustomEncryption
    {
        // Hoiw many times the rabin miller test of primality must be applied to the number to test
        private const Int32 rabinMillerConfidence = 20;

        /// <summary>
        /// First prime number
        /// </summary>
        private BigInteger p;
        
        /// <summary>
        /// Second prime number
        /// </summary>
        private BigInteger q;

        /// <summary>
        /// The exponent of the private key
        /// </summary>
        private BigInteger eC;

        /// <summary>
        /// The product p*q
        /// </summary>
        private BigInteger n;

        /// <summary>
        /// The product (p-1)(q-1)
        /// </summary>
        private BigInteger phi;

        /// <summary>
        /// The value of the expression: (1+k*phi)/eC
        /// </summary>
        private BigInteger d;

        /// <summary>
        /// Boolean indicating that all necessary properties of the algorithm were calculated and ready
        /// </summary>
        private System.Boolean algorithmReady;

        /// <summary>
        /// The size of the RSA key
        /// </summary>
        private System.UInt32 keySize;

        /// <summary>
        /// The variable holding which actions are available to execute, based on what parameters
        /// the constructor with one parameter takes
        /// </summary>
        private ActionsAvailableEnum actions;

        #region PRIVATE_METHODS
        /// <summary>
        /// Method to generate the primes used in this algorithm
        /// If the number is not prime, a new number is generated
        /// This method is faster and surely better for keys up to 2048 bits, but appears from 2048 the
        /// problem that we will never get a prime number, see TestingRemarks.txt
        /// </summary>
        /// <remarks>For implementation details see http://www.di-mgt.com.au/rsa_alg.html</remarks>
        private void Generate_Primes()
        {
            UInt32 pLength = (UInt32)(this.keySize / 2.0), qLength = keySize - pLength;

            // Finding number of cores
            UInt32 coreCount = 0;
            //NumberOfCores
            foreach (System.Management.ManagementObject item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += UInt32.Parse(item["NumberOfCores"].ToString());
            }

            if (coreCount >= 2)
            {
                // Finding primes with two threads is more efficient on multicore systems
                System.Threading.Thread th = new System.Threading.Thread(delegate()
                {
                    do
                    {
                        this.p = GenerateOddNBitNumber(pLength);

                    } while (!RabinMillerPrimeTest(this.p, rabinMillerConfidence));//while (!this.p.RabinMillerTest(rabinMillerConfidence));                
                });

                System.Threading.Thread th2 = new System.Threading.Thread(delegate()
                {
                    do
                    {
                        this.q = GenerateOddNBitNumber(qLength);

                    } while (!RabinMillerPrimeTest(this.q, rabinMillerConfidence)); //while (!this.q.RabinMillerTest(rabinMillerConfidence) || this.p == this.q) ;
                });

                th.Start();
                th2.Start();
                th.Join();
                th2.Join();
            }
            else
            {
                do
                {
                    this.p = GenerateOddNBitNumber(pLength);
                    //this.p.genRandomBits((Int32)pLength, new Random((Int32)DateTime.Now.Ticks));
                    //res = this.p.RabinMillerTest(rabinMillerConfidence);

                } /*while (!RabinMillerPrimeTest(this.p, rabinMillerConfidence) || res == false);*/while (!this.p.RabinMillerTest(rabinMillerConfidence));
                RabinMillerPrimeTest(this.p, rabinMillerConfidence);
                do
                {
                    this.q = GenerateOddNBitNumber(qLength);

                } while (!RabinMillerPrimeTest(this.q, rabinMillerConfidence)); //while (!this.q.RabinMillerTest(rabinMillerConfidence) || this.p == this.q) ;
            }

            if(this.p < this.q)
            {
                BigInteger tmp = new BigInteger(this.p);
                this.p = this.q;
                this.q = tmp;
            }
        }

        /// <summary>
        /// Method to generate the primes used in this algorithm
        /// If the number is not prime, then it is incremented by 2 and tested again for primality.
        /// Tested for generation of 512 bit key, generation run for 10 minutes, no success. (P.S. I verify
        /// in PrepareAlgorithm() if encryption/decryption succeeds before claiming that the key is ready)
        /// </summary>
        /// <remarks></remarks>
        private void Generate_Primes2()
        {
            UInt32 pLength = (UInt32)(this.keySize / 2.0), qLength = keySize - pLength;

            this.p = GenerateOddNBitNumber(pLength);
            this.q = GenerateOddNBitNumber(qLength);

            // Finding number of cores
            UInt32 coreCount = 0;
            //NumberOfCores
            foreach (System.Management.ManagementObject item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += UInt32.Parse(item["NumberOfCores"].ToString());
            }

            if (coreCount > 2)
            {
                // Finding primes with two threads is more efficient on multicore systems
                System.Threading.Thread th = new System.Threading.Thread(delegate()
                {
                    while (!RabinMillerPrimeTest(this.p, rabinMillerConfidence*10))
                    {
                        this.p += 2;
                    }               
                });

                System.Threading.Thread th2 = new System.Threading.Thread(delegate()
                {
                    while (!RabinMillerPrimeTest(this.q, rabinMillerConfidence*10))
                    {
                        this.q += 2;
                    }
                });

                th.Start();
                th2.Start();
                th.Join();
                th2.Join();
            }
            else
            {
                while (!RabinMillerPrimeTest(this.p, rabinMillerConfidence*10))
                {
                    this.p += 2;
                }

                while (!RabinMillerPrimeTest(this.q, rabinMillerConfidence*10))
                {
                    this.q += 2;
                }
            }

            if (this.p < this.q)
            {
                BigInteger tmp = new BigInteger(this.p);
                this.p = this.q;
                this.q = tmp;
            }
        }

        /// <summary>
        /// Method to calculate n
        /// </summary>
        private void CalculateN()
        {
            this.n = this.p * this.q;
        }

        /// <summary>
        /// Method to calculate phi
        /// </summary>
        private void CalculatePhi()
        { 
            this.phi = (this.p - 1) * (this.q - 1);
        }

        /// <summary>
        /// Method to select eC from the interval 1 &lt; e &lt; φ(n)
        /// </summary>
        private void SelectE()
        {
            BigInteger generatedE = GenerateOddNBitNumber((UInt32)(keySize / 2.0));

            for (; ; generatedE++)
            {
                if (AreRelativePrime(this.phi, generatedE))
                {
                    this.eC = generatedE;
                    break;
                }
            }

            if (this.eC == 0)
            {
                throw new RSACustomEncryptionException("Cannnot select e!");
            }
        }

        /// <summary>
        /// The method is the standart one used to find d, based on the RSA algorithm
        /// TO be used only with small numbers. With a key of 1024 bit it was executing for 15 minutes and no success
        /// </summary>
        private void CalculateD()
        {
            BigInteger tmp_D;

            for (int k = 2; ; k++)
            {
                this.d = (this.phi * k + 1) / this.eC;
                tmp_D = (this.phi * k + 1) % this.eC;

                // We verify that d is integral, so in this case tmp_D must be 0
                if (tmp_D == 0)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// This method calculates d using modulo inverse algorithm
        /// </summary>
        private void CalculateD_2()
        {
            this.d = (Extended_GDC(this.eC, this.phi, true))[1];
        }

        #endregion PRIVATE_METHODS
    }
}
