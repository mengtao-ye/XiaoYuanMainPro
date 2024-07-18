using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 鞋子
    /// </summary>
    public class ShoesBuild : BaseChangeTargetPlayerBuild
    {
        private GameObject mCurTarget;
        private byte[] mPreDatas;
        private Material[] mMats;
        private bool mHasMultiMat;
        public ShoesBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }


        public override void Build(byte[] data)
        {
            if (mPreDatas != null && ByteTools.IsCompare(data, mPreDatas)) return;
            mPreDatas = data;
            HideAllChild();
            mCurTarget = mTargetParent.Find(data[0].ToString()).gameObject;
            mCurTarget.SetActiveExtend(true);
            switch (data[0])
            {
                case 1:
                case 5:
                case 6:
                    mHasMultiMat = false;
                    break;
                case 2:
                case 3:
                case 4:
                    mHasMultiMat = true;
                    break;
                default:
                    return;
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
