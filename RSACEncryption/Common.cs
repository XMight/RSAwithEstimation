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
    public static class Common
    {
        public enum AppEventTypeEnum
        {
            None = 0,
            AppStarted = 1,
            AlgoPrepared = 2,
            FileLoaded = 3,
            EncryptionDone = 4,
            DecryptionDone = 5,
            AlgoPreparationRestarted = 6,
            AlgoPreparationStarted = 7,
            EncryptionStarted = 8,
            DecryptionStarted = 9,
            SettingsSaved = 10,
            NrThreadsBiggerThanNrCores = 11,
            WorkerFinishedJob = 12,
            OperationAborted = 13,
            RSADataExported = 14,
            RSADataImported = 15,
            DecryptionImpossible = 16
        }

        public enum ImpExpKeyDataTypeEnum
        { 
            BothKeys = 0,
            PrivateKey = 1,
            PublicKey = 2
        }

        public enum ShowKeyInBaseEnum
        { 
            Decimal = 10,
            Hex = 16,
            Binary = 2
        }

        public delegate void ApplicationEventDelegate(Common.AppEventTypeEnum eventType, Object[] parms);
        public delegate void OneParamDelegate(Object s);
        public delegate void GeneralDelegate();

        public static event ApplicationEventDelegate AppEventOccured;

        public static void RaiseAppEvent(Common.AppEventTypeEnum eventType, Object[] parms)
        {
            AppEventOccured(eventType, parms);
        }

        public static UInt32 CountPhysicalProcessors()
        {
            UInt32 nrPhysicalProcessors = 0;

            foreach (System.Management.ManagementObject item in new System.Management.ManagementObjectSearcher("Select NumberOfProcessors from Win32_ComputerSystem").Get())
            {
                //MessageWindow.ShowInfo(String.Format("Number Of Physical Processors: {0} ", item["NumberOfProcessors"]));
                nrPhysicalProcessors = Convert.ToUInt32(item["NumberOfProcessors"]);
            }

            return nrPhysicalProcessors;
        }

        public static UInt32 CountCores()
        {
            UInt32 coreCount = 0;
            //NumberOfCores
            foreach (System.Management.ManagementObject item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += UInt32.Parse(item["NumberOfCores"].ToString());
            }

            //MessageWindow.ShowInfo(String.Format("Number Of Cores: {0}", coreCount));

            return coreCount;
        }

        public static UInt32 CountLogicalProcessors()
        {
            //MessageWindow.ShowInfo(String.Format("Number Of Logical Processors: {0}", Environment.ProcessorCount));
            UInt32 nrLogicalProcessors = 0;

            foreach (var item in new System.Management.ManagementObjectSearcher("Select NumberOfLogicalProcessors from Win32_ComputerSystem").Get())
            {
                nrLogicalProcessors = Convert.ToUInt32(item["NumberOfLogicalProcessors"]);
            }

            return nrLogicalProcessors;
        }
    }
}
