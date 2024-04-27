using UnityEngine;

namespace Game
{
    /// <summary>
    /// 路径数据
    /// </summary>
    public static class PathData
    {
        /// <summary>
        /// 获取场景资源配置文件
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static string GetSceneABPath(string sceneName)
        {
            return GetSceneDir(sceneName) + "/"+sceneName;
        }
        /// <summary>
        /// 获取场景资源配置文件
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static string GetSceneConfigPath(string sceneName) 
        {
            return GetSceneDir(sceneName) +"/config.txt";
        }
        /// <summary>
        /// 获取场景资源目录
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static string GetSceneDir(string sceneName) 
        {
            return ProjectDataPath + "/SceneDatas/"+sceneName;
        }
        /// <summary>
        /// 项目资源根地址
        /// </summary>
        public static string ProjectDataPath {
            get {
#if UNITY_EDITOR
                return ProjectDir + "/XiaoYuanData";
#elif UNITY_ANDROID
         return Application.persistentDataPath + "/XiaoYuanData";
#endif 
            }
        }

        /// <summary>
        ///  项目目录地址
        /// </summary>
        public static string ProjectDir { get; private set; } = Application.dataPath.Replace("/Assets","");
    }
}
