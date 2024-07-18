using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 肤色
    /// </summary>
    public class ColorOfSkinBuild : BaseChangeHeadPlayerBuild
    {
        private Material mSkinMat;
        private byte[] mPreDatas;

        public ColorOfSkinBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {

        }

        protected override void Init()
        {
            base.Init();
            ResourceHelper.AsyncLoadAsset<Material>(SkinTools.GetSkinMatPath(), LoadSkinMatCallback);
        }


        private void LoadSkinMatCallback(Material mat)
        {
            mSkinMat = mat;
            HeadTarget.GetComponent<SkinnedMeshRenderer>().material = mat;
            mPlayerBuilder.SetSkinMat(mSkinMat);
        }

        public override void Build(byte[] data)
        {
            if (mPreDatas != null && ByteTools.IsCompare(data, mPreDatas)) return;
            mPreDatas = data;
            string texturePath = SkinTools.GetSkinTexturePath(Type, data[0], data[1]);
            ResourceHelper.AsyncLoadAsset<Texture2D>(texturePath, LoadTextureCallBack);
        }
        private void LoadTextureCallBack(Texture2D texture)
        {
            mSkinMat.mainTexture = texture;
        }
    }
}
