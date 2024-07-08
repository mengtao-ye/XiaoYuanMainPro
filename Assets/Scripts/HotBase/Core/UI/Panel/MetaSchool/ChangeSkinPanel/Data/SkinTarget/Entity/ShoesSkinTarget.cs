using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    /// <summary>
    /// 鞋子
    /// </summary>
    public class ShoesSkinTarget : BaseSkinTarget
    {
        public ShoesSkinTarget(byte type) : base(type)
        {

        }

        protected override void ConfigSkinTargetDic()
        {
            AddSkinTarget(0, null);
            Dictionary<byte, Color> dict1 = new Dictionary<byte, Color>();
            dict1.Add(1, ColorTools.StrToColor("#343435"));
            dict1.Add(2, ColorTools.StrToColor("#15A2D7"));
            dict1.Add(3, ColorTools.StrToColor("#653931"));
            dict1.Add(4, ColorTools.StrToColor("#A50E20"));
            dict1.Add(5, ColorTools.StrToColor("#B8C7CD"));
            dict1.Add(6, ColorTools.StrToColor("#E2AA0D"));
            AddSkinTarget(1, dict1);
            Dictionary<byte, Color> dict2 = new Dictionary<byte, Color>();
            dict2.Add(1, ColorTools.StrToColor("#C025AF"));
            dict2.Add(2, ColorTools.StrToColor("#9A1020"));
            dict2.Add(3, ColorTools.StrToColor("#2E7455"));
            dict2.Add(4, ColorTools.StrToColor("#127498"));
            dict2.Add(5, ColorTools.StrToColor("#BC8112"));
            dict2.Add(6, ColorTools.StrToColor("#95101F"));
            AddSkinTarget(2, dict2);
            Dictionary<byte, Color> dict3 = new Dictionary<byte, Color>();
            dict3.Add(1, ColorTools.StrToColor("#323233"));
            dict3.Add(2, ColorTools.StrToColor("#0B86BD"));
            dict3.Add(3, ColorTools.StrToColor("#0C9D32"));
            dict3.Add(4, ColorTools.StrToColor("#D5670B"));
            dict3.Add(5, ColorTools.StrToColor("#AE35A0"));
            dict3.Add(6, ColorTools.StrToColor("#750B27"));
            AddSkinTarget(3, dict3);
            Dictionary<byte, Color> dict4 = new Dictionary<byte, Color>();
            dict4.Add(1, ColorTools.StrToColor("#00ACDA"));
            dict4.Add(2, ColorTools.StrToColor("#97BA83"));
            dict4.Add(3, ColorTools.StrToColor("#C881F2"));
            dict4.Add(4, ColorTools.StrToColor("#A50606"));
            dict4.Add(5, ColorTools.StrToColor("#9A9FA6"));
            dict4.Add(6, ColorTools.StrToColor("#CCAE00"));
            AddSkinTarget(4, dict4);
            Dictionary<byte, Color> dict5 = new Dictionary<byte, Color>();
            dict5.Add(1, ColorTools.StrToColor("#1E1E1E"));
            dict5.Add(2, ColorTools.StrToColor("#3BA2B8"));
            dict5.Add(3, ColorTools.StrToColor("#4F3320"));
            dict5.Add(4, ColorTools.StrToColor("#7D0B19"));
            dict5.Add(5, ColorTools.StrToColor("#8F959B"));
            dict5.Add(6, ColorTools.StrToColor("#B2790A"));
            AddSkinTarget(5, dict5);
            Dictionary<byte, Color> dict6 = new Dictionary<byte, Color>();
            dict6.Add(1, ColorTools.StrToColor("#9D1932"));
            dict6.Add(2, ColorTools.StrToColor("#60B1C1"));
            dict6.Add(3, ColorTools.StrToColor("#4BBE1B"));
            dict6.Add(4, ColorTools.StrToColor("#872BBD"));
            dict6.Add(5, ColorTools.StrToColor("#CD31A7"));
            dict6.Add(6, ColorTools.StrToColor("#D1D1D6"));
            AddSkinTarget(6, dict6);
        }
    }
}
