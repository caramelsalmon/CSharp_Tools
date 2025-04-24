using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace XXX
{
    static class Program
    {
        // Windows 8 以上版本 API
        [DllImport("shcore.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SetProcessDpiAwareness(int awareness);
        private const int PROCESS_DPI_UNAWARE = 0;

        // Windows 7 API
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                //  根據作業系統版本選擇適當的 DPI 設定方法
                if (Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 2)
                {
                    //  Windows 8 及以上版本
                    SetProcessDpiAwareness(PROCESS_DPI_UNAWARE);
                }
                else
                {
                    //  Windows 7 或更早版本
                    SetProcessDPIAware();
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Forms1());
            }
            catch (DllNotFoundException ex)
            {
                string logPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "dll_error_log.txt");

                File.WriteAllText(logPath,
                    $"錯誤時間：{DateTime.Now}\n" +
                    $"錯誤訊息：{ex.Message}\n" +
                    $"缺少的 DLL：{ex.ToString()}\n" +
                    $"堆疊追蹤：{ex.StackTrace}");

                MessageBox.Show($"程式載入 DLL 時發生錯誤，詳細資訊已記錄到桌面的 dll_error_log.txt 檔案中。\n\n錯誤訊息：{ex.Message}",
                    "DLL 載入錯誤",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string logPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "error_log.txt");

                File.WriteAllText(logPath,
                    $"錯誤時間：{DateTime.Now}\n" +
                    $"錯誤類型：{ex.GetType().FullName}\n" +
                    $"錯誤訊息：{ex.Message}\n" +
                    $"堆疊追蹤：{ex.StackTrace}");

                MessageBox.Show($"程式執行時發生錯誤，詳細資訊已記錄到桌面的 error_log.txt 檔案中。\n\n錯誤訊息：{ex.Message}",
                    "應用程式錯誤",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
