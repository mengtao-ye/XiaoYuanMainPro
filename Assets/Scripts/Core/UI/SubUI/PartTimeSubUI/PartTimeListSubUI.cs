using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class PartTimeListSubUI : BaseCustomSubUI
    {
        private PoolScrollView mScrollView;
        private int mLastID;
        private int mCount;
        public PartTimeListSubUI(Transform trans) : base(trans)
        {

        }
        public override void Awake()
        {
            base.Awake();
            mScrollView = transform.Find("ScrollView").AddComponent<PoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mScrollView.SetDownFrashState(true);
            mScrollView.SetDownFrashCallBack(DownFrashCallback);
        }
        private void DownFrashCallback()
        {
            mCount = 5;
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetPartTimeJobList, mLastID.ToBytes());

        }
        public override void Show()
        {
            base.Show();
            mLastID = int.MaxValue;
            mCount = 5;
            mScrollView.SetDownFrashState(true);
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetPartTimeJobList, mLastID.ToBytes());
        }
        public override void Hide()
        {
            base.Hide();
            mScrollView.ClearItems();
        }
        public void SetData(MyReleasePartTimeJobData data)
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
                mScrollView.Add(item);
                mCount--;
                if (mCount > 0)
                {
                    AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetPartTimeJobList, mLastID.ToBytes());
                }
            }
        }
    }
}
