using UnityEngine;
using YFramework;

namespace Game
{
    public class EyesBuild : BaseChangeHeadPlayerBuild
    {
        private Material mEyeMat;
        public EyesBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        protected override void Init()
        {
            base.Init();
            mEyeMat = HeadTarget.GetComponent<SkinnedMeshRenderer>().materials[1];
        }

        public override void Build(byte[] data)
        {
            string texturePath = SkinTools.GetSkinTexturePath(Type, data[0], data[1]);
            ResourceHelper.AsyncLoadAsset<Texture2D>(texturePath, LoadTextureCallBack);
        }
        private void LoadTextureCallBack(Texture2D texture)
        {
            mEyeMat.mainTexture = texture;
        }
    }
}
