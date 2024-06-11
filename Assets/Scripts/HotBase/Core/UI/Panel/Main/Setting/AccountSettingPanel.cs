using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class AccountSettingPanel : BaseCustomPanel
    {
        private Image mHead;
        private Text mNameText;
        private Text mSexText;
        private Text mBirthdayText;
        private byte mSex;
        private int mBirthday;
        public AccountSettingPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { mUICanvas.CloseTopPanel(); });
            mHead = transform.FindObject<Image>("HeadImg");
            mNameText = transform.FindObject<Text>("NameText");
            mSexText = transform.FindObject<Text>("SexText");
            mBirthdayText = transform.FindObject<Text>("BirthdayText");
            transform.FindObject<Button>("NameBtn").onClick.AddListener(NameBtnListener);
            transform.FindObject<Button>("SexBtn").onClick.AddListener(SexBtnListener);
            transform.FindObject<Button>("BirthdayBtn").onClick.AddListener(BirthdayBtnListener);



          

        }

        public override void Show()
        {
            base.Show();
            UpdateAccount();
        }


        private void BirthdayBtnListener()
        {
            GameCenter.Instance.ShowTipsUI<SelectBirthdayTipUI>((ui) =>
            {
                ui.ShowContent(mBirthday);
            });
        }
        private void SexBtnListener()
        {
            GameCenter.Instance.ShowTipsUI<SelectSexTipUI>((ui) =>
            {
                ui.ShowContent(mSex);
            });
        }


        public void UpdateAccount()
        {
            UserDataModule.MapUserData(AppVarData.Account, UserDataMapCallBack);
        }


        private void UserDataMapCallBack(UnityUserData data)
        {
            mHead.sprite = data.headSprite;
            mNameText.text = data.userName;
            mSex = data.sex;
            mBirthday = data.birthday;
            mSexText.text = UserDataTools.GetSex(data.sex);
            mBirthdayText.text = UserDataTools.ValueToBirthdayStr(mBirthday);
        }

        private void NameBtnListener()
        {
            GameCenter.Instance.ShowTipsUI<CommonInputFieldTipUI>((ui) =>
            {
                ui.ShowContent("修改名称", "确认", SureNameBtnCallBack, 20, mNameText.text);
            });
        }

        private void SureNameBtnCallBack(string content)
        {
            if (content.IsNullOrEmpty())
            {
                AppTools.ToastNotify("名称不能为空");
                return;
            }
            byte[] sendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), content.ToBytes());
            GameCenter.Instance.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.ModifyName, sendBytes);
        }

    }
}
