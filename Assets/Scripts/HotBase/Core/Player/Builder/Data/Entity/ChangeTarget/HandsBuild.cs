using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 手部
    /// </summary>
    public class HandsBuild : BaseChangeTargetPlayerBuild
    {
        private GameObject mCurTarget;
        private byte[] mPreDatas;
        private bool mHasMultiMat;
        private Material[] mMats;
        public HandsBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        public override void Build(byte[] data)
        {
            if (mPreDatas != null && ByteTools.IsCompare(data, mPreDatas)) return;
            mPreDatas = data;
            HideAllChild();
            mCurTarget = mTargetParent.Find(data[0].ToString()).gameObject;
            mCurTarget.SetActiveExtend(true);

            if (data[0] == 0)
            {
                mCurTarget.GetComponent<SkinnedMeshRenderer>().sharedMaterial = mMats[0];
                return;
            }

            switch (data[0])
            {
                case 1:
                case 3:
                    mHasMultiMat = false;
                    break;
                case 2:
                case 4:
                    mHasMultiMat = true;
                    break;
            }
            string skinMatPath = SkinTools.GetSkinMaterialPath(Type, data[0], data[1]);
            ResourceHelper.AsyncLoadAsset<Material>(skinMatPath, LoadMatCallBack);
        }
        private void LoadMatCallBack(Material mat)
        {
            if (mHasMultiMat)
            {
                mMats[1] = mat;
                mCurTarget.GetComponent<SkinnedMeshRenderer>().sharedMaterials = mMats;
            }
            else
            {
                mCurTarget.GetComponent<SkinnedMeshRenderer>().material = mat;
            }
        }
        public override void SetSkinMat(Material material)
        {
            base.SetSkinMat(material);
            mMats = new Material[2];
            mMats[0] = material;
        }
    }
}
