using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class PartTimeJobPanel : BaseCustomPanel
    {
        public PartTimeListSubUI PartTimeListSubUI { get; private set; }
        public PartTimeMySubUI PartTimeMySubUI { get; private set; }
        private ISubUI mCur;
        public PartTimeJobPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            Transform typeContent = transform.Find("TypeContent");
            Image listIcon = transform.FindObject<Image>("PartTimeListIcon");
            Text listText = transform.FindObject<Text>("PartTimeListText");
            PartTimeListSubUI = new PartTimeListSubUI(typeContent.Find("PartTimeList"), listIcon, listText);
            Image myIcon = transform.FindObject<Image>("MyIcon");
            Text myText = transform.FindObject<Text>("MyText");
            PartTimeMySubUI = new PartTimeMySubUI(typeContent.Find("My"), myIcon, myText);
            AddSubUI(PartTimeListSubUI);
            AddSubUI(PartTimeMySubUI);
            mCur = PartTimeListSubUI;
            mCur.Show();
            PartTimeMySubUI.Hide();
            Transform type = transform.Find("Type");
            type.FindObject<Button>("PartTimeList").onClick.AddListener(PartTimeListBtnListener);
            type.FindObject<Button>("My").onClick.AddListener(PartTimeMyBtnListener);

            transform.FindObject<Button>("BackBtn").onClick.AddListener(()=> { mUICanvas.CloseTopPanel(); });
          
        }
        private void PartTimeListBtnListener()
        {
            if (mCur.uiName == PartTimeListSubUI.uiName) return;
            mCur.Hide();
            mCur = PartTimeListSubUI;
            mCur.Show();
        }
        private void PartTimeMyBtnListener()
        {
            if (mCur.uiName == PartTimeMySubUI.uiName) return;
            mCur.Hide();
            mCur = PartTimeMySubUI;
            mCur.Show();
        }
    }
}
