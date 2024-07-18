using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 连体衣
    /// </summary>
    public class BodySuitsBuild : BaseChangeTargetPlayerBuild
    {
        private GameObject mCurTarget;
        private byte[] mPreDatas;
        private Material[] mMats;
        public BodySuitsBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
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
                mPlayerBuilder.ShowCloth();
                return;
            }

            mPlayerBuilder.ShowBodySuit();
            string skinMatPath = SkinTools.GetSkinMaterialPath(Type, data[0], data[1]);
            ResourceHelper.AsyncLoadAsset<Material>(skinMatPath, LoadMatCallBack);
        }
        private void LoadMatCallBack(Material mat)
        {
            mMats[1] = mat;
            mCurTarget.GetComponent<SkinnedMeshRenderer>().sharedMaterials = mMats;
        }
        public override void SetSkinMat(Material material)
        {
            base.SetSkinMat(material);
            mMats = new Material[2];
            mMats[0] = material;
        }
    }
}
