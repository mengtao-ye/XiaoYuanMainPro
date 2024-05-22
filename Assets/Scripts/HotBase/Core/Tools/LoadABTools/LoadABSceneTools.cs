using System;
using System.IO;
using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public static class LoadABSceneTools
    {
        public static bool isLoadingScene;
        private static int mCount;
        private static int mSceneABCount;
        private static string mCurSceneVersion;
        private static string mSceneABName;
        private static Action<string> mLoadProcessName;
        private static Action<float> mLoadProcess;
        private static Action<string> mLoadError;
        private static Action mLoadSuccess;
        public static void LoadABScene(string sceneABName, Action<string> loadProcessName, Action<float> loadProcess, Action<string> loadError, Action success)
        {
            if (sceneABName.IsNullOrEmpty())
            {
                loadError?.Invoke("加载的场景名称为空");
                return;
            }
            if (isLoadingScene)
            {
                loadError?.Invoke("场景:" + mSceneABName + "正在加载");
                return;
            }
            mLoadSuccess = success;
            mLoadProcessName = loadProcessName;
            mLoadProcess = loadProcess;
            mLoadError = loadError;
            isLoadingScene = true;
            mCount = 0;
            mSceneABCount = 0;
            mCurSceneVersion = null;
            mSceneABName = sceneABName;
            GetSceneData();
        }
        private static void GetSceneData()
        {
            mLoadProcessName?.Invoke("加载场景配置表");
            mLoadProcess?.Invoke(0);
            string configURL = OssPathData.GetSceneConfigData(mSceneABName);
            HttpTools.GetText(configURL, GetSceneConfigSuccess, GetSceneConfigError);
        }
        private static void GetSceneConfigSuccess(string str)
        {
            string localSceneConfigPath = PathData.GetSceneConfigPath(mSceneABName);
            mCurSceneVersion = str;
            if (File.Exists(localSceneConfigPath))
            {
                string curVersion = File.ReadAllText(localSceneConfigPath);
                if (curVersion.Equals(str))
                {
                    //当前场景版本与服务器一致    
                    mLoadProcessName?.Invoke("加载场景");
                    mLoadProcess?.Invoke(1);
                    string sceneABPath = PathData.GetSceneABPath(mSceneABName);
                    AssetBundleTools.LoadScene(sceneABPath, mSceneABName,()=> { 
                        isLoadingScene = false; 
                        mLoadSuccess?.Invoke();  
                    } , (error)=> {
                        isLoadingScene = false;
                        mLoadError?.Invoke(error);
                    });
                }
                else
                {
                    DownLoadSceneAssetBundleData(str);
                }
            }
            else
            {
                DownLoadSceneAssetBundleData(str);
            }
        }
        private static void DownLoadSceneAssetBundleData(string version)
        {
            mLoadProcessName?.Invoke("下载场景资源");
            string sceneABPath = OssPathData.GetSceneData(mSceneABName, version);
            HttpTools.GetBytes(sceneABPath, mLoadProcess, LoadSceneABSuccess, GetSceneABError);
        }
        private static void LoadSceneABSuccess(byte[] assetBundleBytes)
        {
            mLoadProcessName?.Invoke("加载场景资源");
            mLoadProcess?.Invoke(1);
            string localSceneConfigPath = PathData.GetSceneConfigPath(mSceneABName);
            FileTools.Write(localSceneConfigPath, mCurSceneVersion);
            string sceneABPath = PathData.GetSceneABPath(mSceneABName);
            FileTools.Write(sceneABPath, assetBundleBytes);
            AssetBundleTools.LoadScene(sceneABPath, mSceneABName, () => {
                isLoadingScene = false;
                mLoadSuccess?.Invoke();
            }, (error) => {
                isLoadingScene = false;
                mLoadError?.Invoke(error);
            });
        }
        private static void GetSceneABError(string str)
        {
            if (mSceneABCount > 3)
            {
                isLoadingScene = false;
                LogHelper.LogError("场景资源下载失败：" + mSceneABName);
                return;
            }
            mSceneABCount++;
            DownLoadSceneAssetBundleData(mCurSceneVersion);
        }
        private static void GetSceneConfigError(string str)
        {
            if (mCount > 3)
            {
                isLoadingScene = false;
                LogHelper.LogError("场景配置表资源下载失败：" + mSceneABName);
                return;
            }
            mCount++;
            GetSceneData();
        } 
    }
}
