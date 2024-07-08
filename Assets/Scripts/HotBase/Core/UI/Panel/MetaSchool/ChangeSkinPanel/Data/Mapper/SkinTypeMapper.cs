using YFramework;

namespace Game
{
    public class SkinTypeMapper : Map<byte, ISkinTarget>
    {
        private ChangeSkinPanel mPanel;
        public SkinTypeMapper(ChangeSkinPanel panel) : base()
        {
            mPanel = panel;
        }
        protected override void Config()
        {
            Add(1,new TopsSkinTarget(1));
            Add(2,new PantsSkinTarget(2));
            Add(3,new ShoesSkinTarget(3));
            Add(4,new HandsSkinTarget(4));
            Add(5,new PackagesSkinTarget(5));
            Add(6, new BodySuitsSkinTarget(6));
            Add(7,new HairsSkinTarget(7));
            Add(8,new HatsSkinTarget(8));
            Add(9,new GlassessSkinTarget(9));
            Add(10,new EyesSkinTarget(10));
            Add(11,new ColorOfSkinTarget(11));
            Add(13,new EyebrowsSkinTarget(13));
        }
    }
}
