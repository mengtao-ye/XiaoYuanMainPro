using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 失物招领
    /// </summary>
    public class LostPanel : BaseCustomPanel
    {
        private IScrollView mScrollView;
        private int mLastID;
        public LostPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<MainPanel>(); });
            transform.FindObject<Button>("PublishBtn").onClick.AddListener(() =>
            {
                GameCenter.Instance.ShowPanel<PublishLostPanel>();
            });
            mScrollView = transform.FindObject("ScrollView").AddComponent<PoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mScrollView.SetDownFrashCallBack(DownFrashCallback);
        }

        private void DownFrashCallback() 
        {
            GetMyLost();
        }

        public override void Show()
        {
            base.Show();
            mLastID = 0;
            GetMyLost();
            mScrollView.SetDownFrashState(true);
        }
        private void GetMyLost()
        {
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetMyLostData, ByteTools.Concat(AppVarData.Account.ToBytes(), mLastID.ToBytes()));
        }

        public void SetData(IListData<LostData> data)
        {
            if (data.IsNullOrEmpty())
            {
                mScrollView.SetDownFrashState(false);
            }
            else
            {
                mLastID = data.list.GetLastData().id;
                if (data.Count != 3)
                {
                    mScrollView.SetDownFrashState(false);
                }
                for (int i = 0; i < data.Count; i++)
                {
                    LostScrollViewItem lostScrollViewItem = ClassPool<LostScrollViewItem>.Pop();
                    LostData lostData = data[i];
                    lostScrollViewItem.id = lostData.id;
                    lostScrollViewItem.name = lostData.name;
                    lostScrollViewItem.pos = lostData.pos;
                    lostScrollViewItem.startTime = lostData.startTime;
                    lostScrollViewItem.endTime = lostData.endTime;
                    lostScrollViewItem.account = lostData.account;
                    lostScrollViewItem.images = lostData.images;
                    mScrollView.Add(lostScrollViewItem);
                }
            }
        }
    }
}
