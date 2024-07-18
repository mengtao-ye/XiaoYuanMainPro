using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 背包
    /// </summary>
    public class PackagesBuild : BaseChangeTargetPlayerBuild
    {
        private GameObject mCurTarget;
        private Material mTargetMat;
        private byte[] mPreDatas;

        public PackagesBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        public override void Build(byte[] data)
        {
            if (mPreDatas != null && ByteTools.IsCompare(data, mPreDatas)) return;
            mPreDatas = data;
            HideAllChild();
            mCurTarget = mTargetParent.Find(data[0].ToString()).gameObject;
            mCurTarget.SetActiveExtend(true);
            if (data[0] == 0) return;
            string skinMatPath = SkinTools.GetSkinMaterialPath(Type, data[0], data[1]);
            mTargetMat = mCurTarget.GetComponent<SkinnedMeshRenderer>().materials[0];
            ResourceHelper.AsyncLoadAsset<Material>(skinMatPath, LoadMatCallBack);
        }
        private void LoadMatCallBack(Material mat)
        {
            mTargetMat.CopyPropertiesFromMaterial(mat);
        }
    }
}
