using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 眼镜
    /// </summary>
    public class GlassessSkinTarget : BaseSkinTarget
    {
        public GlassessSkinTarget(byte type) : base(type)
        {
        }
        protected override void ConfigSkinTargetDic()
        {
            AddSkinTarget(0, null);

            Dictionary<byte, Color> dict1 = new Dictionary<byte, Color>();
            dict1.Add(1, ColorTools.StrToColor("#00736A"));
            dict1.Add(2, ColorTools.StrToColor("#212122"));
            dict1.Add(3, ColorTools.StrToColor("#B04705"));
            dict1.Add(4, ColorTools.StrToColor("#B52D78"));
            dict1.Add(5, ColorTools.StrToColor("#900019"));
            dict1.Add(6, ColorTools.StrToColor("#D69C33"));
            AddSkinTarget(1, dict1);

            Dictionary<byte, Color> dict2 = new Dictionary<byte, Color>();
            dict2.Add(1, ColorTools.StrToColor("#292929"));
            dict2.Add(2, ColorTools.StrToColor("#0066EA"));
            dict2.Add(3, ColorTools.StrToColor("#870C95"));
            dict2.Add(4, ColorTools.StrToColor("#A32526"));
            dict2.Add(5, ColorTools.StrToColor("#DADADF"));
            dict2.Add(6, ColorTools.StrToColor("#C59715"));
            AddSkinTarget(2, dict2);

            Dictionary<byte, Color> dict3 = new Dictionary<byte, Color>();
            dict3.Add(1, ColorTools.StrToColor("#0E0E0E"));
            dict3.Add(2, ColorTools.StrToColor("#0291A6"));
            dict3.Add(3, ColorTools.StrToColor("#C34D00"));
            dict3.Add(4, ColorTools.StrToColor("#4D1389"));
            dict3.Add(5, ColorTools.StrToColor("#8D0116"));
            dict3.Add(6, ColorTools.StrToColor("#AFAFAF"));
            AddSkinTarget(3, dict3);

            Dictionary<byte, Color> dict4 = new Dictionary<byte, Color>();
            dict4.Add(1, ColorTools.StrToColor("#179FEE"));
            dict4.Add(2, ColorTools.StrToColor("#82F913"));
            dict4.Add(3, ColorTools.StrToColor("#FC36FB"));
            dict4.Add(4, ColorTools.StrToColor("#D017DA"));
            dict4.Add(5, ColorTools.StrToColor("#E71212"));
            dict4.Add(6, ColorTools.StrToColor("#EDED33"));
            AddSkinTarget(4, dict4);

            Dictionary<byte, Color> dict5 = new Dictionary<byte, Color>();
            dict5.Add(1, ColorTools.StrToColor("#0094FF"));
            dict5.Add(2, ColorTools.StrToColor("#59FF00"));
            dict5.Add(3, ColorTools.StrToColor("#EF20DA"));
            dict5.Add(4, ColorTools.StrToColor("#7D0FA3"));
            dict5.Add(5, ColorTools.StrToColor("#F90000"));
            dict5.Add(6, ColorTools.StrToColor("#F1D121"));
            AddSkinTarget(5, dict5);

            Dictionary<byte, Color> dict6 = new Dictionary<byte, Color>();
            dict6.Add(1, ColorTools.StrToColor("#181818"));
            dict6.Add(2, ColorTools.StrToColor("#1C3180"));
            dict6.Add(3, ColorTools.StrToColor("#54664A"));
            dict6.Add(4, ColorTools.StrToColor("#9C3C08"));
            dict6.Add(5, ColorTools.StrToColor("#B2227C"));
            dict6.Add(6, ColorTools.StrToColor("#680011"));
            AddSkinTarget(6, dict6);
        }
    }
}
