using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerBuilder
    {
        private GameObject mPlayer;
        private IDictionary<byte, IPlayerBuild> mPlayerBuildComponentDict;
        public PlayerBuilder(GameObject target)
        {
            mPlayer = target;
            mPlayerBuildComponentDict = new Dictionary<byte, IPlayerBuild>();
            Config();
        }

        private void Config()
        {
            Add(1, new TopsBuild(mPlayer, 1, "上衣",this));
            Add(2, new PantsBuild(mPlayer, 2, "裤子", this));
            Add(3, new ShoesBuild(mPlayer, 3, "鞋子", this));
            Add(4, new HandsBuild(mPlayer, 4, "手部", this));
            Add(5, new BodySuitsBuild(mPlayer, 5, "连体衣", this));
            Add(6, new PackagesBuild(mPlayer, 6, "背包", this));
            Add(7, new HairsBuild(mPlayer, 7, "发型", this));
            Add(8, new HatsBuild(mPlayer,8, "帽子", this));
            Add(9, new GlassesBuild(mPlayer,9, "眼镜", this));
            Add(10, new EyesBuild(mPlayer,10, "眼睛", this));
            Add(11, new ColorOfSkinBuild(mPlayer,11, "肤色", this));
            Add(13, new EyeBrownBuild(mPlayer,13, "眉毛", this));
        }

        public void Rebuild(byte type,params byte[] data)
        {
            if (mPlayerBuildComponentDict.ContainsKey(type)) {
                mPlayerBuildComponentDict[type].Build(data);
            }
        }

        /// <summary>
        /// 是否有帽子
        /// </summary>
        /// <returns></returns>
        public bool HasHat() 
        {
            if (mPlayerBuildComponentDict.ContainsKey(8)) {
                return (mPlayerBuildComponentDict[8] as HatsBuild).hasHat;
            }
            return false;
        }

        private void Add(byte type, IPlayerBuild playerBuild)
        {
            if (mPlayerBuildComponentDict.ContainsKey(type))
            {
                Debug.LogError("PlayerBuilder type is exist:" + type);
                return;
            }
            if (playerBuild == null)
            {
                Debug.LogError("PlayerBuilder playerBuild is null");
                return;
            }
            mPlayerBuildComponentDict.Add(type, playerBuild);
        }

        public void Builder(IDictionary<byte, byte[]> data)
        {
            foreach (var item in data)
            {
                if (mPlayerBuildComponentDict.ContainsKey(item.Key))
                {
                    mPlayerBuildComponentDict[item.Key].Build(item.Value);
                }
            }
        }
    }
}
