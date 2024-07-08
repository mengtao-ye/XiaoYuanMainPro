using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 上衣
    /// </summary>
    public class TopsSkinTarget : BaseSkinTarget
    {
        public TopsSkinTarget(byte type) : base(type)
        {

        }

        protected override void ConfigSkinTargetDic()
        {
            Dictionary<byte, Color> dict1 = new Dictionary<byte, Color>();
            dict1.Add(1, ColorTools.StrToColor("#3DA9E6"));
            dict1.Add(2, ColorTools.StrToColor("#FD8C46"));
            dict1.Add(3, ColorTools.StrToColor("#DA5F90"));
            dict1.Add(4, ColorTools.StrToColor("#BA383F"));
            dict1.Add(5, ColorTools.StrToColor("#E8FEFE"));
            dict1.Add(6, ColorTools.StrToColor("#FEA639"));
            AddSkinTarget(1, dict1);

            Dictionary<byte, Color> dict2 = new Dictionary<byte, Color>();
            dict2.Add(1, ColorTools.StrToColor("#23B0C6"));
            dict2.Add(2, ColorTools.StrToColor("#4AAB09"));
            dict2.Add(3, ColorTools.StrToColor("#CE5B97"));
            dict2.Add(4, ColorTools.StrToColor("#7F23B6"));
            dict2.Add(5, ColorTools.StrToColor("#9C071D"));
            dict2.Add(6, ColorTools.StrToColor("#DD8809"));
            AddSkinTarget(2, dict2);

            Dictionary<byte, Color> dict3 = new Dictionary<byte, Color>();
            dict3.Add(1, ColorTools.StrToColor("#71828C"));
            dict3.Add(2, ColorTools.StrToColor("#3A9609"));
            dict3.Add(3, ColorTools.StrToColor("#C043B2"));
            dict3.Add(4, ColorTools.StrToColor("#6D22A6"));
            dict3.Add(5, ColorTools.StrToColor("#8B61AD"));
            dict3.Add(6, ColorTools.StrToColor("#840919"));
            AddSkinTarget(3, dict3);

            Dictionary<byte, Color> dict4 = new Dictionary<byte, Color>();
            dict4.Add(1, ColorTools.StrToColor("#303031"));
            dict4.Add(2, ColorTools.StrToColor("#805678"));
            dict4.Add(3, ColorTools.StrToColor("#9D3F79"));
            dict4.Add(4, ColorTools.StrToColor("#701CA6"));
            dict4.Add(5, ColorTools.StrToColor("#9E001B"));
            dict4.Add(6, ColorTools.StrToColor("#CD9500"));
            AddSkinTarget(4, dict4);

            Dictionary<byte, Color> dict5 = new Dictionary<byte, Color>();
            dict5.Add(1, ColorTools.StrToColor("#1C1C1C"));
            dict5.Add(2, ColorTools.StrToColor("#132154"));
            dict5.Add(3, ColorTools.StrToColor("#1F2922"));
            dict5.Add(4, ColorTools.StrToColor("#74290C"));
            dict5.Add(5, ColorTools.StrToColor("#923970"));
            dict5.Add(6, ColorTools.StrToColor("#810B1A"));
            AddSkinTarget(5, dict5);

            Dictionary<byte, Color> dict6 = new Dictionary<byte, Color>();
            dict6.Add(1, ColorTools.StrToColor("#8E17CD"));
            dict6.Add(2, ColorTools.StrToColor("#A69500"));
            dict6.Add(3, ColorTools.StrToColor("#20AB85"));
            dict6.Add(4, ColorTools.StrToColor("#936100"));
            dict6.Add(5, ColorTools.StrToColor("#9A989E"));
            dict6.Add(6, ColorTools.StrToColor("#BEBC2A"));
            AddSkinTarget(6, dict6);
        }
    }
}
