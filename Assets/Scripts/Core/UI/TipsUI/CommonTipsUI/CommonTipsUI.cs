using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class CommonTipsUI : BaseCustomTipsUI
    {
        private Text mContent;
        private Action mSureAction;
        private Action mCancelAction;
        private Text mTwoSureText;
        private Text mTwoCancelText;
        private Text mOneSureText;
        private GameObject mTwoOperator;
        private GameObject mOneOperator;
        private Text mTitle;
        public CommonTipsUI()
        {

        }

        public override void Awake()
        {
            base.Awake();
            mTitle = transform.FindObject<Text>("Title");
            mTwoOperator = transform.Find("TwoOperator").gameObject;
            mOneOperator = transform.Find("OneOperator").gameObject;
            mContent = transform.FindObject<Text>("TextArea");
            mTwoSureText = transform.FindObject<Text>("TwoSureText");
            mTwoCancelText = transform.FindObject<Text>("TwoCancelText");
            mOneSureText = transform.FindObject<Text>("OneSureText");
            transform.FindObject<Button>("TwoSureBtn").onClick.AddListener(()=> {
                if (mSureAction != null) {
                    mSureAction.Invoke();
                } 
            });
            transform.FindObject<Button>("OneSureBtn").onClick.AddListener(() => {
                if (mSureAction != null)
                {
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

     

        public void ShowContent(string content,string title )
        {
            ShowContent(content, "确认", () => { Hide(); },title);
        }
        public void ShowContent(string content, string sureText, Action sureAction,string title)
        {
            mContent.text = content;
            mSureAction = sureAction;
            mOneSureText.text = sureText;
            mTwoOperator.SetActive(false);
            mOneOperator.SetActive(true);
            mTitle.text = title;
        }
        public void ShowContent(string content, Action sureAction,string title, string cancelText ,  string sureText )
        {
            mContent.text = content;
            mSureAction = sureAction;
            mCancelAction = () => { Hide(); };
            mTwoSureText.text = sureText;
            mTwoCancelText.text = cancelText;
            mTwoOperator.SetActive(true);
            mOneOperator.SetActive(false);
            mTitle.text = title;
        }
    }
}
