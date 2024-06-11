using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class SettingPanel : BaseCustomPanel
    {
        private VerticalLayoutGroup mVerticalLayoutGroup;
        public SettingPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mVerticalLayoutGroup = transform.FindObject<VerticalLayoutGroup>("Content");
            NormalVerticalScrollView normalVerticalScrollView = transform.FindObject("ScrollView").AddComponent<NormalVerticalScrollView>();
            normalVerticalScrollView.Init();
            float len = UITools.GetVerticalSize(mVerticalLayoutGroup);
            normalVerticalScrollView.SetSize(len);
            transform.FindObject<Button>("BackBtn").onClick.AddListener(()=> { mUICanvas.CloseTopPanel(); });
            transform.FindObject<Button>("AccountSetting").onClick.AddListener(AccountSettingBtnListener);
            transform.FindObject<Button>("SchoolSetting").onClick.AddListener(SchoolSettingBtnListener);
            
            transform.FindObject<Button>("ExitLogin").onClick.AddListener(ExitLoginBtnListener);
            transform.FindObject<Button>("ExitGame").onClick.AddListener(ExitGameBtnListener);
        }
        private void AccountSettingBtnListener()
        {
            GameCenter.Instance.ShowPanel<AccountSettingPanel>();
        }
        private void SchoolSettingBtnListener()
        {
            GameCenter.Instance.ShowPanel<SchoolSettingPanel>();
        }

        private void ExitGameBtnListener()
        {
            GameCenter.Instance.ShowTipsUI<CommonTwoTipsUI>((ui) =>
            {
                ui.ShowContent("是否退出应用？", "退出应用", "取消", null, "确认", SureExitGame);
            });
        }
        private void SureExitGame()
        {
            AppTools.QuitApp();
        }
        private void ExitLoginBtnListener()
        {
            GameCenter.Instance.ShowTipsUI<CommonTwoTipsUI>((ui) =>
            {
                ui.ShowContent("是否退出登录？", "退出登录", "取消", null, "确认", SureExitLogin);
            });
        }
        private void SureExitLogin()
        {
            GameCenter.Instance.HideTipsUI<CommonTwoTipsUI>();
            GameCenter.Instance.LoadScene(SceneID.LoginScene, ABTagEnum.Main);
        }
    }
}
