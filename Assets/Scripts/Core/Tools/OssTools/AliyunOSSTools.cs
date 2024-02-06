using Aliyun.OSS;
using Aliyun.OSS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class AliyunOSSTools : Singleton<AliyunOSSTools>
    {
        private OssClient mOssClient;
        public AliyunOSSTools()
        {
            mOssClient = new OssClient(OssConfig.EndPoint, OssConfig.AccessKeyId, OssConfig.AccessKeySecret);
        }
        /// <summary>
        /// ����App����
        /// </summary>
        /// <param name="appName">app����</param>
        /// <param name="version">�汾</param>
        /// <param name="cnName">app��������</param>
        public void LoadAppData(string appName, string version, string cnName,Action<float> loadProcessCallback,Action<string> loadSuccess,Action<string> loadFail,Action allLoadFinish)
        {
            if (appName.IsNullOrEmpty() || version.IsNullOrEmpty())
            {
                Debug.LogError("LoadAppData appName or version is null");
                return;
            }

            //IProcess process = GameCenter.Instance.processController.Create()
            //    .Concat(new LoadAppFileConfigProcess(appName, version,loadProcessCallback,loadSuccess,loadFail))
            //    .Concat(new LoadAppDataProcess(appName, version, loadProcessCallback, loadSuccess, loadFail))
            //    .Concat(new SaveAppDataToLocalProcess(mOssClient, appName, cnName, loadSuccess, loadFail))
            //    .Concat(new CallBackProcess(allLoadFinish))
            //    ;
            //process.processManager.Launcher();
        }
        /// <summary>
        /// ����App����
        /// </summary>
        /// <param name="appName">app����</param>
        /// <param name="version">�汾</param>
        /// <param name="cnName">app��������</param>
        public void UpdateAppHotData(string appName, string version)
        {
            if (appName.IsNullOrEmpty() || version.IsNullOrEmpty())
            {
                Debug.LogError("UpdateAppHotData appName or version is null");
                return;
            }
            //IProcess process = GameCenter.Instance.processController.Create()
            //    .Concat(new LoadHotAppFileConfigProcess(mOssClient, appName, version))
            //    .Concat(new LoadHotAppDataProcess(mOssClient, appName, version))
            //    ;
            //process.processManager.Launcher();
        }
        /// <summary>
        /// ����App����
        /// </summary>
        /// <param name="appName">app����</param>
        /// <param name="version">�汾</param>
        /// <param name="cnName">app��������</param>
        public void UpdateAppData(string appName, string version, Action<float> loadProcessCallback, Action<string> loadSuccess, Action<string> loadFail, Action allLoadFinish)
        {
            if (appName.IsNullOrEmpty() || version.IsNullOrEmpty())
            {
                Debug.LogError("UpdateAppData appName or version is null");
                return;
            }
            string dir = OssData.GetLocalAppPlatformDir(appName);
            if (Directory.Exists(dir))
            {
                DirectoryTools.ClearDir(dir);
            }
            //IProcess process = GameCenter.Instance.processController.Create()
            //    .Concat(new LoadAppFileConfigProcess(appName, version,loadProcessCallback,loadSuccess,loadFail))
            //    .Concat(new LoadAppDataProcess(appName, version,loadProcessCallback,loadSuccess,loadFail))
            //    .Concat(new CallBackProcess(allLoadFinish))
            //    ;
            //process.processManager.Launcher();
        }
        /// <summary>
        /// �����ļ����ñ�
        /// </summary>
        /// <param name="resPath"></param>
        /// <returns></returns>
        public AssetBundleFileData LoadLocalFileConfig(string resPath)
        {
            if (!File.Exists(resPath))
            {
                Debug.LogError("δ�ҵ�ABFile���ñ�");
                return null;
            }
            byte[] data = File.ReadAllBytes(resPath);
            EncryptionTools.Decryption(data);
            AssetBundleFileData mAssetBundleFileData = ConverterDataTools.ToObject<AssetBundleFileData>(data);
            if (mAssetBundleFileData == null)
            {
                Debug.LogError("ABFile���ñ����ݴ���");
                return null;
            }
            return mAssetBundleFileData;
        }

        /// <summary>
        /// ����oss�ļ�(����)
        /// </summary>
        /// <param name="ossPath"></param>
        /// <param name="savePath"></param>
        /// <param name="loadProcess"></param>
        /// <param name="loadSuccss"></param>
        /// <param name="loadFail"></param>
        public void LoadOssFile(string ossPath, string savePath, Action<float> loadProcess = null, Action<long> loadSuccss = null, Action<string> loadFail = null)
        {
            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest(OssConfig.Bucket, ossPath);
                getObjectRequest.StreamTransferProgress = (sender, args) =>
                {
                    float process = (args.TransferredBytes * 100 / args.TotalBytes) / 100.0f;
                    loadProcess?.Invoke(process);
                };
                FileTools.CreateFile(savePath);
                OssObject result = mOssClient.GetObject(getObjectRequest);
                using (var resultStream = result.Content)
                {
                    byte[] buf = new byte[1024];
                    var fs = File.Open(savePath, FileMode.OpenOrCreate);
                    var len = 0;
                    bool isFirst = false;
                    long size = 0;
                    while ((len = resultStream.Read(buf, 0, 1024)) != 0)
                    {
                        if (!isFirst)
                        {
                            EncryptionTools.Encryption(buf);
                            isFirst = true;
                        }
                        size += len;
                        fs.Write(buf, 0, len);
                    }
                    fs.Close();
                    loadSuccss?.Invoke(size);
                }
            }
            catch (OssException e)
            {
                Debug.LogError("���������ļ�����" + e.Message);
                loadFail?.Invoke(e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError("���������ļ�����" + e.Message);
                loadFail?.Invoke(e.Message);
            }
        }
        /// <summary>
        /// ����oss�ļ���δ���ܣ�
        /// </summary>
        /// <param name="ossPath"></param>
        /// <param name="savePath"></param>
        /// <param name="loadProcess"></param>
        /// <param name="loadSuccss"></param>
        /// <param name="loadFail"></param>
        public void LoadOssBytes(string ossPath, Action<float> loadProcess = null, Action<byte[]> loadSuccss = null, Action<string> loadFail = null)
        {
            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest(OssConfig.Bucket, ossPath);
                getObjectRequest.StreamTransferProgress = (sender, args) =>
                {
                    float process = (args.TransferredBytes * 100 / args.TotalBytes) / 100.0f;
                    loadProcess?.Invoke(process);
                };
                OssObject result = mOssClient.GetObject(getObjectRequest);
                using (var resultStream = result.Content)
                {
                    byte[] loadBytes = new byte[result.Content.Length];
                    byte[] buf = new byte[1024];
                    var len = 0;
                    long size = 0;
                    while ((len = resultStream.Read(buf, 0, 1024)) != 0)
                    {
                        for (int i = 0; i < len; i++)
                        {
                            loadBytes[size + i] = buf[i];
                        }
                        size += len;
                    }
                    loadSuccss?.Invoke(loadBytes);
                }
            }
            catch (OssException e)
            {
                Debug.LogError("���������ļ�����" + e.Message);
                loadFail?.Invoke(e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError("���������ļ�����" + e.Message);
                loadFail?.Invoke(e.Message);
            }
        }
        /// <summary>
        /// ��ȡ��Ӧ Bucket ���ļ��б�
        /// </summary>
        /// <returns></returns>
        private List<string> GetFileList()
        {
            ObjectListing listing = mOssClient.ListObjects(OssConfig.Bucket);
            List<string> nameList = new List<string>();
            foreach (var item in listing.ObjectSummaries)
            {
                // ���˵��ļ���
                if (Regex.IsMatch(item.Key, "/") == false)
                {
                    nameList.Add(item.Key);

                }
            }
            return nameList;
        }
    }
}
