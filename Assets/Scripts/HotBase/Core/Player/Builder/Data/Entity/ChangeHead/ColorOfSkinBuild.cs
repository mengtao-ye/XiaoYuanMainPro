using UnityEngine;
using YFramework;

namespace Game
{
    public class ColorOfSkinBuild : BaseChangeHeadPlayerBuild
    {
        private Material mSkinMat;
        public ColorOfSkinBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        protected override void Init()
        {
            base.Init();
            mSkinMat = HeadTarget.GetComponent<SkinnedMeshRenderer>().materials[0];
        }

        public override void Build(byte[] data)
        {
            string texturePath = SkinTools.GetSkinTexturePath(Type, data[0], data[1]);
            ResourceHelper.AsyncLoadAsset<Texture2D>(texturePath, LoadTextureCallBack);
        }
        private void LoadTextureCallBack(Texture2D texture)
        {
            mSkinMat.mainTexture = texture;
        }
    }
}
