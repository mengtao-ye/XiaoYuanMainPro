using UnityEngine;
using YFramework;

namespace Game
{
    public class TopsBuild : BaseChangeTargetPlayerBuild
    {
        private GameObject mCurTarget;

        public TopsBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        public override void Build(byte[] data)
        {
            HideAllChild();
            mCurTarget = mTargetParent.Find(data[0].ToString()).gameObject;
            mCurTarget.SetActiveExtend(true);
            string skinMatPath = SkinTools.GetSkinMaterialPath(Type, data[0], data[1]);
            ResourceHelper.AsyncLoadAsset<Material>(skinMatPath, LoadMatCallBack);
        }
        private void LoadMatCallBack(Material mat)
        {
            mCurTarget.GetComponent<SkinnedMeshRenderer>().materials[1] = mat;
        }
    }
}
