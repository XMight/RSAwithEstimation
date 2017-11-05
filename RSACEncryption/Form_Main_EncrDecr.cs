/* This partial windows form code file will include the code for
 * the encryption and decryption methods
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
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace RSACEncryption
{
    public partial class Form_Main : Form
    {
        #region DECRYPTION
        private void DecryptBinary(Object decryptionOffsets)
        {
            try
            {
                Int64 decStartOffset = ((Int64[])decryptionOffsets)[0],
                   decEndOffset = ((Int64[])decryptionOffsets)[1], threadId = ((Int64[])decryptionOffsets)[2];

                if (DecryptionPreconditions())
                {
                    String decryptedFile = Properties.AppSettings.Default.decryptedFile;
                    String encryptedFile = Properties.AppSettings.Default.encryptedFile;

                    // We open the file and count the number of the lines in the file
                    StreamReader sR = new StreamReader(encryptedFile);
                    String lineRead = null;
                    Int32 count = 0, progressBarSteps = 0;
                    String[] lineSplitted = null;
                    Byte[] oneCharInBytes = new Byte[4];
                    Int32 i = 0, j = 0;
                    System.Collections.ArrayList aLWithBytes = new System.Collections.ArrayList();
                    Byte[] resultFromDecByteTable;
                    Int32 encDecBlock = Properties.AppSettings.Default.encDecBlock;
                    Byte[] decryptedBytesBlock = new Byte[encDecBlock];
                    String decryptedStringBlock = String.Empty, tmpString = String.Empty;

                    do
                    {
                        if (count++ == decStartOffset) break;
                    }
                    while ((lineRead = sR.ReadLine()) != null && lineRead != String.Empty);

                    count--;

                    while (count++ < decEndOffset && (lineRead = sR.ReadLine()) != null && lineRead != String.Empty)
                    {
                        lineSplitted = lineRead.Split(' ');

                        //i = 0;
                        foreach (String strByte in lineSplitted)
                        {
                            // Here we have a problem. We encrypted the data succesfully, but we can't decrypt it,
                            // if the block is bigger than 1 :). This is because the byte can have the length from 0 to 255
                            // And we can't know what we encrypted. Normally we need to find a solution, that is, for example,
                            // to transform all the bytes into a length of 4 (represent 0 by 0000 for example, so create a dictionary. By doing
                            // this, we will be sure that every byte has the same length, and we will be able to decrypt correctly), but this
                            // requires some time to research and right now I can't allocate time for this.
                            // So right now the correct encryption/decryption can be done for only one byte
                            decryptedStringBlock = this.rSACE.Decrypt(new BigInteger(strByte, 10)).ToString();
                            //for (j = 0; j < encDecBlock; j++)
                            //{
                            //    tmpString = decryptedStringBlock.Substring(j * 2, 2);
                            //    decryptedBytesBlock[j] = Byte.Parse(tmpString);
                            //    aLWithBytes.Add(decryptedBytesBlock[j]);
                            //}
                            aLWithBytes.Add(Byte.Parse(decryptedStringBlock));

                            //for (j = 0; j < encDecBlock; j++ )
                                
                            //i++;
                        }
                        this.StepProgressBar(); progressBarSteps++;
                    }

                    sR.Close();

                    resultFromDecByteTable = new Byte[aLWithBytes.Count];
                    j = 0;

                    foreach (Byte bait in aLWithBytes)
                    {
                        resultFromDecByteTable[j] = bait;
                        j++;
                    }

                    FileStream fS = new FileStream(decryptedFile.Split('.')[0] + threadId.ToString() + "." + decryptedFile.Split('.')[1], FileMode.Create, FileAccess.Write);
                    fS.Write(resultFromDecByteTable, 0, resultFromDecByteTable.Length);
                    fS.Close();

                    Common.RaiseAppEvent(Common.AppEventTypeEnum.WorkerFinishedJob, new Object[] { Properties.AppSettings.Default.threadsRSA, Common.AppEventTypeEnum.DecryptionDone, progressBarSteps });
                }
            }
            catch (Exception exc)
            {
                exc.ToString();
                Common.RaiseAppEvent(Common.AppEventTypeEnum.DecryptionImpossible, new Object[] { Properties.AppSettings.Default.threadsRSA});
            }

            Thread.CurrentThread.Abort();
        }

        private void DecryptBinaryInMemory(Object decryptionOffsets)
        {
            Int64 decStartOffset = ((Int64[])decryptionOffsets)[0],
               decEndOffset = ((Int64[])decryptionOffsets)[1], threadId = ((Int64[])decryptionOffsets)[2];

            if (DecryptionPreconditions())
            {
                String decryptedFile = Properties.AppSettings.Default.decryptedFile;
                String encryptedFile = Properties.AppSettings.Default.encryptedFile;

                // We open the file and count the number of the lines in the file
                StreamReader sR = new StreamReader(encryptedFile);
                String lineRead = null;
                Int32 count = 0, progressBarSteps = 0, nrLines = 0;
                String[] lineSplitted = null;
                String[] linesToDecrypt = new String[decEndOffset - decStartOffset];
                Byte[] oneCharInBytes = new Byte[4];
                Int32 i = 0, j = 0;
                System.Collections.ArrayList aLWithBytes = new System.Collections.ArrayList();
                Byte[] resultFromDecByteTable;

                do
                {
                    if (count++ == decStartOffset) break;
                }
                while ((lineRead = sR.ReadLine()) != null && lineRead != String.Empty);
                
                count--;

                while (count++ < decEndOffset && (lineRead = sR.ReadLine()) != null && lineRead != String.Empty)
                {
                    linesToDecrypt[nrLines] = lineRead;
                    nrLines++;
                }
                sR.Close();

                foreach(String line in linesToDecrypt)
                {
                    lineSplitted = line.Split(' ');

                    i = 0;
                    foreach (String strByte in lineSplitted)
                    {
                        oneCharInBytes[i] = Byte.Parse(this.rSACE.Decrypt(new BigInteger(strByte, 10)).ToString());
                        aLWithBytes.Add(oneCharInBytes[i]);
                        i++;
                    }
                    this.StepProgressBar(); progressBarSteps++;
                }

                resultFromDecByteTable = new Byte[aLWithBytes.Count];
                j = 0;

                foreach (Byte bait in aLWithBytes)
                {
                    resultFromDecByteTable[j] = bait;
                    j++;
                }

                FileStream fS = new FileStream(decryptedFile.Split('.')[0] + threadId.ToString() + "." + decryptedFile.Split('.')[1], FileMode.Create, FileAccess.Write);
                fS.Write(resultFromDecByteTable, 0, resultFromDecByteTable.Length);
                fS.Close();

                Common.RaiseAppEvent(Common.AppEventTypeEnum.WorkerFinishedJob, new Object[] { Properties.AppSettings.Default.threadsRSA, Common.AppEventTypeEnum.DecryptionDone, progressBarSteps });
            }

            Thread.CurrentThread.Abort();
        }

        private Int64[] PreprocessFileToDecrypt()
        {
            UInt32 threadsRSA = Properties.AppSettings.Default.threadsRSA;
            // We open the file and count the number of the lines in the file
            StreamReader sR = new StreamReader(Properties.AppSettings.Default.encryptedFile);
            String lineRead = null;
            Int32 count = 0;
            while ((lineRead = sR.ReadLine()) != null)
            {
                count++;
            }

            Int64[] splittedLines = new Int64[threadsRSA + 1];
            splittedLines[0] = 0;

            if (threadsRSA == 1)
            {
                splittedLines[1] = count;
            }
            else
            {
                Int64 remaining = 0;

                splittedLines[1] = (Int64)(count / (System.Double)threadsRSA);

                for (Int32 i = 2; i <= threadsRSA; i++)
                {
                    splittedLines[i] = splittedLines[1] + splittedLines[i - 1];
                }

                splittedLines[threadsRSA] = count;

                remaining = count - splittedLines[1] * threadsRSA;

                for (UInt32 i = threadsRSA; i > 0 && remaining != 0; i--)
                {
                    if (i % remaining == 0)
                    {
                        for (UInt32 j = 1; j <= i && remaining-- > 0; j++)
                        {
                            splittedLines[j]++;
                        }
                        break;
                    }
                }
            }

            return splittedLines;
        }

        private void PostProcessDecryptedFiles()
        {
            try
            {
                UInt32 threadsRSA = Properties.AppSettings.Default.threadsRSA;
                String decryptedFile = Properties.AppSettings.Default.decryptedFile;

                FileStream fS;
                Byte[][] bytesEncrypted = new Byte[threadsRSA][];
                Byte[] tmpArray = null;
                Int32 threadsMinusOne = 0, actualLastBytesInOrigFileNr = 0;
                String fileName;

                for (Int32 i = 0; i < threadsRSA; i++)
                {
                    fileName = decryptedFile.Split('.')[0] + i + "." + decryptedFile.Split('.')[1];
                    fS = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                    bytesEncrypted[i] = new Byte[fS.Length];
                    fS.Read(bytesEncrypted[i], 0, (Int32)fS.Length);
                    fS.Close();

                    File.Delete(fileName);
                }

                // Last thread will process the last bit of the file (so the scheduler works), so we need the last byte
                // to know how many bytes were added at the encryption
                threadsMinusOne = (Int32)(threadsRSA - 1);
                Byte nrBytesToEraseFromTheEnd = bytesEncrypted[threadsMinusOne][bytesEncrypted[threadsMinusOne].Length - 1];

                // This difference must always be > 0, because of the encryption that verifies this
                actualLastBytesInOrigFileNr = bytesEncrypted[threadsMinusOne].Length - nrBytesToEraseFromTheEnd;
                tmpArray = new Byte[actualLastBytesInOrigFileNr];

                for (Int32 i = 0; i < actualLastBytesInOrigFileNr; i++)
                {
                    tmpArray[i] = bytesEncrypted[threadsMinusOne][i];
                }

                bytesEncrypted[threadsMinusOne] = tmpArray;

                fS = new FileStream(decryptedFile, FileMode.Create, FileAccess.Write);

                for (Int32 i = 0; i < threadsRSA; i++)
                {
                    fS.Write(bytesEncrypted[i], 0, bytesEncrypted[i].Length);
                }

                fS.Close();
            }
            catch (Exception exc)
            {
                exc.ToString();
            }
        }

        #endregion DECRYPTION

        #region ENCRYPTION
        private void EncryptBinary(Object encryptionOffsets)
        {
            Int64 encStartOffset = ((Int64[])encryptionOffsets)[0],
                encEndOffset = ((Int64[])encryptionOffsets)[1], threadId = ((Int64[])encryptionOffsets)[2];
            
            if (EcryptionPreconditions())
            {
                String encryptedFile = Properties.AppSettings.Default.encryptedFile;

                FileStream fS = new FileStream(fileNameToEncrypt, FileMode.Open, FileAccess.Read);
                Byte[] bytesToEncrypt = new Byte[(Int32)(encEndOffset - encStartOffset)];
                fS.Seek(encStartOffset, SeekOrigin.Begin);
                fS.Read(bytesToEncrypt, 0, (Int32)(encEndOffset - encStartOffset));
                fS.Close();

                String dataToEncrypt = String.Empty, byteToConvertString = String.Empty;
                BigInteger encryptedByte;
                List<String> encryptedLines = new List<String>();
                Int32 encDecBlock = Properties.AppSettings.Default.encDecBlock;
                Byte[] bytesBlockToEncrypt = new Byte[encDecBlock];

                if (bytesToEncrypt.Length != 0)
                {
                    for (int j = 0; j < bytesToEncrypt.Length; j+= encDecBlock)
                    {
                        Int32 k;
                        for (k = 0; k < encDecBlock; k++)
                        {
                            // In the case if the number of bytes in the block that we want to encrypt
                            // fits in the number of the chosen block length, then we assign the data normally
                            // otherwise we add some zeros, so that encryptedBytes % encDecBlock == 0
                            if (k + j < bytesToEncrypt.Length)
                                bytesBlockToEncrypt[k] = bytesToEncrypt[j + k];
                            else
                                bytesBlockToEncrypt[k] = 0;
                        }


                        // We encrypt all the bytes in the block at once
                        encryptedByte = rSACE.Encrypt(new BigInteger(bytesBlockToEncrypt));

                        if (byteToConvertString != String.Empty)
                        {
                            byteToConvertString += " " + encryptedByte.ToString();
                        }
                        else
                        {
                            byteToConvertString = encryptedByte.ToString();
                        }

                        this.StepProgressBar();
                        encryptedLines.Add(byteToConvertString);
                        byteToConvertString = String.Empty;
                    }

                    TextWriter tW = new StreamWriter(encryptedFile.Split('.')[0] + threadId.ToString() + "." + encryptedFile.Split('.')[1]);
                    foreach(String line in encryptedLines)
                    {
                        tW.WriteLine(line);
                    }

                    tW.Close();

                    Common.RaiseAppEvent(Common.AppEventTypeEnum.WorkerFinishedJob, new Object[] {(UInt32)threadId, Common.AppEventTypeEnum.EncryptionDone});
                }
            }

            Thread.CurrentThread.Abort();
        }

        private Int64[] PreprocessFileToEncrypt()
        {
            UInt32 threadsRSA = Properties.AppSettings.Default.threadsRSA;

            Int64[] splittedBytes = new Int64[threadsRSA + 1];
            splittedBytes[0] = 0;

            FileStream fS = new FileStream(fileNameToEncrypt, FileMode.Open, FileAccess.ReadWrite);
            Int64 fSLength = fS.Length;
            Int64 remainingNotFulEncDecBlock = fSLength % Properties.AppSettings.Default.encDecBlock;
            fS.Seek(0, SeekOrigin.End);
            Int64 nrBytesToAdd = Properties.AppSettings.Default.encDecBlock - remainingNotFulEncDecBlock;

            Byte fillByte = (Byte)'$';
            for (Int32 i = 0; i < nrBytesToAdd - 1; i++)
                fS.WriteByte(fillByte);
            // In our case, this value will be never bigger than what can be stored in one byte
            // Because our Properties.AppSettings.Default.encDecBlock is a byte, i.e. maximum 
            // Properties.AppSettings.Default.encDecBlock bytes can be encrypted toghether
            // We will use this last value to know at the decryption how many bytes we added
            fS.WriteByte((Byte)nrBytesToAdd);
            fSLength = fS.Length;
            fS.Close();

            if (threadsRSA == 1)
            {
                splittedBytes[1] = fSLength;
            }
            else
            {
                Int64 remaining = 0;

                splittedBytes[1] = (Int64)(fSLength / (System.Double)threadsRSA);
                
                for (Int32 i = 2; i <= threadsRSA; i++)
                {
                    splittedBytes[i] = splittedBytes[1] + splittedBytes[i - 1];
                }

                splittedBytes[threadsRSA] = fSLength;

                remaining = fSLength - splittedBytes[1] * threadsRSA;

                for (UInt32 i = threadsRSA; i > 0 && remaining != 0; i-- )
                {
                    if (i % remaining == 0)
                    {
                        for (UInt32 j = 1; j <= i && remaining-- > 0; j++)
                        {
                            splittedBytes[j]++;
                        }
                        break;
                    }
                }
            }

            return splittedBytes;
        }

        private void PostProcessEncryptedFiles()
        {
            try
            {
                UInt32 threadsRSA = Properties.AppSettings.Default.threadsRSA;
                String encryptedFile = Properties.AppSettings.Default.encryptedFile;

                FileStream fS;
                Byte[][] bytesEncrypted = new Byte[threadsRSA][];
                String fileName;

                for (Int32 i = 0; i < threadsRSA; i++)
                {
                    fileName = encryptedFile.Split('.')[0] + i + "." + encryptedFile.Split('.')[1];
                    fS = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                    bytesEncrypted[i] = new Byte[fS.Length];
                    fS.Read(bytesEncrypted[i], 0, (Int32)fS.Length);
                    fS.Close();

                    File.Delete(fileName);
                }

                fS = new FileStream(encryptedFile, FileMode.Create, FileAccess.Write);

                for (Int32 i = 0; i < threadsRSA; i++)
                {
                    fS.Write(bytesEncrypted[i], 0, bytesEncrypted[i].Length);
                }

                fS.Close();

                fS = new FileStream(fileNameToEncrypt, FileMode.Open, FileAccess.ReadWrite);
                fS.Seek(-1, SeekOrigin.End);
                Int32 addedBytes = fS.ReadByte();
                fS.SetLength(fS.Length - addedBytes);
                fS.Close();
            }
            catch (Exception exc)
            {
                exc.ToString();
            }
        }
        #endregion ENCRYPTION
    }
}
