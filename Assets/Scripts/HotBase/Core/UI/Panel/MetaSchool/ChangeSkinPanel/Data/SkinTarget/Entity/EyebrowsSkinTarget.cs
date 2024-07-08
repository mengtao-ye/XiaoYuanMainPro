namespace Game
{
    /// <summary>
    /// 眉毛
    /// </summary>
    public class EyebrowsSkinTarget : BaseSkinTarget
    {
        public EyebrowsSkinTarget(byte type) : base(type)
        {
        }
        protected override void ConfigSkinTargetDic()
        {
            AddSkinTarget(1,null);
            AddSkinTarget(2,null);
            AddSkinTarget(3,null);
            AddSkinTarget(4,null);
        }
    }
}
