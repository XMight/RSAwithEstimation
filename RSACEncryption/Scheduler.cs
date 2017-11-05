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
using System.Threading;

namespace RSACEncryption
{
    public class RSAScheduler
    {
        class SimpleClass
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static SimpleClass()
            {
            }

            internal static readonly RSAScheduler instance = new RSAScheduler();
        }

        public static RSAScheduler Instance
        {
            get
            {
                return SimpleClass.instance;
            }
        }

        public static readonly UInt32 nrCores = Common.CountCores();
        private Thread[] threadPool;
        private Thread tmpThread = null;

        private RSAScheduler()
        {
            threadPool = new Thread[nrCores];
        }

        public void RunSingleThreadedRSA(Common.GeneralDelegate gD)
        {
            tmpThread = new Thread(new ThreadStart(gD));
            tmpThread.Start();
        }


        public void RunMultiThreadedRSAEncDec(Common.OneParamDelegate oPD, Object methodToRunParam, UInt32 nrThreads)
        {
            Int64[] splittedDataBoundaries = (Int64[])methodToRunParam;

            if (nrThreads > nrCores)
            {
                threadPool = new Thread[nrThreads];
                Common.RaiseAppEvent(Common.AppEventTypeEnum.NrThreadsBiggerThanNrCores, null);
            }

            for (Int32 i = 0; i < nrThreads; i++)
            {
                threadPool[i] = new Thread(new ParameterizedThreadStart(oPD));
                threadPool[i].Priority = ThreadPriority.AboveNormal;
                threadPool[i].Start(new Int64[] { splittedDataBoundaries[i], splittedDataBoundaries[i + 1], i });
            }
        }

        public void AbortThread(Int32 nrThread)
        {
            threadPool[nrThread].Abort();
            threadPool[nrThread] = null;
        }

        public void AbortAllThreads()
        {
            for(Int32 i = 0; i < threadPool.Length; i++)
            {
                if(threadPool[i] != null) threadPool[i].Abort();
                threadPool[i] = null;
            }
        }
    }
}
