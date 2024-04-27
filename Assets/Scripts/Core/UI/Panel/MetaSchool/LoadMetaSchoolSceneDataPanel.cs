using System.IO;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class LoadMetaSchoolSceneDataPanel : BaseCustomPanel
    {
        private Text mProcessText;
        private Text mLoadText;
        private Image mProcessSlider;
        private int mCount;
        private int mSceneABCount;
        private string mCurSceneVersion;
        public LoadMetaSchoolSceneDataPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mProcessText = transform.FindObject<Text>("ProcessText");
            mLoadText = transform.FindObject<Text>("LoadText");
            mProcessSlider = transform.FindObject<Image>("ProcessSlider");
        }
        public override void Show()
        {
            base.Show();
            mCount = 0;
            mSceneABCount = 0;
            mProcessText.text = "0%";
            mProcessSlider.fillAmount = 0;
            mLoadText.text = "";
            GetSceneData();
        }

        public void SetProcess(float process) 
        {
            mProcessText.text = (int)(process * 100) +"0%";
            mProcessSlider.fillAmount = process;
        }
        private void SetLoadText(string text)
        {
            mLoadText.text = text;
        }

        private void GetSceneData()
        {
            SetLoadText("加载场景配置表");
            SetProcess(0);
            string configURL = OssPathData.GetSceneConfigData(MetaSchoolGlobalVarData.schoolData.assetBundleName);
            HttpTools.GetText(configURL, GetSceneConfigSuccess, GetSceneConfigError);
        }
        private void GetSceneConfigSuccess(string str)
        {
            string localSceneConfigPath = PathData.GetSceneConfigPath(MetaSchoolGlobalVarData.schoolData.assetBundleName);
            mCurSceneVersion = str;
            if (File.Exists(localSceneConfigPath))
            {
                string curVersion = File.ReadAllText(localSceneConfigPath);
                if (curVersion.Equals(str))
                {
                    //当前场景版本与服务器一致    
                    SetLoadText("加载场景");
                    SetProcess(1);
                    string sceneABPath = PathData.GetSceneABPath(MetaSchoolGlobalVarData.schoolData.assetBundleName);
                    AssetBundleTools.LoadScene(sceneABPath, MetaSchoolGlobalVarData.schoolData.assetBundleName, LoadSceneSuccess, LoadSceneError);
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
        private void DownLoadSceneAssetBundleData(string version)
        {
            SetLoadText("下载场景资源");
            string sceneABPath = OssPathData.GetSceneData(MetaSchoolGlobalVarData.schoolData.assetBundleName, version);
            HttpTools.GetBytes(sceneABPath, DownLoadSceneProcess, LoadSceneABSuccess, GetSceneABError);
        }

        private void DownLoadSceneProcess(float process) 
        {
            SetProcess(process);
        }


        private void LoadSceneABSuccess(byte[] assetBundleBytes)
        {
            SetLoadText("加载场景资源");
            SetProcess(1);
            string localSceneConfigPath = PathData.GetSceneConfigPath(MetaSchoolGlobalVarData.schoolData.assetBundleName);
            FileTools.Write(localSceneConfigPath, mCurSceneVersion);
            string sceneABPath = PathData.GetSceneABPath(MetaSchoolGlobalVarData.schoolData.assetBundleName);
            FileTools.Write(sceneABPath, assetBundleBytes);
            AssetBundleTools.LoadScene(sceneABPath, MetaSchoolGlobalVarData.schoolData.assetBundleName, LoadSceneSuccess, LoadSceneError);
        }
        /// <summary>
        /// 场景加载完成
        /// </summary>
        private void LoadSceneSuccess()
        {
            Debug.Log("场景加载成功");
            GameCenter.Instance.ShowPanel<MetaSchoolMainPanel>();
            PlayerChildModel playerChildModel =  new PlayerChildModel(GameCenter.Instance.curModel, UnityTools.CreateGameObject("Player", transform));
            GameCenter.Instance.curModel.AddChildModel(playerChildModel);
            playerChildModel.Awake();
            playerChildModel.Start();
        }
        private void LoadSceneError(string error)
        {
            YFramework.LogHelper.LogError(error);
        }

        private void GetSceneABError(string str)
        {
            if (mSceneABCount > 3)
            {
                LogHelper.LogError("场景资源下载失败：" + MetaSchoolGlobalVarData.schoolData.assetBundleName);
                return;
            }
            mSceneABCount++;
            DownLoadSceneAssetBundleData(mCurSceneVersion);
        }
        private void GetSceneConfigError(string str)
        {
            if (mCount > 3)
            {
                LogHelper.LogError("场景配置表资源下载失败：" + MetaSchoolGlobalVarData.schoolData.assetBundleName);
                return;
            }
            mCount++;
            GetSceneData();
        }
    }
}
