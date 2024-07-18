using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerBuilder
    {
        private GameObject mPlayer;
        private IDictionary<byte, IPlayerBuild> mPlayerBuildComponentDict;
        public Transform head { get; private set; }
        private SkinnedMeshRenderer mHeadSkinnedMeshRenderer;
        public SkinnedMeshRenderer headSkinnedMeshRenderer { get { return mHeadSkinnedMeshRenderer; } }
        public PlayerBuilder(GameObject target)
        {
            mPlayer = target;
            head = mPlayer.transform.Find("KekosCharacter/Meshes/HEAD");
            mHeadSkinnedMeshRenderer = head.GetComponent<SkinnedMeshRenderer>();
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
            Add(13, new EyeBrownBuild(mPlayer,13, "眉毛", this));
            Add(11, new ColorOfSkinBuild(mPlayer, 11, "肤色", this));
        }
        /// <summary>
        /// 设置发型为没有顶的头发
        /// </summary>
        public void SetHairHeadless()
        {
            GetBuild<HairsBuild>(7).LoadHair(true);
        }
        /// <summary>
        /// 设置发型为没有顶的头发
        /// </summary>
        public void SetHairNormal()
        {
            GetBuild<HairsBuild>(7).LoadHair(false);
        }
        /// <summary>
        /// 选择衣服
        /// </summary>
        public void ShowCloth() 
        {
            GetBuild<TopsBuild>(1).SetRootActive(true);
            GetBuild<PantsBuild>(2).SetRootActive(true);
            GetBuild<BodySuitsBuild>(5).SetRootActive(false);
        }
        /// <summary>
        /// 选择连衣裙
        /// </summary>
        public void ShowBodySuit()
        {
            GetBuild<TopsBuild>(1).SetRootActive(false);
            GetBuild<PantsBuild>(2).SetRootActive(false);
            GetBuild<BodySuitsBuild>(5).SetRootActive(true);
        }
        /// <summary>
        /// 获取构建器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetBuild<T>(byte id) where T : class, IPlayerBuild 
        {
            if (mPlayerBuildComponentDict.ContainsKey(id))
            {
                return mPlayerBuildComponentDict[id] as T;
            }
            return default(T) ;
        }

        /// <summary>
        /// 重构数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
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
            if (mPlayerBuildComponentDict.ContainsKey(8)) 
            {
                return (mPlayerBuildComponentDict[8] as HatsBuild).hasHat;
            }
            return false;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="playerBuild"></param>
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
        /// <summary>
        /// 构建数据
        /// </summary>
        /// <param name="data"></param>
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
        /// <summary>
        /// 设置皮肤材质
        /// </summary>
        /// <param name="material"></param>
        public void SetSkinMat(Material material)
        {
            foreach (var item in mPlayerBuildComponentDict)
            {
                item.Value.SetSkinMat(material);
            }
        }
    }
}
