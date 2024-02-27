using System;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class CommonOneTipsUI : BaseCustomTipsUI
    {
        private Text mContent;
        private Action mSureAction;
        private Text mOneSureText;
        private Text mTitle;
        public CommonOneTipsUI()
        {

        }

        public override void Awake()
        {
            base.Awake();
            mTitle = transform.FindObject<Text>("Title");
            mContent = transform.FindObject<Text>("TextArea");
            mOneSureText = transform.FindObject<Text>("OneSureText");
            transform.FindObject<Button>("OneSureBtn").onClick.AddListener(() => {
                if (mSureAction != null)
                {
                    mSureAction.Invoke();
                }
            });
        }
        public void ShowContent(string content,string title )
        {
            ShowContent(content, "确认", title, Hide);
        }
        public void ShowContent(string content, string sureText,string title, Action sureAction)
        {
            mContent.text = content;
            mSureAction = sureAction;
            mOneSureText.text = sureText;
            mTitle.text = title;
        }
    }
}
