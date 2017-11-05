/* This partial windows form code file will include the code for
 * the handlers of the form and form controls
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace RSACEncryption
{
    public partial class Form_Main : Form
    {

        #region BUTTON_HANDLERS
        private void button_Encrypt_Click(object sender, EventArgs e)
        {
            if (fileNameToEncrypt == String.Empty)
            {
                MessageWindow.ShowError("File not loaded!");
                return;
            }

            InitializeProgressBarAndTimer();

            this.totalDone = 0;

            RichTextBoxConsoleAddText("Preprocessing file to encrypt: " + DateTime.Now);

            Int64[] splittedBytesBoundaries = PreprocessFileToEncrypt();

            this.SetProgressBar(splittedBytesBoundaries[splittedBytesBoundaries.Length - 1].ToString());
            RichTextBoxConsoleAddText("Beginning encryption: " + DateTime.Now);
            FileInfo f2 = new FileInfo(fileNameToEncrypt);
            RichTextBoxConsoleAddText("File size: " + f2.Length.ToString() + " bytes");

            Common.RaiseAppEvent(Common.AppEventTypeEnum.EncryptionStarted, null);

            RSAScheduler.Instance.RunMultiThreadedRSAEncDec(EncryptBinary, splittedBytesBoundaries, Properties.AppSettings.Default.threadsRSA);
        }

        private void button_Decrypt_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Properties.AppSettings.Default.encryptedFile))
            {
                MessageWindow.ShowError("No file to decrypt!"); ;
                return;
            }

            InitializeProgressBarAndTimer();

            this.totalDone = 0;

            RichTextBoxConsoleAddText("Preprocessing file to decrypt: " + DateTime.Now);
            Int64[] splittedLinesBoundaries = PreprocessFileToDecrypt();

            this.SetProgressBar(splittedLinesBoundaries[splittedLinesBoundaries.Length - 1].ToString());

            RichTextBoxConsoleAddText("Beginning decryption: " + DateTime.Now);
            FileInfo f2 = new FileInfo(Properties.AppSettings.Default.encryptedFile);
            RichTextBoxConsoleAddText("File size: " + f2.Length.ToString() + " bytes");
            PanelButtonsEnabled(false);
            Common.RaiseAppEvent(Common.AppEventTypeEnum.DecryptionStarted, null);

            if(!Properties.AppSettings.Default.inMemoryDecrypt)
            RSAScheduler.Instance.RunMultiThreadedRSAEncDec(DecryptBinary, splittedLinesBoundaries, Properties.AppSettings.Default.threadsRSA);
            else
                RSAScheduler.Instance.RunMultiThreadedRSAEncDec(DecryptBinaryInMemory, splittedLinesBoundaries, Properties.AppSettings.Default.threadsRSA);
        }

        private void button_Prepare_Algorithm_Click(object sender, EventArgs e)
        {
            if (this.rSACE != null && this.rSACE.IsAlgorithmReady == true)
            {
                if (MessageWindow.ShowQuestion("Algorithm ready! Do you want to recalculate all RSA parameters?") == DialogResult.No)
                    return;
            }

            Common.RaiseAppEvent(Common.AppEventTypeEnum.AlgoPreparationStarted, null);
            RichTextBoxConsoleClear();

            // We create a nez instance of custom encryption class. Since we will prepare the algorithm, we pass
            // false and null as parameters
            NewRSACustomEncryptionInstance(false, null);

            threadCallingRSAPreparingMethod = System.Threading.Thread.CurrentThread;

            Thread t = new Thread(PrepareRSAAlgorithm);
            t.Start();
            //runThread = true;
            //Thread waitScreenThread = new Thread(ShowWaitScreen);
            //waitScreenThread.Start(t);
            //while (runThread)
            //{
            //    Thread.Sleep(20);
            //}
            //runThread = false;

            t.Join();
            threadCallingRSAPreparingMethod = null;

            Common.RaiseAppEvent(Common.AppEventTypeEnum.AlgoPrepared, null);
            if (!this.rSACE.IsAlgorithmReady) return;
        }

        private void button_LoadDataFromFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.fileNameToEncrypt = openFileDialog1.FileName;
                Common.RaiseAppEvent(Common.AppEventTypeEnum.FileLoaded, null);
            }
        }

        #endregion BUTTON_HANDLERS

        #region FORM_HANDLERS
        private void Form_Main_Load(object sender, EventArgs e)
        {
            Common.RaiseAppEvent(Common.AppEventTypeEnum.AppStarted, null);
        }

        #endregion FORM_HANDLERS

        #region FORM_CONTROLS_EVENTS
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Here we update the progress bar. The total progress of the encryption or decription
            // is based on the number of the total encryptions or decriptions done in one second
            pBOneSecondInterval = this.progressBar_EncrDecrProgress.Value - pBPrev;
            totalDone += pBOneSecondInterval;
            pBPrev = this.progressBar_EncrDecrProgress.Value;
            Int32 i = (int)((double)(progressBar_EncrDecrProgress.Maximum - totalDone) / pBOneSecondInterval);

            this.label_RemainingTime.Text = "Aprx. time remaining: " + i.ToString() + " sec.";// : "Calculating...";                

            if (this.progressBar_EncrDecrProgress.Value == progressBar_EncrDecrProgress.Maximum)
            {
                this.timer1.Stop();
                this.progressBar_EncrDecrProgress.Value = 0;
                this.label_RemainingTime.Text = String.Empty;
            }
        }
        #endregion FORM_CONTROLS_EVENTS

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_Settings frmSettings = new frm_Settings();
            frmSettings.ShowDialog();
            RichTextBoxScrollToBottom();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox_RSA aB = new AboutBox_RSA();
            aB.ShowDialog();
        }

        /// <summary>
        /// Shows info about the processor(s), physical, cores and logical
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void processorInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UInt32 nrPhysicalProcessors, nrCores, nrLogicalProcessors;

            nrPhysicalProcessors = Common.CountPhysicalProcessors();
            nrCores = Common.CountCores();
            nrLogicalProcessors = Common.CountLogicalProcessors();

            MessageWindow.ShowInfo(String.Format("Physical processors: {0}\nCores: {1}\nLogical processors: {2}",
                nrPhysicalProcessors, nrCores, nrLogicalProcessors));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbortOperationIfInProgress(false);
            Application.Exit();
        }

        private void abortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbortOperationIfInProgress(true);
        }

        private void exportKeyDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportRSAData();
        }


        private void importKeyDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportRSAData();
        }

        private void button_OpenLoadedFileFolder_Click(object sender, EventArgs e)
        {
            String folderToOpen = String.Empty;

            try
            {
                String[] s = fileNameToEncrypt.Split('\\');

                for (int i = 0; i < s.Length - 1; i++)
                {
                    folderToOpen = folderToOpen + s[i] + '\\';
                }
            }
            catch (Exception exc)
            {
                exc.ToString();
            }

            try
            {
                if (folderToOpen != String.Empty)
                    System.Diagnostics.Process.Start("explorer.exe", folderToOpen);
            }
            catch (Exception exc)
            {
                exc.ToString();
            }
        }

        private void button_OpenWorkingDirectory_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", Environment.CurrentDirectory);
            }
            catch (Exception exc)
            {
                exc.ToString();
            }
        }
    }
}
