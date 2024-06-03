using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 招聘
    /// </summary>
    public class RecruitSubUI : BaseCustomSubUI
    {
        private IScrollView mScrollView;
        private int mLastID;
        private int mCount;
        private Image mBottomImage;
        private Text mBottomText;
        public RecruitSubUI(Transform trans, Image image, Text text) : base(trans)
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
        }

        private void DownFrashCallback()
        {
            mCount = 5;
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetMyReleasePartTimeJob, ByteTools.Concat(AppVarData.Account.ToBytes(), mLastID.ToBytes()));
        }

        public override void Show()
        {
            base.Show();
            mCount = 5;
            mLastID = int.MaxValue;
            mScrollView.SetDownFrashState(true);
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetMyReleasePartTimeJob, ByteTools.Concat(AppVarData.Account.ToBytes(), mLastID.ToBytes()));
            mBottomImage.color = ColorConstData.BottomSelectColor;
            mBottomText.color = ColorConstData.BottomSelectColor;
        }

        public override void Hide()
        {
            base.Hide();
            mScrollView.ClearItems();
            mBottomImage.color = ColorConstData.BottomNormalColor;
            mBottomText.color = ColorConstData.BottomNormalColor;
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
                MyReleasePartTimeJobScrollViewItem item = ClassPool<MyReleasePartTimeJobScrollViewItem>.Pop();
                item.title = data.Title;
                item.time = data.JobTime;
                item.position = data.Position;
                item.price = data.Price;
                item.priceType = data.PriceType;
                item.id = data.ID;
                item.detail = data.Detail;
                mScrollView.Add(item);
                mCount--;
                if (mCount > 0)
                {
                    AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetMyReleasePartTimeJob, ByteTools.Concat(AppVarData.Account.ToBytes(), mLastID.ToBytes()));
                }
            }
        }
    }
}
