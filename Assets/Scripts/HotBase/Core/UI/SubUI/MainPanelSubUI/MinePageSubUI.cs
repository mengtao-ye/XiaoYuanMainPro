using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class MinePageSubUI : BaseCustomSubUI
    {
        private MainPanel mMainPanel;
        private Image mHead;
        private Text mName;
        private Text mID;
        private Text mEdu;
        private Text mDepartment;
        private Text mClass;
        public MinePageSubUI(Transform trans, MainPanel mainPanel) : base(trans)
        {
            mMainPanel = mainPanel;
        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("SetBtn").onClick.AddListener(SetBtnListener);
            mHead = transform.FindObject<Image>("Head");
            mName = transform.FindObject<Text>("Name");
            mID = transform.FindObject<Text>("ID");
            mEdu = transform.FindObject<Text>("Edu");
            mDepartment = transform.FindObject<Text>("Department");
            mClass = transform.FindObject<Text>("Class");
        }

        public override void Show()
        {
            base.Show();
            
            UserDataModule.MapUserData(AppVarData.Account, UserDataCallBack);
        }
        private void UserDataCallBack(UnityUserData unityUserData) {
            mHead.sprite = unityUserData.headSprite;
            mName.text = unityUserData.userName;
            mID.text = unityUserData.UserID.ToString();
        }


        private void SetBtnListener(){
            GameCenter.Instance.ShowPanel<SettingPanel>();
        }
    }
}
