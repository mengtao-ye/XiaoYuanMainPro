namespace Game
{
    /// <summary>
    /// 眼睛
    /// </summary>
    public class EyesSkinTarget : BaseSkinTarget
    {
        public EyesSkinTarget(byte type) : base(type)
        {
        }
        protected override void ConfigSkinTargetDic()
        {
            AddSkinTarget(1,null);
            AddSkinTarget(2, null);
            AddSkinTarget(3, null);
            AddSkinTarget(4, null);
            AddSkinTarget(5, null);
        }
    }
}
