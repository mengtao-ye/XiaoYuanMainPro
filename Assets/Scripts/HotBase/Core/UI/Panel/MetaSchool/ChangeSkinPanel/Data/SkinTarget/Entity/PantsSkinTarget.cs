using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 裤子
    /// </summary>
    public class PantsSkinTarget : BaseSkinTarget
    {
        public PantsSkinTarget(byte type) : base(type)
        {
        }
        protected override void ConfigSkinTargetDic()
        {
            Dictionary<byte, Color> dict1 = new Dictionary<byte, Color>();
            dict1.Add(1,ColorTools.StrToColor("#1D5C5D"));
            dict1.Add(2, ColorTools.StrToColor("#621A1B"));
            dict1.Add(3, ColorTools.StrToColor("#35404E"));
            dict1.Add(4, ColorTools.StrToColor("#4A2F50"));
            dict1.Add(5, ColorTools.StrToColor("#8E8975"));
            dict1.Add(6, ColorTools.StrToColor("#3A3C2E"));
            AddSkinTarget(1, dict1);
            Dictionary<byte, Color> dict2 = new Dictionary<byte, Color>();
            dict2.Add(1, ColorTools.StrToColor("#1E80A1"));
            dict2.Add(2, ColorTools.StrToColor("#70EF56"));
            dict2.Add(3, ColorTools.StrToColor("#454445"));
            dict2.Add(4, ColorTools.StrToColor("#7E3AAF"));
            dict2.Add(5, ColorTools.StrToColor("#821B21"));
            dict2.Add(6, ColorTools.StrToColor("#B8871B"));
            AddSkinTarget(2, dict2);
            Dictionary<byte, Color> dict3 = new Dictionary<byte, Color>();
            dict3.Add(1, ColorTools.StrToColor("#0089AA"));
            dict3.Add(2, ColorTools.StrToColor("#4E6F33"));
            dict3.Add(3, ColorTools.StrToColor("#640280"));
            dict3.Add(4, ColorTools.StrToColor("#8E0301"));
            dict3.Add(5, ColorTools.StrToColor("#838589"));
            dict3.Add(6, ColorTools.StrToColor("#AC9000"));
            AddSkinTarget(3, dict3);
            Dictionary<byte, Color> dict4 = new Dictionary<byte, Color>();
            dict4.Add(1, ColorTools.StrToColor("#1D251F"));
            dict4.Add(2, ColorTools.StrToColor("#0D8399"));
            dict4.Add(3, ColorTools.StrToColor("#4D3E2B"));
            dict4.Add(4, ColorTools.StrToColor("#E45818"));
            dict4.Add(5, ColorTools.StrToColor("#DB58BE"));
            dict4.Add(6, ColorTools.StrToColor("#DB1130"));
            AddSkinTarget(4, dict4);
            Dictionary<byte, Color> dict5= new Dictionary<byte, Color>();
            dict5.Add(1, ColorTools.StrToColor("#086474"));
            dict5.Add(2, ColorTools.StrToColor("#445D45"));
            dict5.Add(3, ColorTools.StrToColor("#B74B8F"));
            dict5.Add(4, ColorTools.StrToColor("#4F08A8"));
            dict5.Add(5, ColorTools.StrToColor("#7A0818"));
            dict5.Add(6, ColorTools.StrToColor("#C28600"));
            AddSkinTarget(5, dict5);
            Dictionary<byte, Color> dict6 = new Dictionary<byte, Color>();
            dict6.Add(1, ColorTools.StrToColor("#233A97"));
            dict6.Add(2, ColorTools.StrToColor("#817E7A"));
            dict6.Add(3, ColorTools.StrToColor("#2B2C31"));
            dict6.Add(4, ColorTools.StrToColor("#306D13"));
            dict6.Add(5, ColorTools.StrToColor("#AA470D"));
            dict6.Add(6, ColorTools.StrToColor("#690C1E"));
            AddSkinTarget(6, dict6);
        }
    }
}
