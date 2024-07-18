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
            AddSkinTarget(1, CommonSkinColorMapper.Instance.data);
            AddSkinTarget(2, CommonSkinColorMapper.Instance.data);
            AddSkinTarget(3, CommonSkinColorMapper.Instance.data);
            AddSkinTarget(4, CommonSkinColorMapper.Instance.data);
            AddSkinTarget(5, CommonSkinColorMapper.Instance.data);
            AddSkinTarget(6, CommonSkinColorMapper.Instance.data);
            AddSkinTarget(7, CommonSkinColorMapper.Instance.data);
            AddSkinTarget(8, CommonSkinColorMapper.Instance.data);
        }
    }
}
