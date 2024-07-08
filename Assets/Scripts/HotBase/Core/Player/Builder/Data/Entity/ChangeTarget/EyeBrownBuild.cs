using UnityEngine;
using YFramework;

namespace Game
{
    /// <summary>
    /// 眉毛
    /// </summary>
    public class EyeBrownBuild : BaseChangeTargetPlayerBuild
    {
        private GameObject mCurTarget;
        public EyeBrownBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {

        }

        public override void Build(byte[] data)
        {
            HideAllChild();
            mCurTarget = mTargetParent.Find(data[0].ToString()).gameObject;
            mCurTarget.SetActiveExtend(true);
            if (data[0] == 4) return;

        }
    }
}
