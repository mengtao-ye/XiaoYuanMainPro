using UnityEngine;

namespace Game
{
    /// <summary>
    /// 路径数据
    /// </summary>
    public static class PathData
    {
        #region Role
        /// <summary>
        /// 获取角色资源配置文件
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static string GetRoleABPath(string roleName)
        {
            return GetRoleDir(roleName) + "/role_" + roleName;
        }
        /// <summary>
        /// 获取角色资源配置文件
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static string GetRoleConfigPath(string roleName)
        {
            return GetRoleDir(roleName) + "/config.txt";
        }

        /// <summary>
        /// 获取角色资源目录
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static string GetRoleDir(string roleName)
        {
            return ProjectDataDir + "/Roles/" + roleName;
        }
        #endregion
        #region Scene
        /// <summary>
        /// 获取场景资源配置文件
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static string GetSceneABPath(string sceneName)
        {
            return GetSceneDir(sceneName) + "/" + sceneName;
        }

        /// <summary>
        /// 获取场景资源配置文件
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static string GetSceneConfigPath(string sceneName)
        {
            return GetSceneDir(sceneName) + "/config.txt";
        }

        /// <summary>
        /// 获取场景资源目录
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static string GetSceneDir(string sceneName)
        {
            return ProjectDataDir + "/SceneDatas/" + sceneName;
        } 
        #endregion
        /// <summary>
        /// 项目资源根地址
        /// </summary>
        public static string ProjectDataDir {
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
        public static string ProjectDir { get; private set; } = Application.dataPath.Replace("/Assets",string.Empty);
    }
}
