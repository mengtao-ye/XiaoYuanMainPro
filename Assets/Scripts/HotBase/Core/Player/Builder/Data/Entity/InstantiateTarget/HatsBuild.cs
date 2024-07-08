using UnityEngine;
using YFramework;

namespace Game
{
    public class HatsBuild : BaseInstantiateTargetPlayerBuild
    {
        private GameObject mCurHat;
        public bool hasHat { get; private set; }
        private byte mType1;
        private byte mType2;

        public HatsBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        public override void Build(byte[] data)
        {
            if (mCurHat != null)
            {
                GameObject.Destroy(mCurHat);
                mCurHat = null;
            }
            if (data[0] == 0)
            {
                hasHat = false;
                return;
            }
            hasHat = true;
            mType1 = data[0];
            mType2 = data[1];
            string modelPath = SkinTools.GetSkinModelPath(Type, mType1, mType2);
            ResourcesLoadHelper.AsyncLoadAsset<GameObject>(modelPath, LoadModelCallBack);
        }
        private void LoadModelCallBack(GameObject go)
        {
            mCurHat =  go.InstantiateGameObject();
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
