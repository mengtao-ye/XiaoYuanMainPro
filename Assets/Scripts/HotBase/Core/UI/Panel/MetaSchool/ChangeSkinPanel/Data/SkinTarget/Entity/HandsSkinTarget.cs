using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 手部
    /// </summary>
    public class HandsSkinTarget : BaseSkinTarget
    {
        public HandsSkinTarget(byte type) : base(type)
        {
        }
        protected override void ConfigSkinTargetDic()
        {
            AddSkinTarget(0, null);

            Dictionary<byte, Color> dict1 = new Dictionary<byte, Color>();
            dict1.Add(1, ColorTools.StrToColor("#343435"));
            dict1.Add(2, ColorTools.StrToColor("#233FA1"));
            dict1.Add(3, ColorTools.StrToColor("#D45999"));
            dict1.Add(4, ColorTools.StrToColor("#7C20B8"));
            dict1.Add(5, ColorTools.StrToColor("#8F001B"));
            dict1.Add(6, ColorTools.StrToColor("#9E6900"));
            AddSkinTarget(1, dict1);
            Dictionary<byte, Color> dict2 = new Dictionary<byte, Color>();
            dict2.Add(1, ColorTools.StrToColor("#2D2D2E"));
            dict2.Add(2, ColorTools.StrToColor("#328C0E"));
            dict2.Add(3, ColorTools.StrToColor("#D86609"));
            dict2.Add(4, ColorTools.StrToColor("#E55DB2"));
            dict2.Add(5, ColorTools.StrToColor("#591992"));
            dict2.Add(6, ColorTools.StrToColor("#90081A"));
            AddSkinTarget(2, dict2);
            Dictionary<byte, Color> dict3 = new Dictionary<byte, Color>();
            dict3.Add(1, ColorTools.StrToColor("#313132"));
            dict3.Add(2, ColorTools.StrToColor("#0396AD"));
            dict3.Add(3, ColorTools.StrToColor("#4F3A2B"));
            dict3.Add(4, ColorTools.StrToColor("#CB6F02"));
            dict3.Add(5, ColorTools.StrToColor("#581893"));
            dict3.Add(6, ColorTools.StrToColor("#B5B5B9"));
            AddSkinTarget(3, dict3);
            Dictionary<byte, Color> dict4 = new Dictionary<byte, Color>();
            dict4.Add(1, ColorTools.StrToColor("#0093BE"));
            dict4.Add(2, ColorTools.StrToColor("#4D7539"));
            dict4.Add(3, ColorTools.StrToColor("#792090"));
            dict4.Add(4, ColorTools.StrToColor("#860104"));
            dict4.Add(5, ColorTools.StrToColor("#808B96"));
            dict4.Add(6, ColorTools.StrToColor("#A59300"));
            AddSkinTarget(4, dict4);
          
        }
    }
}
