using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 眉毛
    /// </summary>
    public class EyeBrownBuild : BaseChangeTargetPlayerBuild
    {
        private GameObject mCurTarget;
        private byte[] mPreDatas;

        public EyeBrownBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {

        }

        public override void Build(byte[] data)
        {
            if (mPreDatas != null && ByteTools.IsCompare(data, mPreDatas)) return;
            mPreDatas = data;
            HideAllChild();
            mCurTarget = mTargetParent.Find(data[0].ToString()).gameObject;
            mCurTarget.SetActiveExtend(true);
            if (data[0] == 4) return;
            mCurTarget.GetComponent<SkinnedMeshRenderer>().material.color = CommonSkinColorMapper.Instance.Get(data[1]);
        }
    }
}
