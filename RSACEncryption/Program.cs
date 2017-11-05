using System;
using System.Windows.Forms;

namespace RSACEncryption
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form_Main());
            }
            catch (Exception exc)
            {
                exc.ToString();
            }
        }
    }
}
