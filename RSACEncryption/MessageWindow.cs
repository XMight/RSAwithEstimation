using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace RSACEncryption
{
    /// <summary>
    /// The class that must be called in all the cases when a messageBox must be displayed
    /// </summary>
    public static class MessageWindow
    {
        public static void ShowError(String message)
        {
            System.Windows.Forms.MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ShowQuestion(String message)
        {
            return System.Windows.Forms.MessageBox.Show(message, "Confirmation!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        public static void ShowInfo(String message)
        {
            System.Windows.Forms.MessageBox.Show(message, "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
