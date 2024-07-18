using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 帽子
    /// </summary>
    public class HatsBuild : BaseInstantiateTargetPlayerBuild
    {
        private GameObject mCurHat;
        public bool hasHat { get; private set; }
        private byte mType1;
        private byte mType2;
        private byte[] mPreDatas;

        public HatsBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        public override void Build(byte[] data)
        {
            if (mPreDatas != null && ByteTools.IsCompare(data, mPreDatas)) return;
            mPreDatas = data;
            if (mType1 != data[0] && mCurHat != null)
            {
                GameObject.DestroyImmediate(mCurHat);
                mCurHat = null;
            }
            if (data[0] == 0)
            {
                hasHat = false;
                mPlayerBuilder.SetHairNormal();
                return;
            }
            mPlayerBuilder.SetHairHeadless();
            hasHat = true;
            mType1 = data[0];
            mType2 = data[1];
            string modelPath = SkinTools.GetSkinModelPath(Type, mType1, mType2);
            if (mCurHat == null)
            {
                ResourcesLoadHelper.AsyncLoadAsset<GameObject>(modelPath, LoadModelCallBack);
            }
            else 
            {
                string matPath = SkinTools.GetSkinMaterialPath(Type, mType1, mType2);
                ResourceHelper.AsyncLoadAsset<Material>(matPath, LoadMatCallBack);
            }
        }
        private void LoadModelCallBack(GameObject go)
        {
            mCurHat = go.InstantiateGameObject();
            string matPath = SkinTools.GetSkinMaterialPath(Type, mType1, mType2);
            ResourceHelper.AsyncLoadAsset<Material>(matPath, LoadMatCallBack);
        }
        private void LoadMatCallBack(Material mat)
        {
            mCurHat.GetComponent<MeshRenderer>().material = mat;
            mCurHat.transform.parent = HeadParent;
            mCurHat.transform.Reset();
        }
    }
}
