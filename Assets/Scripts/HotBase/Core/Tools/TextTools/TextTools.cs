using UnityEngine;

namespace Game
{
    public static  class TextTools
    {
        public static string GetColorText(string content,Color color)
        {
            string colorStr = ColorUtility.ToHtmlStringRGBA(color);
            return $"<color=#{ colorStr}>{content}</color>";
        }
    }
}
