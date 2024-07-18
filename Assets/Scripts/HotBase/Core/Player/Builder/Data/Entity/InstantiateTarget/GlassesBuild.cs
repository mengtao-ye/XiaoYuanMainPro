using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 眼镜
    /// </summary>
    public class GlassesBuild : BaseInstantiateTargetPlayerBuild
    {
        private GameObject mCurGlasses;
        private byte mType1;
        private byte mType2;
        private byte[] mPreDatas;

        public GlassesBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        public override void Build(byte[] data)
        {
            if (mPreDatas != null && ByteTools.IsCompare(data, mPreDatas)) return;
            mPreDatas = data;
            if (mType1 != data[0] && mCurGlasses != null)
            {
                GameObject.Destroy(mCurGlasses);
                mCurGlasses = null;
            }
            if (data[0] == 0)
            {
                return;
            }
            mType1 = data[0];
            mType2 = data[1];
            if (mCurGlasses == null)
            {
                string modelPath = SkinTools.GetSkinModelPath(Type, mType1, mType2);
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
            mCurGlasses = go.InstantiateGameObject();
            string matPath = SkinTools.GetSkinMaterialPath(Type, mType1, mType2);
            ResourceHelper.AsyncLoadAsset<Material>(matPath, LoadMatCallBack);
        }
        private void LoadMatCallBack(Material mat)
        {
            mCurGlasses.GetComponent<MeshRenderer>().materials[0] .CopyPropertiesFromMaterial(mat);
            mCurGlasses.transform.parent = HeadParent;
            mCurGlasses.transform.Reset();
        }
    }
}
