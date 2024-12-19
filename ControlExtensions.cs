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
        /// <summary>
        /// 容器內所有特定控制項Text值綁定實體資料模型屬性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="dataSource"></param>
        public static void AllBindToText<T>(this Control control, object dataSource) where T : Control
        {
            foreach (var t in control.GetAllControls<T>())
                t.BindToText(dataSource);
        }
        /// <summary>
        /// Text值綁定實體資料模型屬性值(雙向),須設定控制項Tag與屬性名相同,且須先設定屬性變更通知功能(可參考SqlDatamodel.cs)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="dataSource"></param>
        public static void BindToText<T>(this Control control, T dataSource)
        {
            string propertyName = control.Tag?.ToString();
        
            if (!string.IsNullOrEmpty(propertyName))
            {
                control.DataBindings.Add("Text", dataSource, propertyName, true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }
    }
}
