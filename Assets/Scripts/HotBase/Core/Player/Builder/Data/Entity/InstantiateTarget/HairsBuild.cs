using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 发型
    /// </summary>
    public class HairsBuild : BaseInstantiateTargetPlayerBuild
    {
        private GameObject mCurHair;
        private byte mType1;
        private byte mType2;
        private byte[] mPreDatas;
        private bool mIsHeadless;
        public HairsBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        public override void Build(byte[] data)
        {
            if (mPreDatas != null && ByteTools.IsCompare(data, mPreDatas)) return;
            mPreDatas = data;
            if (mType1 != data[0] && mCurHair != null)
            {
                GameObject.DestroyImmediate(mCurHair);
                mCurHair = null;
            }
            mType1 = data[0];
            mType2 = data[1];
            if (data[0] == 1)
            {
                return;
            }
            if (mCurHair == null)
            {
                string modelPath = SkinTools.GetSkinHairModelPath(Type, data[0], data[1], mPlayerBuilder.HasHat());
                ResourcesLoadHelper.AsyncLoadAsset<GameObject>(modelPath, LoadModelCallBack);
            }
            else
            {
                mCurHair.GetComponent<MeshRenderer>().material.color = CommonSkinColorMapper.Instance.Get(mType2);
            }
        }

        public void LoadHair(bool hasHot)
        {
            if (mIsHeadless == hasHot) return;
            mIsHeadless = hasHot;
            if (mCurHair != null)
            {
                GameObject.DestroyImmediate(mCurHair);
                mCurHair = null;
            }
            if (mType1 == 1) return;
            if (mCurHair == null)
            {
                string modelPath = SkinTools.GetSkinHairModelPath(Type, mType1, mType2, hasHot);
                ResourcesLoadHelper.AsyncLoadAsset<GameObject>(modelPath, LoadModelCallBack);
            }
        }

        private void LoadModelCallBack(GameObject go)
        {
            mCurHair = go.InstantiateGameObject();
            mCurHair.GetComponent<MeshRenderer>().material.color = CommonSkinColorMapper.Instance.Get(mType2);
            mCurHair.transform.parent = HeadParent;
            mCurHair.transform.Reset();
        }
    }
}
