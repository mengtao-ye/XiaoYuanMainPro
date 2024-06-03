using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

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
        private Button mApplicationBtn;
        private Button mCancelApplicationBtn;
        public bool IsCollection
        {
            get
            {
                return mIsCollection;
            }
            set
            {
                mIsCollection = value;
                if (mIsCollection)
                {
                    mCollectionBtn.targetGraphic.color = Color.red;
                }
                else
                {
                    mCollectionBtn.targetGraphic.color = Color.white;
                }
            }
        }
        private bool mIsCollection;
        private Button mCollectionBtn;
        public PartTimeJobDetailPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mApplicationBtn = transform.FindObject<Button>("ApplicationBtn");
            mApplicationBtn.onClick.AddListener(ApplicationBtnListener);
            mCancelApplicationBtn = transform.FindObject<Button>("CancelApplicationBtn");
            mCancelApplicationBtn.onClick.AddListener(CancelApplicationBtnListener);
            NormalVerticalScrollView normalScrollView = transform.FindObject("ScrollView").AddComponent<NormalVerticalScrollView>();
            normalScrollView.Init();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<PartTimeJobPanel>(); });
            mCollectionBtn = transform.FindObject<Button>("CollectBtn");
            mCollectionBtn.onClick.AddListener(CollectBtnListener);

            mTitle = transform.FindObject<Text>("PartTimeJobTitle");
            mPrice = transform.FindObject<Text>("Price");
            mHead = transform.FindObject<Image>("Head");
            mName = transform.FindObject<Text>("Name");
            mTime = transform.FindObject<Text>("Time");
            mPosition = transform.FindObject<Text>("Position");
            mDetail = transform.FindObject<Text>("Detail");
            mDetailArea = transform.FindObject<RectTransform>("DetailArea");
        }

        public override void Show()
        {
            base.Show();
            IsCollection = false;
          
        }

        /// <summary>
        /// 取消报名按钮
        /// </summary>
        private void CancelApplicationBtnListener()
        {
            if (PartTimeJobID == 0)
            {
                AppTools.ToastError("兼职错误");
                return;
            }
            GameCenter.Instance.ShowTipsUI<CommonTwoTipsUI>((ui) =>
            {
                ui.ShowContent("是否取消报名?", "", "取消", null, "确认", SureCancelApplicationCallBack);
            });
        }
        private void SureCancelApplicationCallBack()
        {
            GameCenter.Instance.HideTipsUI<CommonTwoTipsUI>();
            byte[] sendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), PartTimeJobID.ToBytes());
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.CancelApplicationPartTimeJob, sendBytes);
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
            GameCenter.Instance.ShowTipsUI<ApplicationPartTimeJobTipUI>((ui) =>
            {
                ui.SetPartTimeJobID(PartTimeJobID);
            });
        }
        /// <summary>
        /// 收藏按钮点击
        /// </summary>
        private void CollectBtnListener()
        {
            if (PartTimeJobID == 0)
            {
                AppTools.ToastError("兼职错误");
                return;
            }
            byte[] sendBytes = ByteTools.ConcatParam(AppVarData.Account.ToBytes(), PartTimeJobID.ToBytes());
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.CollectionPartTimeJob, sendBytes);
        }

        public void SetData(string title, int price, byte priceType, string time, string pos, string detail, int id, bool isApplication)
        {
            mApplicationBtn.gameObject.SetActive(!isApplication);
            mCancelApplicationBtn.gameObject.SetActive(isApplication);
            PartTimeJobID = id;
            mTitle.text = title;
            mPrice.text = price + "/" + PartTimeJobTools.GetPriceType(priceType);
            UserDataModule.MapUserData(AppVarData.Account, mHead, mName);
            mTime.text = "工作时间：\n" + time;
            mPosition.text = "工作地点：\n" + pos;
            mDetail.text = "职位详情：\n" + detail;
            mDetailArea.sizeDelta = new Vector2(mDetailArea.sizeDelta.x, mDetail.preferredHeight + 40);

            byte[] sendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), PartTimeJobID.ToBytes());
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.IsCollectionPartTimeJob, sendBytes);
        }
    }
}
