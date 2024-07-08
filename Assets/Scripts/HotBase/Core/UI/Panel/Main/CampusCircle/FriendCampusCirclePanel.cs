using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class FriendCampusCirclePanel : BaseCustomPanel
    {
        private IScrollView mScrollView;
        private long mLastID;
        private long mFriendAccount;
        private GameObject mNotFindTip;
        public FriendCampusCirclePanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mNotFindTip = transform.FindObject("NotFindTip");
            mScrollView = transform.FindObject("CampusCircleScrollView").AddComponent<PoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mScrollView.SetDownFrashCallBack(GetCampusCircleData);

            transform.FindObject<Button>("BackBtn").onClick.AddListener(BackBtnListener);
        }

        private void BackBtnListener()
        {
            mUICanvas.CloseTopPanel();
        }

        public override void Hide()
        {
            base.Hide();
            mScrollView.ClearItems();
        }

        public override void Show()
        {
            base.Show();
            mNotFindTip.SetActiveExtend(false);
            mScrollView.gameObject.SetActiveExtend(false);
            mScrollView.SetDownFrashState(true);
        }

        private void GetCampusCircleData()
        {
            byte[] sendBytes = ByteTools.Concat(mFriendAccount.ToBytes(), mLastID.ToBytes());
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetFriendCampusCircle, sendBytes);
        }


        public void SetData(IListData<CampusCircleData> listData)
        {
            if (listData.IsNullOrEmpty())
            {
                mScrollView.SetDownFrashState(false);
                mNotFindTip.SetActiveExtend(true);
                mScrollView.gameObject.SetActiveExtend(false);
            }
            else 
            {
                if (listData.Count != 10)
                {
                    //数据获取完了
                    mScrollView.SetDownFrashState(false);
                }
                else 
                { 
                    mScrollView.SetDownFrashState(true);
                }
                mNotFindTip.SetActiveExtend(false);
                mScrollView.gameObject.SetActiveExtend(true);
                mLastID = listData.list.GetLastData().ID;
                for (int i = 0; i < listData.Count; i++)
                {
                    CampusCircleData data = listData[i];
                    CampusCircleScrollViewItem scrollViewItem = ClassPool<CampusCircleScrollViewItem>.Pop();
                    scrollViewItem.hasData = true;
                    scrollViewItem.id = data.ID;
                    scrollViewItem.ViewItemID = data.ID;
                    scrollViewItem.account = data.Account;
                    scrollViewItem.content = data.Content;
                    scrollViewItem.imageTargets = SelectImageDataTools.GetData(data.Images);
                    scrollViewItem.time = data.Time;
                    scrollViewItem.isAnonymous = data.IsAnonymous;
                    scrollViewItem.isFriendCampusCircle = true;
                    mScrollView.Add(scrollViewItem);
                    AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.HasLikeCampusCircleItem, ByteTools.ConcatParam(AppVarData.Account.ToBytes(), data.ID.ToBytes(),((byte)2).ToBytes()));
                    AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetCampusCircleLikeCount, ByteTools.ConcatParam(data.ID.ToBytes(),((byte)2).ToBytes()));
                    AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetCampusCircleCommitCount, ByteTools.ConcatParam(data.ID.ToBytes(),((byte)2).ToBytes()));
                }
            }
        }
        /// <summary>
        /// 设置是否喜欢
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isLike"></param>
        public void SetIsLike(long id,bool isLike)
        {
            CampusCircleScrollViewItem scrollViewItem = mScrollView.Get(id) as CampusCircleScrollViewItem;
            scrollViewItem.SetIsLike(isLike);
        }
        /// <summary>
        /// 是否喜欢
        /// </summary>
        /// <param name="campusCircleID"></param>
        /// <param name="like"></param>
        public void IsLike(long campusCircleID,bool like) 
        {
            CampusCircleScrollViewItem scrollViewItem = mScrollView.Get(campusCircleID) as CampusCircleScrollViewItem;
            scrollViewItem.IsLike(like);
        }
        /// <summary>
        /// 设置喜欢数量
        /// </summary>
        /// <param name="campusCircleID"></param>
        /// <param name="count"></param>
        public void SetLikeCount(long campusCircleID,int count)
        {
            CampusCircleScrollViewItem scrollViewItem = mScrollView.Get(campusCircleID) as CampusCircleScrollViewItem;
            scrollViewItem.SetLikeCount(count);
        }
        public void SetCommitCount(long campusCircleID, int count)
        {
            CampusCircleScrollViewItem scrollViewItem = mScrollView.Get(campusCircleID) as CampusCircleScrollViewItem;
            scrollViewItem.SetCommitCount(count);
        }
        public void ShowContent(long friendAccount)
        {
            mFriendAccount = friendAccount;
            mLastID = long.MaxValue;
            GetCampusCircleData();
        }

    }
}
