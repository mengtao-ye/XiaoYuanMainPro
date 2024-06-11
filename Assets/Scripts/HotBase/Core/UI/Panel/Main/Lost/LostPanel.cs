using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 失物招领/寻物启事
    /// </summary>
    public class LostPanel : BaseCustomPanel
    {
        public LostPanelListSubUI listSubUI { get; private set; }
        public LostPanelMySubUI mySubUI { get; private set; }
        private ISubUI mCur;
        public LostPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { mUICanvas.CloseTopPanel(); });
            Transform content = transform.FindObject<Transform>("TypeContent");
            Image listIcon = transform.FindObject<Image>("ListIcon");
            Text listText = transform.FindObject<Text>("ListText");
            listSubUI = new LostPanelListSubUI(content.Find("List"), listIcon, listText);
            Image myIcon = transform.FindObject<Image>("MyIcon");
            Text myText = transform.FindObject<Text>("MyText");
            mySubUI = new LostPanelMySubUI(content.Find("My"), myIcon, myText);
            AddSubUI(listSubUI);
            AddSubUI(mySubUI);
            listSubUI.Hide();
            mySubUI.Hide();
            Transform type = transform.FindObject<Transform>("Type");
            type.FindObject<Button>("ListBtn").onClick.AddListener(ListBtnListener);
            type.FindObject<Button>("MyBtn").onClick.AddListener(MyBtnListener);

        }

        private void ListBtnListener() 
        {
            if (mCur == listSubUI) 
            {
                return;    
            }
            listSubUI.Show();
            mySubUI.Hide();
            mCur = listSubUI;
        }
        private void MyBtnListener()
        {
            if (mCur == mySubUI)
            {
                return;
            }
            mySubUI.Show();
            listSubUI.Hide();
            mCur = mySubUI;
        }
        public override void FirstShow()
        {
            base.FirstShow();
            listSubUI.Show();
            mCur = listSubUI;
        }

    }
}
