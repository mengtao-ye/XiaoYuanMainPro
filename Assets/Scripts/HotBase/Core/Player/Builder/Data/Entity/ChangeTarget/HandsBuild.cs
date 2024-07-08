using UnityEngine;
using YFramework;

namespace Game
{
    public class HandsBuild : BaseChangeTargetPlayerBuild
    {
        private GameObject mCurTarget;
        private Material mTargetMat;

        public HandsBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        public override void Build(byte[] data)
        {
            HideAllChild();
            mCurTarget = mTargetParent.Find(data[0].ToString()).gameObject;
            mCurTarget.SetActiveExtend(true);
            switch (data[0])
            {
                case 1:
                case 3:
                    mTargetMat = mCurTarget.GetComponent<SkinnedMeshRenderer>().materials[0];
                    break;
                case 2:
                case 4:
                    mTargetMat = mCurTarget.GetComponent<SkinnedMeshRenderer>().materials[1];
                    break;
                default:
                    return;
            }
            string skinMatPath = SkinTools.GetSkinMaterialPath(Type, data[0], data[1]);
            ResourceHelper.AsyncLoadAsset<Material>(skinMatPath, LoadMatCallBack);
        }
        private void LoadMatCallBack(Material mat)
        {
            mTargetMat = mat;
        }
    }
}
