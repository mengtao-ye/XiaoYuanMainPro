using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 背包
    /// </summary>
    public class BodySuitsSkinTarget : BaseSkinTarget
    {
        public BodySuitsSkinTarget(byte type) : base(type)
        {

        }

        protected override void ConfigSkinTargetDic()
        {
            AddSkinTarget(0, null);

            Dictionary<byte, Color> dict1 = new Dictionary<byte, Color>();
            dict1.Add(1, ColorTools.StrToColor("#2DA5BB"));
            dict1.Add(2, ColorTools.StrToColor("#628F47"));
            dict1.Add(3, ColorTools.StrToColor("#DF7010"));
            dict1.Add(4, ColorTools.StrToColor("#A33E71"));
            dict1.Add(5, ColorTools.StrToColor("#BB28BC"));
            dict1.Add(6, ColorTools.StrToColor("#C0142C"));
            AddSkinTarget(1, dict1);

            Dictionary<byte, Color> dict2 = new Dictionary<byte, Color>();
            dict2.Add(1, ColorTools.StrToColor("#1D90C5"));
            dict2.Add(2, ColorTools.StrToColor("#972268"));
            dict2.Add(3, ColorTools.StrToColor("#6A1EA0"));
            dict2.Add(4, ColorTools.StrToColor("#71848D"));
            dict2.Add(5, ColorTools.StrToColor("#8C300C"));
            dict2.Add(6, ColorTools.StrToColor("#720614"));
            AddSkinTarget(2, dict2);

            Dictionary<byte, Color> dict3 = new Dictionary<byte, Color>();
            dict3.Add(1, ColorTools.StrToColor("#83001B"));
            dict3.Add(2, ColorTools.StrToColor("#A26C00"));
            dict3.Add(3, ColorTools.StrToColor("#212123"));
            dict3.Add(4, ColorTools.StrToColor("#236900"));
            dict3.Add(5, ColorTools.StrToColor("#0099B0"));
            dict3.Add(6, ColorTools.StrToColor("#A63A09"));
            AddSkinTarget(3, dict3);

        }
    }
}
