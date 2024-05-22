using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class MyReleasePartTimeJobDetailPanel : BaseCustomPanel
    {
        private Text mTitle;
        private Text mPrice;
        private Image mHead;
        private Text mName;
        private Text mTime;
        private Text mPosition;
        private Text mDetail;
        private RectTransform mDetailArea;
        private int id;
        public MyReleasePartTimeJobDetailPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            NormalVerticalScrollView normalScrollView = transform.FindObject("ScrollView").AddComponent<NormalVerticalScrollView>();
            normalScrollView.Init();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<BusinessPartTimeJobPanel>(); });
            mTitle = transform.FindObject<Text>("PartTimeJobTitle");
            mPrice = transform.FindObject<Text>("Price");
            mHead = transform.FindObject<Image>("Head");
            mName = transform.FindObject<Text>("Name");
            mTime = transform.FindObject<Text>("Time");
            mPosition = transform.FindObject<Text>("Position");
            mDetail = transform.FindObject<Text>("Detail");
            mDetailArea = transform.FindObject<RectTransform>("DetailArea");
            transform.FindObject<Button>("ApplicationListBtn").onClick.AddListener(ApplicationBtnListener) ;
        }

        private void ApplicationBtnListener() 
        {
            GameCenter.Instance.ShowPanel<PartTimeJobApplicationListPanel>((ui)=> 
            {
                ui.SetPartTimeJobID(id);
            });
        }

        public void SetData(string title,int price,byte priceType,string time,string pos,string detail,int id) {
            this.id = id;
            mTitle.text = title;
            mPrice.text = price + "/" + PartTimeJobTools.GetPriceType(priceType);
            UserDataModule.MapUserData(AppVarData.Account,mHead,mName);
            mTime.text = "工作时间：\n" + time;
            mPosition.text = "工作地点：\n" + pos;
            mDetail.text = "职位详情：\n" +detail;
            mDetailArea.sizeDelta = new Vector2(mDetailArea.sizeDelta.x , mDetail.preferredHeight + 40);
        }
    }
}
