using System;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class MyApplicationPartTimeJobListPanel : BaseCustomPanel
    {
        private IScrollView mScrollView;
        private int mLastID;
        private bool mIsClear;
        public MyApplicationPartTimeJobListPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(BackBtnListener);
            mScrollView = transform.FindObject("ScrollView").AddComponent<RecyclePoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mScrollView.SetDownFrashCallBack(DownFrashCallback);
            mIsClear = true;
        }
        private void BackBtnListener()
        {
            GameCenter.Instance.ShowPanel<PartTimeJobPanel>();
            mScrollView.ClearItems();
            mIsClear = true;
        }

        public override void Show()
        {
            base.Show();
            if (mIsClear)
            {
                mLastID = int.MaxValue ;
                GetMyApplicationPartTimeJob();
                mScrollView.SetDownFrashState(true);
                mIsClear = false;
            }
        }

        private void DownFrashCallback()
        {
            GetMyApplicationPartTimeJob();
        }

        private void GetMyApplicationPartTimeJob()
        {
            byte[] sendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), mLastID.ToBytes());
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetMyApplicationPartTimeJobList, sendBytes);
        }
        public void SetIDsData(IListData<PartTimeJobData> data)
        {
            if (data.IsNullOrEmpty())
            {
                //没数据了
                mScrollView.SetDownFrashState(false);
            }
            else
            {
                if (data.Count != 10)
                {
                    //没数据了
                    mScrollView.SetDownFrashState(false);
                }
                mLastID = data.list.GetLastData().ID;
                for (int i = 0; i < data.Count; i++)
                {
                    PartTimeJobScrollViewItem item = ClassPool<PartTimeJobScrollViewItem>.Pop();
                    item.title = data[i].Title;
                    item.time = data[i].JobTime;
                    item.position = data[i].Position;
                    item.price = data[i].Price;
                    item.priceType = data[i].PriceType;
                    item.id = data[i].ID;
                    item.detail = data[i].Detail;
                    item.ViewItemID = data[i].ID;
                    item.isMyApplication = true;
                    mScrollView.Add(item);
                }
            }
        }

        public void DeleteScrollViewItem(int id)
        {
            mScrollView.Delete(id);
        }
    }
}
