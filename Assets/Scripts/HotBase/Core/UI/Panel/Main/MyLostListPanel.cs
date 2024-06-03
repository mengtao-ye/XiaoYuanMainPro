using System;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class MyLostListPanel : BaseCustomPanel
    {
        private IScrollView mScrollView;
        private long mLastUpdateTime;
        public MyLostListPanel()
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
        }
        private void BackBtnListener()
        {
            GameCenter.Instance.ShowPanel<LostPanel>();
            mScrollView.ClearItems();
        }

        public override void Show()
        {
            base.Show();
            mLastUpdateTime =  DateTimeTools.MaxValueTime;
            GetMyLost();
            mScrollView.SetDownFrashState(true);
        }

        private void DownFrashCallback()
        {
            GetMyLost();
        }

        private void GetMyLost()
        {
            byte[] sendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), mLastUpdateTime.ToBytes());
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetMyLostList, sendBytes);
        }
        public void SetData(IListData<LostData> data)
        {
            if (data.IsNullOrEmpty())
            {
                mScrollView.SetDownFrashState(false);
            }
            else
            {
                mLastUpdateTime = data.list.GetLastData().updateTime;
                if (data.Count != 3)
                {
                    mScrollView.SetDownFrashState(false);
                }
                for (int i = 0; i < data.Count; i++)
                {
                    LostScrollViewItem lostScrollViewItem = ClassPool<LostScrollViewItem>.Pop();
                    lostScrollViewItem.lostData = data[i];
                    lostScrollViewItem.ViewItemID = data[i].id;
                    lostScrollViewItem.isMy = true;
                    mScrollView.Add(lostScrollViewItem);
                }
            }
        }

        public void DeleteLostItem(int id)
        {
            mScrollView.Delete(id);
        }

    }
}
