using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class CampusCirclePanel : BaseCustomPanel
    {
        private IScrollView mScrollView;
        private long mLastID;
        private GameObject mNotFindTip;

        public CampusCirclePanel()
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
            mScrollView.SetDownFrashState(true);
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() =>
            {
                mUICanvas.CloseTopPanel();
            });
            transform.FindObject<Button>("AddBtn").onClick.AddListener(() =>
            {
                GameCenter.Instance.ShowPanel<PublishCampusCirclePanel>();
            });
        }

        public override void FirstShow()
        {
            base.FirstShow();
            mNotFindTip.SetActiveExtend(false);
            mScrollView.gameObject.SetActiveExtend(false);
            mLastID = long.MaxValue;
            mScrollView.ClearItems();
            GetCampusCircleData();
        }

        private void GetCampusCircleData()
        {
            IListData<byte[]> list = ClassPool<ListData<byte[]>>.Pop();
            list.Add(AppVarData.Account.ToBytes());
            list.Add(mLastID.ToBytes());
            list.Add(SchoolGlobalVarData.SchoolCode.ToBytes());
            byte[] sendBytes = list.list.ToBytes();
            list.Recycle();
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetCampusCircle, sendBytes);
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
                    scrollViewItem.isFriendCampusCircle = false;
                    mScrollView.Add(scrollViewItem);
                    AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.HasLikeCampusCircleItem, ByteTools.ConcatParam(AppVarData.Account.ToBytes(), data.ID.ToBytes(), ((byte)1).ToBytes()));
                    AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetCampusCircleLikeCount, ByteTools.ConcatParam(data.ID.ToBytes(), ((byte)1).ToBytes()));
                    AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetCampusCircleCommitCount, ByteTools.ConcatParam(data.ID.ToBytes(), ((byte)1).ToBytes()));
                }
            }
        }
        /// <summary>
        /// 设置喜欢数量
        /// </summary>
        /// <param name="campusCircleID"></param>
        /// <param name="count"></param>
        public void SetLikeCount(long campusCircleID, int count)
        {
            CampusCircleScrollViewItem scrollViewItem = mScrollView.Get(campusCircleID) as CampusCircleScrollViewItem;
            scrollViewItem.SetLikeCount(count);
        }
        /// <summary>
        /// 设置评论数量
        /// </summary>
        /// <param name="campusCircleID"></param>
        /// <param name="count"></param>
        public void SetCommitCount(long campusCircleID, int count)
        {
            CampusCircleScrollViewItem scrollViewItem = mScrollView.Get(campusCircleID) as CampusCircleScrollViewItem;
            scrollViewItem.SetCommitCount(count);
        }
        /// <summary>
        /// 设置是否喜欢（操作层面）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isLike"></param>
        public void SetIsLike(long id,bool isLike)
        {
            CampusCircleScrollViewItem scrollViewItem = mScrollView.Get(id) as CampusCircleScrollViewItem;
            scrollViewItem.SetIsLike(isLike);
        }
        /// <summary>
        /// 是否喜欢（显示层面）
        /// </summary>
        /// <param name="campusCircleID"></param>
        /// <param name="like"></param>
        public void IsLike(long campusCircleID, bool like)
        {
            CampusCircleScrollViewItem scrollViewItem = mScrollView.Get(campusCircleID) as CampusCircleScrollViewItem;
            scrollViewItem.IsLike(like);
        }

    }
}
