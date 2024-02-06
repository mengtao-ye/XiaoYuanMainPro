using UnityEngine;
using YFramework;

namespace Game
{
    public static class OssData
    {
        /// <summary>
        /// 源ABfile
        /// </summary>
        public static string OriginalABFileConfigName = "ABFileConfig.bytes";
        /// <summary>
        /// 热更ABfile
        /// </summary>
        public static string HotABFileConfigName = "HotABFileConfig.bytes";
        /// <summary>
        /// 获取AB包资源目录
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string GetOssHotABFilePathDir(string appName, string version)
        {
            if (appName.IsNullOrEmpty() || version.IsNullOrEmpty())
                return null;
            return GetOssPathDir(appName, version) + "/HotFile";
        }
        /// <summary>
        /// 获取AB包资源目录
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string GetOssOriginalABFilePathDir(string appName, string version)
        {
            if (appName.IsNullOrEmpty() || version.IsNullOrEmpty())
                return null;
            return GetOssPathDir(appName, version) + "/OriginalFile";
        }
        /// <summary>
        /// 获取AB包资源目录
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private static string GetOssPathDir(string appName, string version)
        {
            if (appName.IsNullOrEmpty() || version.IsNullOrEmpty())
                return null;
            return "AppData/" + appName + "/" + AppData.RunPlatformName + "/" + version;
        }


        /// <summary>
        /// 获取oss平台根目录
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public static string GetOssPlatformDir(string appName)
        {
            if (appName.IsNullOrEmpty())
                return null;
            return "AppData/" + appName + "/" + AppData.RunPlatformName ;
        }

        /// <summary>
        /// 获取oss上的logo资源
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public static string GetOssLOGOPath(string appName)
        {
            if (appName.IsNullOrEmpty() )
                return null;
            return "AppData/" + appName +"/LOGO.jpg" ;
        }
        /// <summary>
        /// 获取本地资源目录
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string GetFullLocalDir(string appName)
        {
            if (appName.IsNullOrEmpty() )
                return null;
#if UNITY_EDITOR
            return Application.dataPath.Replace("Assets","")+ "AssetBundle/" + appName + "/" + AppData.RunPlatformName ;
#else
            return Application.persistentDataPath + "/AssetBundle/" + appName + "/" + AppData.RunPlatformName ;
#endif
        }

        /// <summary>
        /// 获取本地资源目录
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string GetLocalAppDir(string appName)
        {
            if (appName.IsNullOrEmpty())
                return null;
#if UNITY_EDITOR
            return "Assets/../AssetBundle/" + appName;
#else
            return Application.persistentDataPath + "/AssetBundle/" + appName  ;
#endif
        }

        /// <summary>
        /// 获取本地资源目录
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string GetLocalAppPlatformDir(string appName)
        {
            if (appName.IsNullOrEmpty())
                return null;
            return GetLocalAppDir(appName ) +"/"+ AppData.RunPlatformName;
        }

        /// <summary>
        /// 获取本地资源目录
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string GetLocalDir()
        {
#if UNITY_EDITOR
            return "Assets/../AssetBundle";
#else
            return Application.persistentDataPath + "/AssetBundle" ;
#endif
        }

        /// <summary>
        /// 获取保存到本地的地址
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="version"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetLocalFilePath(string appName, string fileName)
        {
            return GetLocalAppPlatformDir(appName) +"/"+fileName;
        }
    }
}
