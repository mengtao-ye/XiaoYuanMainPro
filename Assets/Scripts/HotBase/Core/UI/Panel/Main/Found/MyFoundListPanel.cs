using System;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class MyFoundListPanel : BaseCustomPanel
    {
        private IScrollView mScrollView;
        private long mLastUpdateTime;
        private bool mIsClear;
        public MyFoundListPanel()
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
            mUICanvas.CloseTopPanel();
            mScrollView.ClearItems();
            mIsClear = true;
        }

        public override void Show()
        {
            base.Show();
            if (mIsClear) {
                mLastUpdateTime =  DateTimeTools.MaxValueTime;
                GetMyFound();
                mScrollView.SetDownFrashState(true);
                mIsClear = false;
            }
        }

        private void DownFrashCallback()
        {
            GetMyFound();
        }

        private void GetMyFound()
        {
            byte[] sendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), mLastUpdateTime.ToBytes());
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetMyFoundList, sendBytes);
        }
        public void SetData(IListData<FoundData> data)
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
                    FoundScrollViewItem lostScrollViewItem = ClassPool<FoundScrollViewItem>.Pop();
                    lostScrollViewItem.foundData= data[i];
                    lostScrollViewItem.ViewItemID = data[i].id;
                    lostScrollViewItem.isMy = true;
                    mScrollView.Add(lostScrollViewItem);
                }
            }
        }

        public void DeleteFoundItem(int id)
        {
            mScrollView.Delete(id);
        }
    }
}
