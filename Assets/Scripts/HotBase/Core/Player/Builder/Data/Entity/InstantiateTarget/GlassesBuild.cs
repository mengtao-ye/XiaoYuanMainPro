using UnityEngine;
using YFramework;

namespace Game
{
    public class GlassesBuild : BaseInstantiateTargetPlayerBuild
    {
        private GameObject mCurGlasses;
        public bool hasHat { get; private set; }
        private byte mType1;
        private byte mType2;

        public GlassesBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        public override void Build(byte[] data)
        {
            if (mCurGlasses != null) {
                GameObject.Destroy(mCurGlasses);
                mCurGlasses = null;
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
            mCurGlasses =  go.InstantiateGameObject();
            string matPath = SkinTools.GetSkinMaterialPath(Type, mType1, mType2);
            ResourceHelper.AsyncLoadAsset<Material>(matPath, LoadMatCallBack);
        }
        private void LoadMatCallBack(Material mat)
        {
            mCurGlasses.GetComponent<MeshRenderer>().materials[0] = mat;
            mCurGlasses.transform.parent = HeadParent;
            mCurGlasses.transform.Reset();
        }
    }
}
