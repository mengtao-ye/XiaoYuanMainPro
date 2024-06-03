using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class PartTimeListSubUI : BaseCustomSubUI
    {
        private RecyclePoolScrollView mScrollView;
        private int mLastID;
        private int mCount;
        private Image mBottomImage;
        private Text mBottomText;
        public PartTimeListSubUI(Transform trans, Image image, Text text) : base(trans)
        {
            mBottomImage = image;
            mBottomText = text;
        }
        public override void Awake()
        {
            base.Awake();
            mScrollView = transform.Find("ScrollView").AddComponent<RecyclePoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mScrollView.SetDownFrashState(true);
            mScrollView.SetDownFrashCallBack(DownFrashCallback);
            transform.FindObject<Button>("SearchBtn").onClick.AddListener(SearchBtnListener);
        }

        public override void Show()
        {
            base.Show();
            mBottomImage.color = ColorConstData.BottomSelectColor;
            mBottomText.color = ColorConstData.BottomSelectColor;
        }
        public override void Hide()
        {
            base.Hide();
            mBottomImage.color = ColorConstData.BottomNormalColor;
            mBottomText.color = ColorConstData.BottomNormalColor;
        }

        private void SearchBtnListener() {
            GameCenter.Instance.ShowPanel<SearchPartTimeJobPanel>();
        }

        private void DownFrashCallback()
        {
            mCount = 5;
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetPartTimeJobList, mLastID.ToBytes());

        }
        public override void FirstShow()
        {
            base.FirstShow();
            mLastID = int.MaxValue;
            mCount = 5;
            mScrollView.SetDownFrashState(true);
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetPartTimeJobList, mLastID.ToBytes());
        }
      
        public void SetData(PartTimeJobData data)
        {
            if (data == null)
            {
                //到底了   
                mScrollView.SetDownFrashState(false);
            }
            else
            {
                mLastID = data.ID;
                PartTimeJobScrollViewItem item = ClassPool<PartTimeJobScrollViewItem>.Pop();
                item.title = data.Title;
                item.time = data.JobTime;
                item.position = data.Position;
                item.price = data.Price;
                item.priceType = data.PriceType;
                item.id = data.ID;
                item.detail = data.Detail;
                item.isMyApplication = false;
                mScrollView.Add(item);
                mCount--;
                if (mCount > 0)
                {
                    AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetPartTimeJobList, mLastID.ToBytes());
                }
            }
        }
    }
}
