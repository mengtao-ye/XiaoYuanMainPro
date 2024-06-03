using System;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class CommonInputFieldTipUI : BaseCustomTipsUI
    {
        private Text mTitle;
        private InputField mIF;
        private Action<string> mSureCallBack;
        private Text mSureText;
        public CommonInputFieldTipUI()
        {
        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("CloseBtn").onClick.AddListener(Hide);
            mTitle = transform.FindObject<Text>("Title");
            mIF = transform.FindObject<InputField>("IF");
            transform.FindObject<Button>("SureBtn").onClick.AddListener(SureBtnListener);
            mSureText = transform.FindObject<Text>("SureText");
        }

        public override void Show()
        {
            base.Show();
            SetBGClickCallBack(Hide);
            mIF.Select();
            mIF.ActivateInputField();
        }

        public override void Hide()
        {
            base.Hide();
            mSureCallBack = null;
        }
        private void SureBtnListener()
        {
            mSureCallBack?.Invoke(mIF.text);
            Hide();
        }
        public void ShowContent(string title,string sureText,Action<string> sureCallBack)
        {
            mTitle.text = title;
            mSureCallBack = sureCallBack;
            mSureText.text = sureText;
        }
    }
}
