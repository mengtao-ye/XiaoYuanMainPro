using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 连衣裙
    /// </summary>
    public class PackagesSkinTarget : BaseSkinTarget
    {
        public PackagesSkinTarget(byte type) : base(type)
        {

        }

        protected override void ConfigSkinTargetDic()
        {
            AddSkinTarget(0, null);

            Dictionary<byte, Color> dict1 = new Dictionary<byte, Color>();
            dict1.Add(1, ColorTools.StrToColor("#16A1B7"));
            dict1.Add(2, ColorTools.StrToColor("#CA5C0A"));
            dict1.Add(3, ColorTools.StrToColor("#D053A2"));
            dict1.Add(4, ColorTools.StrToColor("#B4449C"));
            dict1.Add(5, ColorTools.StrToColor("#6E092D"));
            dict1.Add(6, ColorTools.StrToColor("#AD091F"));
            AddSkinTarget(1, dict1);

            Dictionary<byte, Color> dict2 = new Dictionary<byte, Color>();
            dict2.Add(1, ColorTools.StrToColor("#841B24"));
            dict2.Add(2, ColorTools.StrToColor("#4E4B4E"));
            dict2.Add(3, ColorTools.StrToColor("#2F637E"));
            dict2.Add(4, ColorTools.StrToColor("#B76320"));
            dict2.Add(5, ColorTools.StrToColor("#8B3E57"));
            dict2.Add(6, ColorTools.StrToColor("#700D18"));
            AddSkinTarget(2, dict2);

            Dictionary<byte, Color> dict3 = new Dictionary<byte, Color>();
            dict3.Add(1, ColorTools.StrToColor("#007EFF"));
            dict3.Add(2, ColorTools.StrToColor("#29B89B"));
            dict3.Add(3, ColorTools.StrToColor("#F26CC9"));
            dict3.Add(4, ColorTools.StrToColor("#49159F"));
            dict3.Add(5, ColorTools.StrToColor("#9B001B"));
            dict3.Add(6, ColorTools.StrToColor("#FEAC00"));
            AddSkinTarget(3, dict3);

            Dictionary<byte, Color> dict4 = new Dictionary<byte, Color>();
            dict4.Add(1, ColorTools.StrToColor("#59CDD9"));
            dict4.Add(2, ColorTools.StrToColor("#59BE23"));
            dict4.Add(3, ColorTools.StrToColor("#F77AC9"));
            dict4.Add(4, ColorTools.StrToColor("#8825C1"));
            dict4.Add(5, ColorTools.StrToColor("#CB1422"));
            dict4.Add(6, ColorTools.StrToColor("#F1AE08"));
            AddSkinTarget(4, dict4);

            Dictionary<byte, Color> dict5 = new Dictionary<byte, Color>();
            dict5.Add(1, ColorTools.StrToColor("#499CE1"));
            dict5.Add(2, ColorTools.StrToColor("#00798E"));
            dict5.Add(3, ColorTools.StrToColor("#B54708"));
            dict5.Add(4, ColorTools.StrToColor("#5C1C9D"));
            dict5.Add(5, ColorTools.StrToColor("#D11432"));
            dict5.Add(6, ColorTools.StrToColor("#1E398A"));
            AddSkinTarget(5, dict5);

            Dictionary<byte, Color> dict6 = new Dictionary<byte, Color>();
            dict6.Add(1, ColorTools.StrToColor("#348C15"));
            dict6.Add(2, ColorTools.StrToColor("#420574"));
            dict6.Add(3, ColorTools.StrToColor("#751119"));
            dict6.Add(4, ColorTools.StrToColor("#1C8EA1"));
            dict6.Add(5, ColorTools.StrToColor("#25358F"));
            dict6.Add(6, ColorTools.StrToColor("#AE337B"));
            AddSkinTarget(6, dict6);
        }
    }
}
