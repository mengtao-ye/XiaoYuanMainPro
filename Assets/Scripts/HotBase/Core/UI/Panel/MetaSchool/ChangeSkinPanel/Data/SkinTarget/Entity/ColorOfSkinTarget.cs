namespace Game
{
    /// <summary>
    /// 肤色
    /// </summary>
    public class ColorOfSkinTarget : BaseSkinTarget
    {
        public ColorOfSkinTarget(byte type) : base(type)
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
            AddSkinTarget(9, null);
            AddSkinTarget(10, null);
            AddSkinTarget(11, null);
            AddSkinTarget(12, null);
            AddSkinTarget(13, null);
        }
    }
}
