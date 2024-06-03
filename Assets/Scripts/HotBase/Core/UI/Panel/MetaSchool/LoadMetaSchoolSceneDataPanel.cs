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
            mProcessText.text = "0%";
            mProcessSlider.fillAmount = 0;
            mLoadText.text = string.Empty;
            LoadABSceneTools.LoadABScene(MetaSchoolGlobalVarData.schoolData.assetBundleName, SetLoadText, SetProcess, LoadError, LoadSceneSuccess);
        }
        private void LoadError(string error)
        {
            LogHelper.LogError(error);    
        }

        public void SetProcess(float process) 
        {
            mProcessText.text = (int)(process * 100) +"%";
            mProcessSlider.fillAmount = process;
        }
        private void SetLoadText(string text)
        {
            mLoadText.text = text;
        }

        /// <summary>
        /// 场景加载完成
        /// </summary>
        private static void LoadSceneSuccess()
        {
            Debug.Log("场景加载成功");
            GameCenter.Instance.ShowPanel<MetaSchoolMainPanel>();
            PlayerChildModel playerChildModel = new PlayerChildModel(GameCenter.Instance.curModel, UnityTools.CreateGameObject("Player", GameCenter.Instance.curModel.transform));
            GameCenter.Instance.curModel.AddChildModel(playerChildModel);
            playerChildModel.Awake();
            playerChildModel.Start();
        }
    }
}
