using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 眼睛
    /// </summary>
    public class EyesBuild : BaseChangeHeadPlayerBuild
    {
        private Material mEyeMat;
        private byte[] mPreDatas;

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
            if (mPreDatas != null && ByteTools.IsCompare(data, mPreDatas)) return;
            mPreDatas = data;
            string texturePath = SkinTools.GetSkinTexturePath(Type, data[0], data[1]);
            ResourceHelper.AsyncLoadAsset<Texture2D>(texturePath, LoadTextureCallBack);
        }
        private void LoadTextureCallBack(Texture2D texture)
        {
            mEyeMat.mainTexture = texture;
        }
    }
}
