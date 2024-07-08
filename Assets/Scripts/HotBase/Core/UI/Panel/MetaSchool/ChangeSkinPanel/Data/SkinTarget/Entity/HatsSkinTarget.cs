using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 帽子
    /// </summary>
    public class HatsSkinTarget : BaseSkinTarget
    {
        public HatsSkinTarget(byte type) : base(type)
        {
        }
        protected override void ConfigSkinTargetDic()
        {
            AddSkinTarget(0, null);
                
            Dictionary<byte, Color> dict1 = new Dictionary<byte, Color>();
            dict1.Add(1, ColorTools.StrToColor("#919194"));
            dict1.Add(2, ColorTools.StrToColor("#004C5A"));
            dict1.Add(3, ColorTools.StrToColor("#2A3B30"));
            dict1.Add(4, ColorTools.StrToColor("#4E1586"));
            dict1.Add(5, ColorTools.StrToColor("#ACAAAE"));
            dict1.Add(6, ColorTools.StrToColor("#A87100"));
            AddSkinTarget(1, dict1);

            Dictionary<byte, Color> dict2 = new Dictionary<byte, Color>();
            dict2.Add(1, ColorTools.StrToColor("#095F6F"));
            dict2.Add(2, ColorTools.StrToColor("#30382A"));
            dict2.Add(3, ColorTools.StrToColor("#BB4766"));
            dict2.Add(4, ColorTools.StrToColor("#582728"));
            dict2.Add(5, ColorTools.StrToColor("#45147C"));
            dict2.Add(6, ColorTools.StrToColor("#A97300"));
            AddSkinTarget(2, dict2);

            Dictionary<byte, Color> dict3 = new Dictionary<byte, Color>();
            dict3.Add(1, ColorTools.StrToColor("#421585"));
            dict3.Add(2, ColorTools.StrToColor("#75141B"));
            dict3.Add(3, ColorTools.StrToColor("#DA9521"));
            dict3.Add(4, ColorTools.StrToColor("#88AACC"));
            dict3.Add(5, ColorTools.StrToColor("#869E92"));
            dict3.Add(6, ColorTools.StrToColor("#C695B6"));
            AddSkinTarget(3, dict3);

            Dictionary<byte, Color> dict4 = new Dictionary<byte, Color>();
            dict4.Add(1, ColorTools.StrToColor("#008499"));
            dict4.Add(2, ColorTools.StrToColor("#503528"));
            dict4.Add(3, ColorTools.StrToColor("#A35908"));
            dict4.Add(4, ColorTools.StrToColor("#9E3A69"));
            dict4.Add(5, ColorTools.StrToColor("#56188B"));
            dict4.Add(6, ColorTools.StrToColor("#BBBBBD"));
            AddSkinTarget(4, dict4);
        }
    }
}
