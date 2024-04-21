using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class PartTimeJobDetailPanel : BaseCustomPanel
    {
        private Text mTitle;
        private Text mPrice;
        private Image mHead;
        private Text mName;
        private Text mTime;
        private Text mPosition;
        private Text mDetail;
        private RectTransform mDetailArea;
        private int PartTimeJobID;
        public PartTimeJobDetailPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            NormalVerticalScrollView normalScrollView = transform.FindObject("ScrollView").AddComponent<NormalVerticalScrollView>();
            normalScrollView.Init();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<PartTimeJobPanel>(); });
            transform.FindObject<Button>("CollectBtn").onClick.AddListener(CollectBtnListener);
            transform.FindObject<Button>("ApplicationBtn").onClick.AddListener(ApplicationBtnListener);
            mTitle = transform.FindObject<Text>("PartTimeJobTitle");
            mPrice = transform.FindObject<Text>("Price");
            mHead = transform.FindObject<Image>("Head");
            mName = transform.FindObject<Text>("Name");
            mTime = transform.FindObject<Text>("Time");
            mPosition = transform.FindObject<Text>("Position");
            mDetail = transform.FindObject<Text>("Detail");
            mDetailArea = transform.FindObject<RectTransform>("DetailArea");
        }
        /// <summary>
        /// 报名按钮
        /// </summary>
        private void ApplicationBtnListener()
        {
            if (PartTimeJobID == 0) 
            {
                AppTools.ToastError("兼职错误");
                return;
            }
            GameCenter.Instance.ShowTipsUI<ApplicationPartTimeJobTipUI>((ui)=> {
                ui.SetPartTimeJobID(PartTimeJobID);
            });
        }
        /// <summary>
        /// 收藏按钮点击
        /// </summary>
        private void CollectBtnListener()
        { 
            
        }

        public void SetData(string title,int price,byte priceType,string time,string pos,string detail,int id)
        {
            PartTimeJobID = id;
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
