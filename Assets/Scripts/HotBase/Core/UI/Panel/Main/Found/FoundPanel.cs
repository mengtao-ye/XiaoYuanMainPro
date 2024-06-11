using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    /// <summary>
    /// 失物招领
    /// </summary>
    public class FoundPanel : BaseCustomPanel
    {
        public FoundPanelListSubUI listSubUI { get; private set; }
        public FoundPanelMySubUI mySubUI { get; private set; }
        private ISubUI mCur;
        public FoundPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { mUICanvas.CloseTopPanel(); });
            Transform content = transform.FindObject<Transform>("TypeContent");
            Image listIcon = transform.FindObject<Image>("ListIcon");
            Text listText = transform.FindObject<Text>("ListText");
            listSubUI = new FoundPanelListSubUI(content.Find("List"), listIcon, listText);
            Image myIcon = transform.FindObject<Image>("MyIcon");
            Text myText = transform.FindObject<Text>("MyText");
            mySubUI = new FoundPanelMySubUI(content.Find("My"), myIcon, myText);
            AddSubUI(listSubUI);
            AddSubUI(mySubUI);
            listSubUI.Hide();
            mySubUI.Hide();
            Transform type = transform.FindObject<Transform>("Type");
            type.FindObject<Button>("List").onClick.AddListener(ListBtnListener);
            type.FindObject<Button>("My").onClick.AddListener(MyBtnListener);

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
