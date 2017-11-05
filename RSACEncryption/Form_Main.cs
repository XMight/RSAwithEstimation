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
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace RSACEncryption
{
    public partial class Form_Main : Form
    {
        private RSACustomEncryption rSACE;
        private Boolean fileLoaded;
        private Int32 pBOneSecondInterval, totalDone;
        private Int32 pBPrev;
        private Thread threadCallingRSAPreparingMethod = null;
        private Form_Wait fWait;
        private Boolean runThread = false;
        private String fileNameToEncrypt;
        private Int32 workersFinishedJob = 0;
        private System.Collections.ArrayList pBSteps = null;
        private Common.AppEventTypeEnum lastOperation = Common.AppEventTypeEnum.None;

        public Form_Main()
        {
            InitializeComponent();
            this.fileNameToEncrypt = String.Empty;
            this.fileLoaded = false;
            Common.AppEventOccured += new Common.ApplicationEventDelegate(Common_AppEventOccured);
            this.label_CurrentOperation.Text = "";
            this.label_RemainingTime.Text = "";
        }

        #region PRIVATE_METHODS

        private void Common_AppEventOccured(Common.AppEventTypeEnum eventType, Object[] parms)
        {
            switch (eventType)
            {
                case Common.AppEventTypeEnum.AppStarted: 
                    DeleteTemporaryFiles();
                    ButtonLoadDataEnabled(false);
                    ButtonOpenLoadedFileFolderEnabled(false);
                    ButtonEncryptEnabled(false);
                    ButtonDecryptEnabled(false);
                    LabelKeySizeUpdate();
                    break;
                case Common.AppEventTypeEnum.AlgoPrepared:
                    this.PanelButtonsEnabled(true);
                    ButtonLoadDataEnabled(true);
                    ButtonOpenLoadedFileFolderEnabled(false);
                    ButtonEncryptEnabled(false);
                    ButtonDecryptEnabled(false);
                    this.PrintRSAParameters(false);
                    break;
                case Common.AppEventTypeEnum.FileLoaded:
                    ButtonEncryptEnabled(true);
                    ButtonOpenLoadedFileFolderEnabled(true);
                    ButtonDecryptEnabled(false);
                    this.fileLoaded = true;
                    RichTextBoxConsoleAddText("File to encrypt loaded!");
                    break;
                case Common.AppEventTypeEnum.EncryptionDone:
                    RichTextBoxConsoleAddText(String.Format("Generating final output! : {0}", DateTime.Now));
                    PostProcessEncryptedFiles();
                    RichTextBoxConsoleAddText(String.Format("Encryption done! Verify encrypted.enc file! : {0}", DateTime.Now));
                    this.PanelButtonsEnabled(true);
                    ButtonDecryptEnabled(true);
                    UpdateCurrOperationStatus("");
                    break;
                case Common.AppEventTypeEnum.AlgoPreparationStarted:
                    this.PanelButtonsEnabled(false);
                    LabelKeySizeUpdate();
                    break;
                case Common.AppEventTypeEnum.AlgoPreparationRestarted:
                    this.ParamsToDefault();
                    this.DeleteTemporaryFiles();
                    ButtonLoadDataEnabled(false);
                    ButtonOpenLoadedFileFolderEnabled(false);
                    ButtonEncryptEnabled(false);
                    ButtonDecryptEnabled(false);
                    RichTextBoxConsoleClear();
                    break;
                case Common.AppEventTypeEnum.EncryptionStarted:
                    UpdateCurrOperationStatus("Encryption in progress...");
                    this.PanelButtonsEnabled(false);
                    workersFinishedJob = 0;
                    break;
                case Common.AppEventTypeEnum.DecryptionStarted:
                    UpdateCurrOperationStatus("Decryption in progress...");
                    this.PanelButtonsEnabled(false);
                    workersFinishedJob = 0;
                    pBSteps = new System.Collections.ArrayList();
                    break;
                case Common.AppEventTypeEnum.DecryptionDone:
                    RichTextBoxConsoleAddText(String.Format("Generating final output! : {0}", DateTime.Now));
                    PostProcessDecryptedFiles();
                    this.PanelButtonsEnabled(true);
                    RichTextBoxConsoleAddText(String.Format("Decryption done! Verify decrypted.dec file! : {0}", DateTime.Now));
                    UpdateCurrOperationStatus("");
                    break;
                case Common.AppEventTypeEnum.SettingsSaved:
                    if (parms == null) break;
                    if ((UInt32)parms[0] != Properties.AppSettings.Default.keySize)
                    {
                        Common_AppEventOccured(Common.AppEventTypeEnum.AlgoPreparationRestarted, null);
                        RichTextBoxConsoleAddText(String.Format("Key size modified. Key size: {0} bits", Properties.AppSettings.Default.keySize));
                    }
                    if ((Int32)parms[1] != Properties.AppSettings.Default.showKeyInBase
                        && (lastOperation == Common.AppEventTypeEnum.AlgoPrepared || lastOperation == Common.AppEventTypeEnum.SettingsSaved))
                    {
                        RichTextBoxConsoleClear();
                        PrintRSAParameters(false);
                    }
                    LabelKeySizeUpdate();
                    break;
                case Common.AppEventTypeEnum.NrThreadsBiggerThanNrCores:
                    RichTextBoxConsoleAddText("Warning! Running more threads than cores on the machine!");
                    break;
                case Common.AppEventTypeEnum.WorkerFinishedJob:
                    if (parms != null && parms[0] != null && parms[1] != null)
                    {
                        if ((Common.AppEventTypeEnum)parms[1] != Common.AppEventTypeEnum.EncryptionDone && parms[2] != null) pBSteps.Add(parms[2]);
                        workersFinishedJob++;
                        if (workersFinishedJob == Properties.AppSettings.Default.threadsRSA)
                        {
                            Common.RaiseAppEvent((Common.AppEventTypeEnum)parms[1], null);
                        }
                    }
                    break;
                case Common.AppEventTypeEnum.OperationAborted:
                    PanelButtonsEnabled(true);
                    // This will ensure that the timer and progressBar are updated and stopped
                    SetProgressBarToMax();
                    UpdateCurrOperationStatus("");
                    RichTextBoxConsoleAddText("Operation aborted!");
                    break;
                case Common.AppEventTypeEnum.RSADataExported:
                    
                    String messageToShow = String.Empty;
                    
                    switch (Properties.AppSettings.Default.impExpKeySetting)
                    {
                        case (Int32)Common.ImpExpKeyDataTypeEnum.BothKeys:
                            messageToShow = String.Format("RSA data exported to: {0}", Properties.AppSettings.Default.encryptionKeysExportFileName); ;
                            break;
                        case (Int32)Common.ImpExpKeyDataTypeEnum.PrivateKey:
                            messageToShow = String.Format("Private key exported to: {0}", Properties.AppSettings.Default.privateKeyExportFileName);
                            break;
                        case (Int32)Common.ImpExpKeyDataTypeEnum.PublicKey:
                            messageToShow = String.Format("Public key exported to: {0}", Properties.AppSettings.Default.publicKeyExportFileName);
                            break;
                        default:
                            break;
                    }

                    MessageWindow.ShowInfo(messageToShow);
                    break;
                case Common.AppEventTypeEnum.RSADataImported:
                    switch (rSACE.ActionsAvailable)
                    { 
                        case RSACustomEncryption.ActionsAvailableEnum.Encrypt | RSACustomEncryption.ActionsAvailableEnum.Decrypt:
                        case RSACustomEncryption.ActionsAvailableEnum.Encrypt:
                            this.PanelButtonsEnabled(true);
                            ButtonLoadDataEnabled(true);
                            ButtonOpenLoadedFileFolderEnabled(false);
                            ButtonEncryptEnabled(false);
                            ButtonDecryptEnabled(true);
                            this.PrintRSAParameters(true);
                            break;
                        case RSACustomEncryption.ActionsAvailableEnum.Decrypt:
                            PanelButtonsEnabled(true);
                            ButtonLoadDataEnabled(false);
                            ButtonOpenLoadedFileFolderEnabled(false);
                            ButtonEncryptEnabled(false);
                            ButtonDecryptEnabled(true);
                            break;
                    }
                    break;
                case Common.AppEventTypeEnum.DecryptionImpossible:
                    if (parms != null)
                    {
                        workersFinishedJob++;
                        if (workersFinishedJob == Properties.AppSettings.Default.threadsRSA)
                        {
                            MessageWindow.ShowError("Cannot decrypt data with the provided key!");
                            SetProgressBarToMax();
                            UpdateCurrOperationStatus("");
                            this.PanelButtonsEnabled(true);
                        }
                    }
                    break;
                default: break;
            }

            lastOperation = eventType;
        }

        private void PrintRSAParameters(Boolean importedKeyData)
        {
            if (!importedKeyData)
            {
                if (this.rSACE == null || rSACE.IsAlgorithmReady == false)
                {
                    this.richTextBox_Console.Text += "RSA Parameters are not available or an error occured";
                    return;
                }

                RichTextBoxConsoleAddText("p: " + rSACE.GetP.ToString(Properties.AppSettings.Default.showKeyInBase));
                RichTextBoxConsoleAddText("q: " + rSACE.GetQ.ToString(Properties.AppSettings.Default.showKeyInBase));
                RichTextBoxConsoleAddText("n: " + rSACE.GetN.ToString(Properties.AppSettings.Default.showKeyInBase));
                RichTextBoxConsoleAddText("phi: " + rSACE.GetPhi.ToString(Properties.AppSettings.Default.showKeyInBase));
                RichTextBoxConsoleAddText("e: " + rSACE.GetE.ToString(Properties.AppSettings.Default.showKeyInBase));
                RichTextBoxConsoleAddText("d: " + rSACE.GetD.ToString(Properties.AppSettings.Default.showKeyInBase));
                RichTextBoxConsoleAddText(rSACE.GetPublicKeyAsString(Properties.AppSettings.Default.showKeyInBase));
                RichTextBoxConsoleAddText(rSACE.GetPrivateKeyAsString(Properties.AppSettings.Default.showKeyInBase));
                RichTextBoxConsoleAddText("Algorithm RSA ready to apply!");
            }
            else
            {
                switch (Properties.AppSettings.Default.impExpKeySetting)
                {
                    case (Int32)Common.ImpExpKeyDataTypeEnum.BothKeys:
                        RichTextBoxConsoleAddText("e: " + rSACE.GetE.ToString(Properties.AppSettings.Default.showKeyInBase));
                        RichTextBoxConsoleAddText("n: " + rSACE.GetN.ToString(Properties.AppSettings.Default.showKeyInBase));
                        RichTextBoxConsoleAddText("d: " + rSACE.GetD.ToString(Properties.AppSettings.Default.showKeyInBase));
                        break;
                    case (Int32)Common.ImpExpKeyDataTypeEnum.PrivateKey:
                        RichTextBoxConsoleAddText("d: " + rSACE.GetD.ToString(Properties.AppSettings.Default.showKeyInBase));
                        RichTextBoxConsoleAddText("n: " + rSACE.GetN.ToString(Properties.AppSettings.Default.showKeyInBase));
                        break;
                    case (Int32)Common.ImpExpKeyDataTypeEnum.PublicKey:
                        RichTextBoxConsoleAddText("e: " + rSACE.GetE.ToString(Properties.AppSettings.Default.showKeyInBase));
                        RichTextBoxConsoleAddText("n: " + rSACE.GetN.ToString(Properties.AppSettings.Default.showKeyInBase));
                        break;
                    default:
                        break;
                }
            }
        }

        private void ParamsToDefault()
        {
            this.rSACE = null;
            this.fileLoaded = false;
        }

        private void PanelButtonsEnabled(Object enabled)
        {
            if (this.panel_Buttons.InvokeRequired)
            {
                Common.OneParamDelegate aPC = new Common.OneParamDelegate(this.PanelButtonsEnabled);
                this.panel_Buttons.Invoke(aPC, enabled);
            }
            else
            {
                this.panel_Buttons.Enabled = (Boolean)enabled;
            }
        }

        private void ButtonDecryptEnabled(Object enabled)
        {
            if (this.button_Decrypt.InvokeRequired)
            {
                Common.OneParamDelegate aPC = new Common.OneParamDelegate(this.ButtonDecryptEnabled);
                this.button_Decrypt.Invoke(aPC, enabled);
            }
            else
            {
                this.button_Decrypt.Enabled = (Boolean)enabled;
            }
        }

        private void ButtonEncryptEnabled(Object enabled)
        {
            if (this.button_Encrypt.InvokeRequired)
            {
                Common.OneParamDelegate aPC = new Common.OneParamDelegate(this.ButtonEncryptEnabled);
                this.button_Encrypt.Invoke(aPC, enabled);
            }
            else
            {
                this.button_Encrypt.Enabled = (Boolean)enabled;
            }
        }

        private void ButtonLoadDataEnabled(Object enabled)
        {
            if (this.button_LoadDataFromFile.InvokeRequired)
            {
                Common.OneParamDelegate aPC = new Common.OneParamDelegate(this.ButtonLoadDataEnabled);
                this.button_LoadDataFromFile.Invoke(aPC, enabled);
            }
            else
            {
                this.button_LoadDataFromFile.Enabled = (Boolean)enabled;
            }
        }

        private void ButtonPrepareAlgorithmEnabled(Object enabled)
        {
            if (this.button_PrepareAlgorithm.InvokeRequired)
            {
                Common.OneParamDelegate aPC = new Common.OneParamDelegate(this.ButtonPrepareAlgorithmEnabled);
                this.button_PrepareAlgorithm.Invoke(aPC, enabled);
            }
            else
            {
                this.button_PrepareAlgorithm.Enabled = (Boolean)enabled;
            }
        }

        private void ButtonOpenLoadedFileFolderEnabled(Object enabled)
        {
            if (this.button_OpenLoadedFileFolder.InvokeRequired)
            {
                Common.OneParamDelegate aPC = new Common.OneParamDelegate(this.ButtonOpenLoadedFileFolderEnabled);
                this.button_OpenLoadedFileFolder.Invoke(aPC, enabled);
            }
            else
            {
                this.button_OpenLoadedFileFolder.Enabled = (Boolean)enabled;
            }
        }

        private void RichTextBoxConsoleClear()
        {
            if (this.richTextBox_Console.InvokeRequired)
            {
                Common.GeneralDelegate gC = new Common.GeneralDelegate(this.RichTextBoxConsoleClear);
                this.richTextBox_Console.Invoke(gC);
            }
            else
            {
                this.richTextBox_Console.Clear();
            }
        }

        private void RichTextBoxConsoleAddText(Object text)
        {
            if (this.richTextBox_Console.InvokeRequired)
            {
                Common.OneParamDelegate aPC = new Common.OneParamDelegate(this.RichTextBoxConsoleAddText);
                this.richTextBox_Console.Invoke(aPC, text);
            }
            else
            {
                this.richTextBox_Console.Text += (String)text + "\n";
                RichTextBoxScrollToBottom();
            }
        }

        private void LabelKeySizeUpdate()
        {
            if (this.label_keySize.InvokeRequired)
            {
                Common.GeneralDelegate gC = new Common.GeneralDelegate(this.LabelKeySizeUpdate);
                this.label_keySize.Invoke(gC);
            }
            else
            {
                this.label_keySize.Text = String.Format("Key size: {0} bits", Properties.AppSettings.Default.keySize);
            }
        }

        private void UpdateCurrOperationStatus(Object text)
        {
            if (this.label_CurrentOperation.InvokeRequired)
            {
                Common.OneParamDelegate gC = new Common.OneParamDelegate(this.UpdateCurrOperationStatus);
                this.label_CurrentOperation.Invoke(gC, text);
            }
            else
            {
                this.label_CurrentOperation.Text = (String)text;
            }
        }

        private void RichTextBoxScrollToBottom()
        {
            if (this.richTextBox_Console.InvokeRequired)
            {
                Common.GeneralDelegate gC = new Common.GeneralDelegate(this.RichTextBoxScrollToBottom);
                this.richTextBox_Console.Invoke(gC);
            }
            else
            {
                WinAPI.SendMessage(this.richTextBox_Console.Handle, WinAPI.WM_VSCROLL, WinAPI.SB_BOTTOM, 0);
            }
        }

        private void SetProgressBarToMax()
        {
            if (this.progressBar_EncrDecrProgress.InvokeRequired)
            {
                Common.GeneralDelegate gC = new Common.GeneralDelegate(this.SetProgressBarToMax);
                this.progressBar_EncrDecrProgress.Invoke(gC);
            }
            else
            {
                this.progressBar_EncrDecrProgress.Value = this.progressBar_EncrDecrProgress.Maximum;
            }
        }

        private void DeleteTemporaryFiles()
        {
            try
            {
                // We assure ourselves that we have no other temporary encrypted and decrypted files.
                // Of course, we could generate a new random file name, but I don't see the need for the moment
                if (File.Exists(Properties.AppSettings.Default.encryptedFile))
                {
                    File.Delete(Properties.AppSettings.Default.encryptedFile);
                }
                if (File.Exists(Properties.AppSettings.Default.decryptedFile))
                {
                    File.Delete(Properties.AppSettings.Default.decryptedFile);
                }
            }
            catch (UnauthorizedAccessException exc)
            {
                exc.ToString();
            }
            catch (Exception exc)
            {
                exc.ToString();
            }
        }

        private void ShowWaitScreen(Object threadTOWait)
        {
            fWait = new Form_Wait();
            fWait.ClientSize = this.ClientSize;
            fWait.Location = this.Location;
            fWait.Show();
            
            while (runThread)
            {
                Thread.Sleep(1000);
            }

            fWait.Close();
            fWait.Dispose();
            fWait = null;
        }

        private void PrepareRSAAlgorithm()
        {
            rSACE.PrepareAlgorithm();
            runThread = false;
        }

        private void InitializeProgressBarAndTimer()
        {
            progressBar_EncrDecrProgress.Step = Properties.AppSettings.Default.encDecBlock;
            progressBar_EncrDecrProgress.Value = 0;
            timer1.Interval = 1000; //one second
            timer1.Start();
            pBPrev = progressBar_EncrDecrProgress.Value;
        }

        private void StepProgressBar()
        {
            if (this.progressBar_EncrDecrProgress.InvokeRequired)
            {
                Common.GeneralDelegate aPC = new Common.GeneralDelegate(this.StepProgressBar);
                this.Invoke(aPC);
            }
            else
            {
                this.progressBar_EncrDecrProgress.PerformStep();
            }
        }

        private void SetProgressBar(Object s)
        {
            if (this.progressBar_EncrDecrProgress.InvokeRequired)
            {
                Common.OneParamDelegate aPC = new Common.OneParamDelegate(this.SetProgressBar);
                this.Invoke(aPC, new object[] { s });
            }
            else
            {
                this.progressBar_EncrDecrProgress.Maximum = Int32.Parse((String)s);
            }
        }
        private void SetLabelTime(Object s)
        {
            if (this.label_RemainingTime.InvokeRequired)
            {
                Common.OneParamDelegate aPC = new Common.OneParamDelegate(this.SetLabelTime);
                this.Invoke(aPC, new object[] { s });
            }
            else
            {
                this.label_RemainingTime.Text = (String)s;
            }
        }

        /// <summary>
        /// This method verifies if the preconditions for starting the encryption are met
        /// </summary>
        /// <returns></returns>
        private Boolean EcryptionPreconditions()
        {
            Boolean result = true;

            if (rSACE.IsAlgorithmReady == true)
            {
                if ((Int32)(rSACE.ActionsAvailable & RSACustomEncryption.ActionsAvailableEnum.Encrypt) != 0)
                {
                    if (this.fileLoaded == true)
                    {
                        if (File.Exists(this.fileNameToEncrypt))
                        {
                            DeleteTemporaryFiles();
                        }
                        else
                        {
                            result = false;
                            MessageWindow.ShowError("File doesn't exist anymore!");
                        }
                    }
                    else
                    {
                        result = false;
                        MessageWindow.ShowError("No file selected to encrypt. Please select the file!");
                    }
                }
                else
                {
                    result = false;
                    MessageWindow.ShowError("Encryption not supported!");
                }
            }
            else
            {
                result = false;
                MessageWindow.ShowError("Algorithm not ready! Press Prepare Algorithm and try again!");
            }

            return result;
        }

        /// <summary>
        /// Method that verifies if Decryption preconditions are met
        /// </summary>
        /// <returns></returns>
        private Boolean DecryptionPreconditions()
        {
            Boolean result = true;
            String encryptedFile = Properties.AppSettings.Default.encryptedFile;
            String decryptedFile = Properties.AppSettings.Default.decryptedFile;

            if (this.rSACE.IsAlgorithmReady == true)
            {
                if ((Int32)(rSACE.ActionsAvailable & RSACustomEncryption.ActionsAvailableEnum.Decrypt) != 0)
                {
                    if (File.Exists(encryptedFile))
                    {
                        if (File.Exists(decryptedFile))
                        {
                            try
                            {
                                File.Delete(decryptedFile);
                            }
                            catch (UnauthorizedAccessException exc)
                            {
                                exc.ToString();
                                MessageWindow.ShowError(String.Format("Cannot access: {0}", decryptedFile));
                            }
                            catch (Exception exc)
                            {
                                exc.ToString();
                                MessageWindow.ShowError(String.Format("An error occured during deleting: {0}", decryptedFile));
                            }
                        }
                    }
                    else
                    {
                        result = false;
                        MessageWindow.ShowError("No file to decrypt!");
                    }
                }
                else
                {
                    result = false;
                    MessageWindow.ShowError("Decryption not supported!");
                }
            }
            else
            {
                result = false;
                MessageWindow.ShowError("Algorithm not ready!");
            }

            return result;
        }

        private void AbortOperationIfInProgress(Boolean showQuestion)
        {
            if (OperationInProgress())
            {
                if (showQuestion)
                    if(MessageWindow.ShowQuestion("Do you really want to cancel the operation?") == DialogResult.No)
                    return;

                RSAScheduler.Instance.AbortAllThreads();
                Common.RaiseAppEvent(Common.AppEventTypeEnum.OperationAborted, null);
            }
        }

        private Boolean OperationInProgress()
        {
            return (lastOperation == Common.AppEventTypeEnum.AlgoPreparationStarted
                || lastOperation == Common.AppEventTypeEnum.EncryptionStarted
                || lastOperation == Common.AppEventTypeEnum.DecryptionStarted);
        }

        private void ExportRSAData()
        {
            if (rSACE == null) return;

            TextWriter tWriter = null;

            switch (Properties.AppSettings.Default.impExpKeySetting)
            {
                case (Int32)Common.ImpExpKeyDataTypeEnum.BothKeys:
                    if (File.Exists(Properties.AppSettings.Default.encryptionKeysExportFileName))
                        File.Delete(Properties.AppSettings.Default.encryptionKeysExportFileName);
                    tWriter = new StreamWriter(Properties.AppSettings.Default.encryptionKeysExportFileName);
                    tWriter.WriteLine("e" + Properties.AppSettings.Default.splitCharInExpImpFiles + this.rSACE.GetE);
                    tWriter.WriteLine("n" + Properties.AppSettings.Default.splitCharInExpImpFiles + this.rSACE.GetN);
                    tWriter.WriteLine("d" + Properties.AppSettings.Default.splitCharInExpImpFiles + this.rSACE.GetD);
                    break;
                case (Int32)Common.ImpExpKeyDataTypeEnum.PrivateKey:
                    if (File.Exists(Properties.AppSettings.Default.privateKeyExportFileName))
                        File.Delete(Properties.AppSettings.Default.privateKeyExportFileName);
                    tWriter = new StreamWriter(Properties.AppSettings.Default.privateKeyExportFileName);
                    tWriter.WriteLine("d" + Properties.AppSettings.Default.splitCharInExpImpFiles + this.rSACE.GetD);
                    tWriter.WriteLine("n" + Properties.AppSettings.Default.splitCharInExpImpFiles + this.rSACE.GetN);
                    break;
                case (Int32)Common.ImpExpKeyDataTypeEnum.PublicKey:
                    if (File.Exists(Properties.AppSettings.Default.publicKeyExportFileName))
                        File.Delete(Properties.AppSettings.Default.publicKeyExportFileName);
                    tWriter = new StreamWriter(Properties.AppSettings.Default.publicKeyExportFileName);
                    tWriter.WriteLine("e" + Properties.AppSettings.Default.splitCharInExpImpFiles + this.rSACE.GetE);
                    tWriter.WriteLine("n" + Properties.AppSettings.Default.splitCharInExpImpFiles + this.rSACE.GetN);
                    break;
                default:
                    break;
            }

            tWriter.Close();

            Common.RaiseAppEvent(Common.AppEventTypeEnum.RSADataExported, null);
        }

        private void ImportRSAData()
        {
            try
            {
                String eC, n, d;
                TextReader tReader = null;

                switch (Properties.AppSettings.Default.impExpKeySetting)
                {
                    case (Int32)Common.ImpExpKeyDataTypeEnum.BothKeys:
                        if (!File.Exists(Properties.AppSettings.Default.encryptionKeysExportFileName))
                        {
                            MessageWindow.ShowError("No file to import: " + Properties.AppSettings.Default.encryptionKeysExportFileName);
                            break;
                        }

                        tReader = new StreamReader(Properties.AppSettings.Default.encryptionKeysExportFileName);
                        eC = tReader.ReadLine().Split(Properties.AppSettings.Default.splitCharInExpImpFiles)[1];
                        n = tReader.ReadLine().Split(Properties.AppSettings.Default.splitCharInExpImpFiles)[1];
                        d = tReader.ReadLine().Split(Properties.AppSettings.Default.splitCharInExpImpFiles)[1];
                        NewRSACustomEncryptionInstance(true, new Object[] { eC, n, d });
                        break;
                    case (Int32)Common.ImpExpKeyDataTypeEnum.PrivateKey:
                        if (!File.Exists(Properties.AppSettings.Default.encryptionKeysExportFileName))
                        {
                            MessageWindow.ShowError("No file to import: " + Properties.AppSettings.Default.privateKeyExportFileName);
                            break;
                        }

                        tReader = new StreamReader(Properties.AppSettings.Default.privateKeyExportFileName);
                        d = tReader.ReadLine().Split(Properties.AppSettings.Default.splitCharInExpImpFiles)[1];
                        n = tReader.ReadLine().Split(Properties.AppSettings.Default.splitCharInExpImpFiles)[1];
                        NewRSACustomEncryptionInstance(true, new Object[] { null, n, d });
                        break;
                    case (Int32)Common.ImpExpKeyDataTypeEnum.PublicKey:
                        if (!File.Exists(Properties.AppSettings.Default.encryptionKeysExportFileName))
                        {
                            MessageWindow.ShowError("No file to import: " + Properties.AppSettings.Default.publicKeyExportFileName);
                            break;
                        }

                        tReader = new StreamReader(Properties.AppSettings.Default.publicKeyExportFileName);
                        eC = tReader.ReadLine().Split(Properties.AppSettings.Default.splitCharInExpImpFiles)[1];
                        n = tReader.ReadLine().Split(Properties.AppSettings.Default.splitCharInExpImpFiles)[1];
                        NewRSACustomEncryptionInstance(true, new Object[] { eC, n, null });
                        break;
                    default:
                        break;
                }

                if (tReader != null) { tReader.Close(); Common.RaiseAppEvent(Common.AppEventTypeEnum.RSADataImported, null); }
            }
            catch (ArgumentOutOfRangeException exc)
            {
                exc.ToString();
                MessageWindow.ShowError("Corrupt import file");
            }
            catch (Exception exc)
            {
                exc.ToString();
            }
        }

        private void NewRSACustomEncryptionInstance(Boolean importedKeyData, Object[] keyData)
        {
            if (importedKeyData)
            {
                rSACE = new RSACustomEncryption(keyData);
            }
            else
            {
                rSACE = new RSACustomEncryption(keyData);
                rSACE.KeySize = Properties.AppSettings.Default.keySize;
            }
        }
        #endregion PRIVATE_METHODS
    }
}
