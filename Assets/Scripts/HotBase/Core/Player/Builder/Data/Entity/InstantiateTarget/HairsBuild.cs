using UnityEngine;
using YFramework;

namespace Game
{
    public class HairsBuild : BaseInstantiateTargetPlayerBuild
    {
        private GameObject mCurHair;
        public HairsBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        public override void Build(byte[] data)
        {
            if (mCurHair != null) {
                GameObject.Destroy(mCurHair);
                mCurHair = null;
             }
            if (data[0] == 1)
            {
                return;
            }
            string modelPath = SkinTools.GetSkinHairModelPath(Type, data[0], data[1], mPlayerBuilder.HasHat());
            ResourcesLoadHelper.AsyncLoadAsset<GameObject>(modelPath, LoadModelCallBack);
        }
        private void LoadModelCallBack(GameObject go)
        {
            mCurHair = go.InstantiateGameObject();
            mCurHair.transform.parent = HeadParent;
            mCurHair.transform.Reset();
        }
    }
}
