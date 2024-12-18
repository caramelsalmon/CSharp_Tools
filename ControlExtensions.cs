using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace YourNameSpace
{
    /// <summary>
    /// 控制項擴充
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// 遞迴取得控制項內特定類型物件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetAllControls<T>(this Control control) where T : Control
        {
            return control.GetAllControls()
                          .OfType<T>();
        }

        /// <summary>
        /// 遞迴取得控制項內所有物件
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private static IEnumerable<Control> GetAllControls(this Control control)
        {
            return new[] { control }
                .Concat(control.Controls.Cast<Control>()
                .SelectMany(c => c.GetAllControls()));  //   遞迴
        }

        /// <summary>
        /// 清空特定類型子控制項文字
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        public static void CelarAllText<T>(this Control control) where T : Control
        {
            foreach (var t in control.GetAllControls<T>())
                t.Text = string.Empty;
        }
    }
}
