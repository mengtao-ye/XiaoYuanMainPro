using UnityEngine;
using YFramework;

namespace Game
{
    public static class ColorTools
    {
        /// <summary>
        /// 将颜色字符串转换成颜色 #FF00FF
        /// </summary>
        /// <param name="colorStr"></param>
        /// <returns></returns>
        public static Color StrToColor(string colorStr)
        {
            Color color = Color.white;
            if (colorStr.IsNullOrEmpty())
            {
                return color;
            }
            if (ColorUtility.TryParseHtmlString(colorStr, out color)) {
                return color;
            }
            return color;
        }
    }
}
