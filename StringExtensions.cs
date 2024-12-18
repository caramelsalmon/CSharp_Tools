using System.Runtime.InteropServices;

namespace YourNameSpace
{
    public static class StringExtensions
    {
        private const int LocaleSystemDefault = 0x0800;
        private const int LcmapSimplifiedChinese = 0x02000000;
        private const int LcmapTraditionalChinese = 0x04000000;

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LCMapString(int locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);

        /// <summary>
        /// 轉簡體
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToSimplified(this string ori)
        {
            if (string.IsNullOrEmpty(ori)) return ori;

            var result = new string(' ', ori.Length);
            LCMapString(LocaleSystemDefault, LcmapSimplifiedChinese, ori, ori.Length, result, ori.Length);
            return result;
        }

        /// <summary>
        /// 轉繁體
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToTraditional(this string ori)
        {
            if (string.IsNullOrEmpty(ori)) return ori;

            var result = new string(' ', ori.Length);
            LCMapString(LocaleSystemDefault, LcmapTraditionalChinese, ori, ori.Length, result, ori.Length);
            return result;
        }
    }
}
