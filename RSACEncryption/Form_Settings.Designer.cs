namespace RSACEncryption
{
    partial class frm_Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label_KeySize = new System.Windows.Forms.Label();
            this.textBox_KeySize = new System.Windows.Forms.TextBox();
            this.button_SaveSettings = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.checkBox_MultithreadedRSA = new System.Windows.Forms.CheckBox();
            this.label_Threads = new System.Windows.Forms.Label();
            this.textBox_NrThreads = new System.Windows.Forms.TextBox();
            this.errorProvider_keySize = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider_NrThreads = new System.Windows.Forms.ErrorProvider(this.components);
            this.checkBox_InMemoryDecrypt = new System.Windows.Forms.CheckBox();
            this.label_ImpExpKeyDataType = new System.Windows.Forms.Label();
            this.comboBox_ImpExpKeyDataType = new System.Windows.Forms.ComboBox();
            this.label_ShowKeyInBase = new System.Windows.Forms.Label();
            this.comboBox_ShowKeyInBase = new System.Windows.Forms.ComboBox();
            this.label_EncDecBlock = new System.Windows.Forms.Label();
            this.textBox_EncDecBlock = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider_keySize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider_NrThreads)).BeginInit();
            this.SuspendLayout();
            // 
            // label_KeySize
            // 
            this.label_KeySize.AutoSize = true;
            this.label_KeySize.Location = new System.Drawing.Point(9, 15);
            this.label_KeySize.Name = "label_KeySize";
            this.label_KeySize.Size = new System.Drawing.Size(55, 13);
            this.label_KeySize.TabIndex = 2;
            this.label_KeySize.Text = "[Key size:]";
            // 
            // textBox_KeySize
            // 
            this.textBox_KeySize.Location = new System.Drawing.Point(104, 12);
            this.textBox_KeySize.Name = "textBox_KeySize";
            this.textBox_KeySize.Size = new System.Drawing.Size(156, 20);
            this.textBox_KeySize.TabIndex = 3;
            this.textBox_KeySize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // button_SaveSettings
            // 
            this.button_SaveSettings.Location = new System.Drawing.Point(12, 184);
            this.button_SaveSettings.Name = "button_SaveSettings";
            this.button_SaveSettings.Size = new System.Drawing.Size(127, 23);
            this.button_SaveSettings.TabIndex = 0;
            this.button_SaveSettings.Text = "[Save]";
            this.button_SaveSettings.UseVisualStyleBackColor = true;
            this.button_SaveSettings.Click += new System.EventHandler(this.button_SaveSettings_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(145, 184);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(127, 23);
            this.button_Cancel.TabIndex = 1;
            this.button_Cancel.Text = "[Cancel]";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // checkBox_MultithreadedRSA
            // 
            this.checkBox_MultithreadedRSA.AutoSize = true;
            this.checkBox_MultithreadedRSA.Location = new System.Drawing.Point(12, 42);
            this.checkBox_MultithreadedRSA.Name = "checkBox_MultithreadedRSA";
            this.checkBox_MultithreadedRSA.Size = new System.Drawing.Size(96, 17);
            this.checkBox_MultithreadedRSA.TabIndex = 4;
            this.checkBox_MultithreadedRSA.Text = "[Multihtreaded]";
            this.checkBox_MultithreadedRSA.UseVisualStyleBackColor = true;
            this.checkBox_MultithreadedRSA.CheckedChanged += new System.EventHandler(this.checkBox_MultithreadedRSA_CheckedChanged);
            // 
            // label_Threads
            // 
            this.label_Threads.AutoSize = true;
            this.label_Threads.Location = new System.Drawing.Point(118, 42);
            this.label_Threads.Name = "label_Threads";
            this.label_Threads.Size = new System.Drawing.Size(52, 13);
            this.label_Threads.TabIndex = 5;
            this.label_Threads.Text = "[Threads]";
            // 
            // textBox_NrThreads
            // 
            this.textBox_NrThreads.Location = new System.Drawing.Point(176, 39);
            this.textBox_NrThreads.Name = "textBox_NrThreads";
            this.textBox_NrThreads.Size = new System.Drawing.Size(84, 20);
            this.textBox_NrThreads.TabIndex = 6;
            this.textBox_NrThreads.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // errorProvider_keySize
            // 
            this.errorProvider_keySize.ContainerControl = this;
            // 
            // errorProvider_NrThreads
            // 
            this.errorProvider_NrThreads.ContainerControl = this;
            // 
            // checkBox_InMemoryDecrypt
            // 
            this.checkBox_InMemoryDecrypt.AutoSize = true;
            this.checkBox_InMemoryDecrypt.Location = new System.Drawing.Point(12, 66);
            this.checkBox_InMemoryDecrypt.Name = "checkBox_InMemoryDecrypt";
            this.checkBox_InMemoryDecrypt.Size = new System.Drawing.Size(118, 17);
            this.checkBox_InMemoryDecrypt.TabIndex = 7;
            this.checkBox_InMemoryDecrypt.Text = "[In memory decrypt]";
            this.checkBox_InMemoryDecrypt.UseVisualStyleBackColor = true;
            // 
            // label_ImpExpKeyDataType
            // 
            this.label_ImpExpKeyDataType.AutoSize = true;
            this.label_ImpExpKeyDataType.Location = new System.Drawing.Point(12, 90);
            this.label_ImpExpKeyDataType.Name = "label_ImpExpKeyDataType";
            this.label_ImpExpKeyDataType.Size = new System.Drawing.Size(123, 13);
            this.label_ImpExpKeyDataType.TabIndex = 8;
            this.label_ImpExpKeyDataType.Text = "[Imp/Exp key data type:]";
            // 
            // comboBox_ImpExpKeyDataType
            // 
            this.comboBox_ImpExpKeyDataType.FormattingEnabled = true;
            this.comboBox_ImpExpKeyDataType.Location = new System.Drawing.Point(139, 87);
            this.comboBox_ImpExpKeyDataType.Name = "comboBox_ImpExpKeyDataType";
            this.comboBox_ImpExpKeyDataType.Size = new System.Drawing.Size(121, 21);
            this.comboBox_ImpExpKeyDataType.TabIndex = 9;
            // 
            // label_ShowKeyInBase
            // 
            this.label_ShowKeyInBase.AutoSize = true;
            this.label_ShowKeyInBase.Location = new System.Drawing.Point(12, 120);
            this.label_ShowKeyInBase.Name = "label_ShowKeyInBase";
            this.label_ShowKeyInBase.Size = new System.Drawing.Size(79, 13);
            this.label_ShowKeyInBase.TabIndex = 10;
            this.label_ShowKeyInBase.Text = "[Show keys in:]";
            // 
            // comboBox_ShowKeyInBase
            // 
            this.comboBox_ShowKeyInBase.FormattingEnabled = true;
            this.comboBox_ShowKeyInBase.Location = new System.Drawing.Point(139, 117);
            this.comboBox_ShowKeyInBase.Name = "comboBox_ShowKeyInBase";
            this.comboBox_ShowKeyInBase.Size = new System.Drawing.Size(121, 21);
            this.comboBox_ShowKeyInBase.TabIndex = 11;
            // 
            // label_EncDecBlock
            // 
            this.label_EncDecBlock.AutoSize = true;
            this.label_EncDecBlock.Location = new System.Drawing.Point(12, 151);
            this.label_EncDecBlock.Name = "label_EncDecBlock";
            this.label_EncDecBlock.Size = new System.Drawing.Size(89, 13);
            this.label_EncDecBlock.TabIndex = 12;
            this.label_EncDecBlock.Text = "[Enc/Dec block:]";
            // 
            // textBox_EncDecBlock
            // 
            this.textBox_EncDecBlock.Location = new System.Drawing.Point(139, 144);
            this.textBox_EncDecBlock.Name = "textBox_EncDecBlock";
            this.textBox_EncDecBlock.Size = new System.Drawing.Size(121, 20);
            this.textBox_EncDecBlock.TabIndex = 13;
            // 
            // frm_Settings
            // 
            this.AcceptButton = this.button_SaveSettings;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(284, 219);
            this.Controls.Add(this.textBox_EncDecBlock);
            this.Controls.Add(this.label_EncDecBlock);
            this.Controls.Add(this.comboBox_ShowKeyInBase);
            this.Controls.Add(this.label_ShowKeyInBase);
            this.Controls.Add(this.comboBox_ImpExpKeyDataType);
            this.Controls.Add(this.label_ImpExpKeyDataType);
            this.Controls.Add(this.checkBox_InMemoryDecrypt);
            this.Controls.Add(this.textBox_NrThreads);
            this.Controls.Add(this.label_Threads);
            this.Controls.Add(this.checkBox_MultithreadedRSA);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_SaveSettings);
            this.Controls.Add(this.textBox_KeySize);
            this.Controls.Add(this.label_KeySize);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frm_Settings";
            this.Text = "[Settings]";
            this.Load += new System.EventHandler(this.frm_Settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider_keySize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider_NrThreads)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_KeySize;
        private System.Windows.Forms.TextBox textBox_KeySize;
        private System.Windows.Forms.Button button_SaveSettings;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.CheckBox checkBox_MultithreadedRSA;
        private System.Windows.Forms.Label label_Threads;
        private System.Windows.Forms.TextBox textBox_NrThreads;
        private System.Windows.Forms.ErrorProvider errorProvider_keySize;
        private System.Windows.Forms.ErrorProvider errorProvider_NrThreads;
        private System.Windows.Forms.CheckBox checkBox_InMemoryDecrypt;
        private System.Windows.Forms.Label label_ImpExpKeyDataType;
        private System.Windows.Forms.ComboBox comboBox_ImpExpKeyDataType;
        private System.Windows.Forms.ComboBox comboBox_ShowKeyInBase;
        private System.Windows.Forms.Label label_ShowKeyInBase;
        private System.Windows.Forms.TextBox textBox_EncDecBlock;
        private System.Windows.Forms.Label label_EncDecBlock;
    }
}