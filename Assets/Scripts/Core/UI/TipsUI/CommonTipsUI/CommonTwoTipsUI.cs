using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class CommonTwoTipsUI : BaseCustomTipsUI
    {
        private Text mContent;
        private Action mSureAction;
        private Action mCancelAction;
        private Text mTwoSureText;
        private Text mTwoCancelText;
        private Text mTitle;
        public CommonTwoTipsUI()
        {

        }

        public override void Awake()
        {
            base.Awake();
            mTitle = transform.FindObject<Text>("Title");
            mContent = transform.FindObject<Text>("TextArea");
            mTwoSureText = transform.FindObject<Text>("TwoSureText");
            mTwoCancelText = transform.FindObject<Text>("TwoCancelText");
            transform.FindObject<Button>("TwoSureBtn").onClick.AddListener(()=> {
                if (mSureAction != null) {
                    mSureAction.Invoke();
                } 
            });
            transform.FindObject<Button>("TwoCancelBtn").onClick.AddListener(() => {
                if (mCancelAction != null)
                {
                    mCancelAction.Invoke();
                }
            });
        }
        public void ShowContent(string content,string title, string cancelText , Action cancelAction,string sureText, Action sureAction)
        {
            mContent.text = content;
            mSureAction = sureAction;
            mCancelAction = () => 
            {
                Hide(); 
                cancelAction?.Invoke(); 
            };
            mTwoSureText.text = sureText;
            mTwoCancelText.text = cancelText;
            mTitle.text = title;
        }
    }
}
