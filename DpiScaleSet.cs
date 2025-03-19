using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace XXX
{
    static class Program
    {
        [DllImport("shcore.dll")]
        private static extern int SetProcessDpiAwareness(int awareness);

        private const int PROCESS_DPI_UNAWARE = 0;
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetProcessDpiAwareness(PROCESS_DPI_UNAWARE);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.vLogin());
        }
    }
}
