using UnityEngine;
using YFramework;

namespace Game
{
    public class CommonSkinColorMapper : SingleMap<byte, Color, CommonSkinColorMapper>
    {
        protected override void Config()
        {
            Add(1,ColorTools.StrToColor("#010101"));
            Add(2,ColorTools.StrToColor("#FEC02D"));
            Add(3,ColorTools.StrToColor("#FF8B2C"));
            Add(4, ColorTools.StrToColor("#D4350E"));
            Add(5, ColorTools.StrToColor("#EF5CA0"));
            Add(6, ColorTools.StrToColor("#8700C9"));
            Add(7, ColorTools.StrToColor("#A1B00B"));
            Add(8, ColorTools.StrToColor("#195FCF"));
            Add(9, ColorTools.StrToColor("#924608"));
            Add(10, ColorTools.StrToColor("#A1A5A4"));
            Add(11, ColorTools.StrToColor("#FFFFFF"));
        }
    }
}
