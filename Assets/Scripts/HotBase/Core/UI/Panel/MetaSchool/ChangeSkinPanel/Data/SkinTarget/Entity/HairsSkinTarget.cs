namespace Game
{
    /// <summary>
    /// 发型
    /// </summary>
    public class HairsSkinTarget : BaseSkinTarget
    {
        public HairsSkinTarget(byte type) : base(type)
        {
        }
        protected override void ConfigSkinTargetDic()
        {
            AddSkinTarget(1, null);
            AddSkinTarget(2, null);
            AddSkinTarget(3, null);
            AddSkinTarget(4, null);
            AddSkinTarget(5, null);
            AddSkinTarget(6, null);
            AddSkinTarget(7, null);
            AddSkinTarget(8, null);
        }
    }
}
