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
            AddSkinTarget(1, CommonSkinColorMapper.Instance.data);
            AddSkinTarget(2, CommonSkinColorMapper.Instance.data);
            AddSkinTarget(3, CommonSkinColorMapper.Instance.data);
            AddSkinTarget(4, CommonSkinColorMapper.Instance.data);
        }
    }
}
