using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class CommitTipUI : BaseCustomTipsUI
    {
        private IScrollView mScrollView;
        private long mCampusCircleID;
        private long mLastID;
        protected override ShowAnimEnum ShowAnim => ShowAnimEnum.BottomToTopPos;
        protected override HideAnimEnum HideAnim => HideAnimEnum.TopToBottomPos;
        protected override float ShowAnimTime => 0.25f;
        protected override float HideAnimTime => 0.25f;
        private bool mIsFirstOpen;
        public CommitTipUI()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mCampusCircleID = 0;
            mLastID = long.MaxValue;
            transform.FindObject<Button>("CloseBtn").onClick.AddListener(Hide);
            mScrollView = transform.FindObject("ScrollView").AddComponent<CustomScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mScrollView.SetDownFrashCallBack(DownFrashCallBack);
        }
        public void SetCampusCircleID(long campusCircleID)
        {
            mCampusCircleID = campusCircleID;
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetCommit, ByteTools.Concat(mCampusCircleID.ToBytes(), mLastID.ToBytes()));
        }

        private void DownFrashCallBack()
        {
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetCommit, ByteTools.Concat(mCampusCircleID.ToBytes(), mLastID.ToBytes()));
        }

        public override void Hide()
        {
            base.Hide();
            mScrollView.ClearItems();
            mCampusCircleID = 0;
            mLastID = long.MaxValue;
            mIsFirstOpen = false;
        }
        public override void Show()
        {
            base.Show();
            mScrollView.SetDownFrashState(true);
        }
        public void SetData(IListData<CampusCircleCommitData> listData)
        {
            bool isEnd = false;
            if (listData.IsNullOrEmpty())
            {
                //到底了
                mScrollView.SetDownFrashState(false);
                isEnd = true;
            }
            else
            {
                if (listData.Count != 5)
                {
                    //到底了
                    mScrollView.SetDownFrashState(false);
                    isEnd = true;
                }
                mLastID = listData.list.GetLastData().ID;
                for (int i = 0; i < listData.Count; i++)
                {
                    CampusCircleCommitScrollViewItem campusCircleCommitScrollViewItem = ClassPool<CampusCircleCommitScrollViewItem>.Pop();
                    campusCircleCommitScrollViewItem.ID = listData[i].ID;
                    campusCircleCommitScrollViewItem.Account = listData[i].Account;
                    campusCircleCommitScrollViewItem.CampusCircleID = listData[i].CampusCircleID;
                    campusCircleCommitScrollViewItem.ReplayID = listData[i].ReplayID;
                    campusCircleCommitScrollViewItem.Content = listData[i].Content;
                    mScrollView.Add(campusCircleCommitScrollViewItem);
                }
                if (!isEnd)
                {

                    if (!mIsFirstOpen) 
                    {
                        mIsFirstOpen = true;
                        AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetCommit, ByteTools.Concat(mCampusCircleID.ToBytes(), mLastID.ToBytes()));
                    }
                }
            }

        }
    }
}
