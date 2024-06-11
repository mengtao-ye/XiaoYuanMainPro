using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class LoadRoleData 
    {
        public string roleABName;
        public Action<float> loadProcess;
        public Action<string> loadError;
        public Action<GameObject> success;
        public int mCount;
        public int mRoleABCount;
        public string mCurRoleVersion;
    }
    public static class LoadABRoleTools
    {
        public static bool isLoading;
        private static LoadRoleData mCur;
        private static Coroutine mCurCor;
        private static Queue<LoadRoleData> mQueue;
        public static void LoadABRole(string roleABName ,Action<float> loadProcess, Action<string> loadError, Action<GameObject> success)
        {
            if (mQueue == null) mQueue = new Queue<LoadRoleData>();
            if (mCurCor == null) IEnumeratorModule.StartCoroutine(IELoadRole());
            if (roleABName.IsNullOrEmpty())
            {
                loadError?.Invoke("加载的角色名称为空");
                return;
            }
            LoadRoleData loadRoleData = new LoadRoleData();
            loadRoleData.roleABName = roleABName;
            loadRoleData.loadProcess = loadProcess;
            loadRoleData.loadError = loadError;
            loadRoleData.success = success;
            loadRoleData.mCount = 0;
            loadRoleData.mRoleABCount = 0;
            loadRoleData.mCurRoleVersion = null;
            mQueue.Enqueue(loadRoleData);
        }

        private static IEnumerator IELoadRole() 
        {
            while (true)
            {
                if (!isLoading && mQueue.Count >0 ) 
                {
                    mCur =  mQueue.Dequeue();
                    GetRoleData();
                    isLoading = true;
                }
                yield return Yielders.WaitForEndOfFrame;
                if (!isLoading && mQueue.Count == 0 && mCurCor!=null)
                {
                    IEnumeratorModule.StopCoroutine(mCurCor);
                    mCurCor = null;
                }
            }    
        }

        private static void GetRoleData()
        {
            mCur. loadProcess?.Invoke(0);
            string configURL = OssPathData.GetRoleConfigData(mCur.roleABName);
            HttpTools.GetText(configURL, GetRoleConfigSuccess, GetRoleConfigError);
        }
        private static void GetRoleConfigSuccess(string str)
        {
            string localRoleConfigPath = PathData.GetRoleConfigPath(mCur.roleABName);
            mCur.mCurRoleVersion = str;
            if (File.Exists(localRoleConfigPath))
            {
                string curVersion = File.ReadAllText(localRoleConfigPath);
                if (curVersion.Equals(str))
                {
                    //当前场景版本与服务器一致    
                    mCur.loadProcess?.Invoke(1);
                    string roleABPath = PathData.GetRoleABPath(mCur.roleABName);
                    AssetBundleTools.LoadRole(roleABPath, mCur.roleABName, (obj)=> {
                        isLoading = false;
                        mCur.success?.Invoke (obj);
                    } , (str)=> {
                        isLoading = false;
                        mCur.loadError?.Invoke(str);
                    });
                }
                else
                {
                    DownLoadRoleAssetBundleData(str);
                }
            }
            else
            {
                DownLoadRoleAssetBundleData(str);
            }
        }
        private static void DownLoadRoleAssetBundleData(string version)
        {
            string roleABPath = OssPathData.GetRoleData(mCur.roleABName, version);
            HttpTools.GetBytes(roleABPath, mCur.loadProcess, LoadRoleABSuccess, GetRoleABError);
        }
        private static void LoadRoleABSuccess(byte[] assetBundleBytes)
        {
            mCur.loadProcess?.Invoke(1);
            string localRoleConfigPath = PathData.GetRoleConfigPath(mCur.roleABName);
            FileTools.Write(localRoleConfigPath, mCur.mCurRoleVersion);
            string roleABPath = PathData.GetRoleABPath(mCur.roleABName);
            FileTools.Write(roleABPath, assetBundleBytes);
            AssetBundleTools.LoadRole(roleABPath, mCur.roleABName, (obj) => {
                isLoading = false;
                mCur.success?.Invoke(obj);
            }, (str) => {
                isLoading = false;
                mCur.loadError?.Invoke(str);
            });
        }
        private static void GetRoleABError(string str)
        {
            if (mCur.mRoleABCount > 3)
            {
                isLoading = false;
                LogHelper.LogError("角色资源下载失败：" + mCur.roleABName);
                return;
            }
            mCur.mRoleABCount++;
            DownLoadRoleAssetBundleData(mCur.mCurRoleVersion);
        }
        private static void GetRoleConfigError(string str)
        {
            if (mCur.mCount > 3)
            {
                isLoading = false;
                LogHelper.LogError("角色配置表资源下载失败：" + mCur.roleABName);
                return;
            }
            mCur.mCount++;
            GetRoleData();
        } 
    }
}
