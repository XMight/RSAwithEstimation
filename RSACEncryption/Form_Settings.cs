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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RSACEncryption
{
    public partial class frm_Settings : Form
    {
        public frm_Settings()
        {
            InitializeComponent();
        }

        private void button_SaveSettings_Click(object sender, EventArgs e)
        {
            errorProvider_keySize.SetError(this.textBox_KeySize, "");
            errorProvider_NrThreads.SetError(this.textBox_NrThreads, "");

            UInt32 keySize;
            try
            {
                keySize = Properties.AppSettings.Default.keySize;
                Properties.AppSettings.Default.keySize = Convert.ToUInt32(this.textBox_KeySize.Text); ;
            }
            catch (Exception exc)
            {
                exc.ToString();
                errorProvider_keySize.SetError(this.textBox_KeySize, "Invalid integer value entered!");
                return;
            }

            if (checkBox_MultithreadedRSA.Checked)
            {
                if (Common.CountCores() < 2)
                {
                    MessageWindow.ShowError("No multithreaded RSA encryption is available on this processor");
                    checkBox_MultithreadedRSA.Checked = false;
                    return;
                }
            }
            Properties.AppSettings.Default.multithreadedRSA = checkBox_MultithreadedRSA.Checked;

            UInt32 nrThreads;
            try
            {
                nrThreads = checkBox_MultithreadedRSA.Checked ? Convert.ToUInt32(this.textBox_NrThreads.Text) : 1;
                Properties.AppSettings.Default.threadsRSA = nrThreads;
            }
            catch (Exception exc)
            {
                exc.ToString();
                errorProvider_NrThreads.SetError(this.textBox_NrThreads, "Invalid integer value entered!");
                return;
            }

            Properties.AppSettings.Default.inMemoryDecrypt = checkBox_InMemoryDecrypt.Checked;
            Properties.AppSettings.Default.impExpKeySetting = comboBox_ImpExpKeyDataType.SelectedIndex;

            Int32 keyBaseOld = Properties.AppSettings.Default.showKeyInBase;
            System.Reflection.FieldInfo[] fInfo = Common.ShowKeyInBaseEnum.Decimal.GetType().GetFields();
            Properties.AppSettings.Default.showKeyInBase = (Int32)(fInfo[comboBox_ShowKeyInBase.SelectedIndex + 1].GetValue(fInfo[comboBox_ShowKeyInBase.SelectedIndex + 1]));

            unchecked
            {
                Int32 tmpInt = 1;
                try
                {
                    tmpInt = Convert.ToInt32(textBox_EncDecBlock.Text);
                    if (tmpInt != 1)
                    {
                        MessageWindow.ShowError("Currently only one byte can be encrypted.");
                        tmpInt = 1;
                    }
                }
                catch (Exception exc)
                { 
                    exc.ToString();
                }

                Properties.AppSettings.Default.encDecBlock = tmpInt != 0 ? (Byte)tmpInt : (Byte)1;
            }

            Common.RaiseAppEvent(Common.AppEventTypeEnum.SettingsSaved, new Object[] { keySize, keyBaseOld });

            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {

        }

        private void checkBox_MultithreadedRSA_CheckedChanged(object sender, EventArgs e)
        {
            textBox_NrThreads.Enabled = checkBox_MultithreadedRSA.Checked;
        }

        private void frm_Settings_Load(object sender, EventArgs e)
        {
            textBox_KeySize.Text = Properties.AppSettings.Default.keySize.ToString();
            textBox_NrThreads.Text = Properties.AppSettings.Default.threadsRSA.ToString();
            checkBox_MultithreadedRSA.Checked = Properties.AppSettings.Default.multithreadedRSA;

            textBox_NrThreads.Enabled = checkBox_MultithreadedRSA.Checked;
            checkBox_InMemoryDecrypt.Checked = Properties.AppSettings.Default.inMemoryDecrypt;

            comboBox_ImpExpKeyDataType.Items.Add("Both keys");
            comboBox_ImpExpKeyDataType.Items.Add("Private key");
            comboBox_ImpExpKeyDataType.Items.Add("Public key");

            comboBox_ImpExpKeyDataType.SelectedIndex = Properties.AppSettings.Default.impExpKeySetting;

            comboBox_ShowKeyInBase.Items.Add("Decimal");
            comboBox_ShowKeyInBase.Items.Add("Hex");
            comboBox_ShowKeyInBase.Items.Add("Binary");

            System.Reflection.FieldInfo[] fInfo = Common.ShowKeyInBaseEnum.Decimal.GetType().GetFields();

            Int32 counter = 0;
            foreach(System.Reflection.FieldInfo fI in fInfo)
            {
                if ((Int32)(fInfo[counter + 1].GetValue(fInfo[counter + 1])) == Properties.AppSettings.Default.showKeyInBase)
                    break;
                counter++;
            }

            comboBox_ShowKeyInBase.SelectedIndex = counter;
            textBox_EncDecBlock.Text = Properties.AppSettings.Default.encDecBlock.ToString();
        }
    }
}
