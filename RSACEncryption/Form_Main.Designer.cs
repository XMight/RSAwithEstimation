namespace RSACEncryption
{
    partial class Form_Main
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
            this.richTextBox_Console = new System.Windows.Forms.RichTextBox();
            this.button_Encrypt = new System.Windows.Forms.Button();
            this.button_Decrypt = new System.Windows.Forms.Button();
            this.button_PrepareAlgorithm = new System.Windows.Forms.Button();
            this.button_LoadDataFromFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel_Buttons = new System.Windows.Forms.Panel();
            this.button_OpenLoadedFileFolder = new System.Windows.Forms.Button();
            this.label_CurrentOperation = new System.Windows.Forms.Label();
            this.progressBar_EncrDecrProgress = new System.Windows.Forms.ProgressBar();
            this.label_RemainingTime = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportpublicKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importKeyDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.processorInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label_keySize = new System.Windows.Forms.Label();
            this.button_OpenWorkingDirectory = new System.Windows.Forms.Button();
            this.panel_Buttons.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox_Console
            // 
            this.richTextBox_Console.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_Console.Location = new System.Drawing.Point(13, 27);
            this.richTextBox_Console.Name = "richTextBox_Console";
            this.richTextBox_Console.ReadOnly = true;
            this.richTextBox_Console.Size = new System.Drawing.Size(319, 221);
            this.richTextBox_Console.TabIndex = 7;
            this.richTextBox_Console.Text = "";
            // 
            // button_Encrypt
            // 
            this.button_Encrypt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Encrypt.Location = new System.Drawing.Point(0, 61);
            this.button_Encrypt.Name = "button_Encrypt";
            this.button_Encrypt.Size = new System.Drawing.Size(179, 23);
            this.button_Encrypt.TabIndex = 3;
            this.button_Encrypt.Text = "[Encrypt]";
            this.button_Encrypt.UseVisualStyleBackColor = true;
            this.button_Encrypt.Click += new System.EventHandler(this.button_Encrypt_Click);
            // 
            // button_Decrypt
            // 
            this.button_Decrypt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Decrypt.Location = new System.Drawing.Point(0, 90);
            this.button_Decrypt.Name = "button_Decrypt";
            this.button_Decrypt.Size = new System.Drawing.Size(179, 23);
            this.button_Decrypt.TabIndex = 4;
            this.button_Decrypt.Text = "[Decrypt]";
            this.button_Decrypt.UseVisualStyleBackColor = true;
            this.button_Decrypt.Click += new System.EventHandler(this.button_Decrypt_Click);
            // 
            // button_PrepareAlgorithm
            // 
            this.button_PrepareAlgorithm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button_PrepareAlgorithm.Location = new System.Drawing.Point(0, 3);
            this.button_PrepareAlgorithm.Name = "button_PrepareAlgorithm";
            this.button_PrepareAlgorithm.Size = new System.Drawing.Size(179, 23);
            this.button_PrepareAlgorithm.TabIndex = 1;
            this.button_PrepareAlgorithm.Text = "[Prepare Algorithm]";
            this.button_PrepareAlgorithm.UseVisualStyleBackColor = true;
            this.button_PrepareAlgorithm.Click += new System.EventHandler(this.button_Prepare_Algorithm_Click);
            // 
            // button_LoadDataFromFile
            // 
            this.button_LoadDataFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button_LoadDataFromFile.Location = new System.Drawing.Point(0, 32);
            this.button_LoadDataFromFile.Name = "button_LoadDataFromFile";
            this.button_LoadDataFromFile.Size = new System.Drawing.Size(179, 23);
            this.button_LoadDataFromFile.TabIndex = 2;
            this.button_LoadDataFromFile.Text = "[Load File]";
            this.button_LoadDataFromFile.UseVisualStyleBackColor = true;
            this.button_LoadDataFromFile.Click += new System.EventHandler(this.button_LoadDataFromFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel_Buttons
            // 
            this.panel_Buttons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Buttons.Controls.Add(this.button_OpenWorkingDirectory);
            this.panel_Buttons.Controls.Add(this.button_OpenLoadedFileFolder);
            this.panel_Buttons.Controls.Add(this.button_PrepareAlgorithm);
            this.panel_Buttons.Controls.Add(this.button_Decrypt);
            this.panel_Buttons.Controls.Add(this.button_LoadDataFromFile);
            this.panel_Buttons.Controls.Add(this.button_Encrypt);
            this.panel_Buttons.Location = new System.Drawing.Point(343, 29);
            this.panel_Buttons.Name = "panel_Buttons";
            this.panel_Buttons.Size = new System.Drawing.Size(182, 179);
            this.panel_Buttons.TabIndex = 5;
            // 
            // button_OpenLoadedFileFolder
            // 
            this.button_OpenLoadedFileFolder.Location = new System.Drawing.Point(0, 120);
            this.button_OpenLoadedFileFolder.Name = "button_OpenLoadedFileFolder";
            this.button_OpenLoadedFileFolder.Size = new System.Drawing.Size(179, 23);
            this.button_OpenLoadedFileFolder.TabIndex = 5;
            this.button_OpenLoadedFileFolder.Text = "[Open Loaded File Folder]";
            this.button_OpenLoadedFileFolder.UseVisualStyleBackColor = true;
            this.button_OpenLoadedFileFolder.Click += new System.EventHandler(this.button_OpenLoadedFileFolder_Click);
            // 
            // label_CurrentOperation
            // 
            this.label_CurrentOperation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label_CurrentOperation.AutoSize = true;
            this.label_CurrentOperation.Location = new System.Drawing.Point(340, 213);
            this.label_CurrentOperation.Name = "label_CurrentOperation";
            this.label_CurrentOperation.Size = new System.Drawing.Size(61, 13);
            this.label_CurrentOperation.TabIndex = 7;
            this.label_CurrentOperation.Text = "[CurrentOp]";
            // 
            // progressBar_EncrDecrProgress
            // 
            this.progressBar_EncrDecrProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar_EncrDecrProgress.Location = new System.Drawing.Point(13, 254);
            this.progressBar_EncrDecrProgress.Name = "progressBar_EncrDecrProgress";
            this.progressBar_EncrDecrProgress.Size = new System.Drawing.Size(319, 23);
            this.progressBar_EncrDecrProgress.TabIndex = 8;
            // 
            // label_RemainingTime
            // 
            this.label_RemainingTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label_RemainingTime.AutoSize = true;
            this.label_RemainingTime.Location = new System.Drawing.Point(340, 254);
            this.label_RemainingTime.Name = "label_RemainingTime";
            this.label_RemainingTime.Size = new System.Drawing.Size(113, 13);
            this.label_RemainingTime.TabIndex = 6;
            this.label_RemainingTime.Text = "[Aprx. time remaining: ]";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aToolStripMenuItem,
            this.aToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(537, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aToolStripMenuItem
            // 
            this.aToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.exportpublicKeyToolStripMenuItem,
            this.importKeyDataToolStripMenuItem,
            this.abortToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.aToolStripMenuItem.Name = "aToolStripMenuItem";
            this.aToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.aToolStripMenuItem.Text = "[RSA]";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.settingsToolStripMenuItem.Text = "[Se&ttings]";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // exportpublicKeyToolStripMenuItem
            // 
            this.exportpublicKeyToolStripMenuItem.Name = "exportpublicKeyToolStripMenuItem";
            this.exportpublicKeyToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exportpublicKeyToolStripMenuItem.Text = "[&Export key data]";
            this.exportpublicKeyToolStripMenuItem.Click += new System.EventHandler(this.exportKeyDataToolStripMenuItem_Click);
            // 
            // importKeyDataToolStripMenuItem
            // 
            this.importKeyDataToolStripMenuItem.Name = "importKeyDataToolStripMenuItem";
            this.importKeyDataToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.importKeyDataToolStripMenuItem.Text = "[I&mport key data]";
            this.importKeyDataToolStripMenuItem.Click += new System.EventHandler(this.importKeyDataToolStripMenuItem_Click);
            // 
            // abortToolStripMenuItem
            // 
            this.abortToolStripMenuItem.Name = "abortToolStripMenuItem";
            this.abortToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.abortToolStripMenuItem.Text = "[&Abort]";
            this.abortToolStripMenuItem.Click += new System.EventHandler(this.abortToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exitToolStripMenuItem.Text = "[E&xit]";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aToolStripMenuItem1
            // 
            this.aToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processorInfoToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.aToolStripMenuItem1.Name = "aToolStripMenuItem1";
            this.aToolStripMenuItem1.Size = new System.Drawing.Size(52, 20);
            this.aToolStripMenuItem1.Text = "[Help]";
            // 
            // processorInfoToolStripMenuItem
            // 
            this.processorInfoToolStripMenuItem.Name = "processorInfoToolStripMenuItem";
            this.processorInfoToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.processorInfoToolStripMenuItem.Text = "[Processor &Info]";
            this.processorInfoToolStripMenuItem.Click += new System.EventHandler(this.processorInfoToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.aboutToolStripMenuItem.Text = "[&About]";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // label_keySize
            // 
            this.label_keySize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label_keySize.AutoSize = true;
            this.label_keySize.Location = new System.Drawing.Point(340, 235);
            this.label_keySize.Name = "label_keySize";
            this.label_keySize.Size = new System.Drawing.Size(58, 13);
            this.label_keySize.TabIndex = 10;
            this.label_keySize.Text = "[Key size: ]";
            // 
            // button_OpenWorkingDirectory
            // 
            this.button_OpenWorkingDirectory.Location = new System.Drawing.Point(0, 149);
            this.button_OpenWorkingDirectory.Name = "button_OpenWorkingDirectory";
            this.button_OpenWorkingDirectory.Size = new System.Drawing.Size(179, 23);
            this.button_OpenWorkingDirectory.TabIndex = 6;
            this.button_OpenWorkingDirectory.Text = "[Open Working Directory]";
            this.button_OpenWorkingDirectory.UseVisualStyleBackColor = true;
            this.button_OpenWorkingDirectory.Click += new System.EventHandler(this.button_OpenWorkingDirectory_Click);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 289);
            this.Controls.Add(this.label_keySize);
            this.Controls.Add(this.label_RemainingTime);
            this.Controls.Add(this.progressBar_EncrDecrProgress);
            this.Controls.Add(this.label_CurrentOperation);
            this.Controls.Add(this.panel_Buttons);
            this.Controls.Add(this.richTextBox_Console);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(553, 327);
            this.Name = "Form_Main";
            this.Text = "RSA Encryption Example";
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.panel_Buttons.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_Console;
        private System.Windows.Forms.Button button_Encrypt;
        private System.Windows.Forms.Button button_Decrypt;
        private System.Windows.Forms.Button button_PrepareAlgorithm;
        private System.Windows.Forms.Button button_LoadDataFromFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel_Buttons;
        private System.Windows.Forms.Label label_CurrentOperation;
        private System.Windows.Forms.ProgressBar progressBar_EncrDecrProgress;
        private System.Windows.Forms.Label label_RemainingTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label label_keySize;
        private System.Windows.Forms.ToolStripMenuItem processorInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportpublicKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importKeyDataToolStripMenuItem;
        private System.Windows.Forms.Button button_OpenLoadedFileFolder;
        private System.Windows.Forms.Button button_OpenWorkingDirectory;
    }
}

