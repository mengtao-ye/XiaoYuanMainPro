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
        public CampusCirclePanel()
        {

        }
        public override void Awake()
        {
            base.Awake();

            mScrollView = transform.FindObject("CampusCircleScrollView").AddComponent<PoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);

            transform.FindObject<Button>("BackBtn").onClick.AddListener(() =>
            {
                GameCenter.Instance.ShowPanel<MainPanel>();
            });
            transform.FindObject<Button>("AddBtn").onClick.AddListener(() =>
            {
                GameCenter.Instance.ShowPanel<PublishCampusCirclePanel>();
            });
        }
        public override void FirstShow()
        {
            base.FirstShow();
            mLastID = long.MaxValue;
            mScrollView.ClearItems();
            IListData<byte[]> list = ClassPool<ListData<byte[]>>.Pop();
            list.Add(AppVarData.Account.ToBytes());
            list.Add(mLastID.ToBytes());
            list.Add(SchoolGlobalVarData.SchoolCode.ToBytes());
            byte[] sendBytes = list.list.ToBytes();
            list.Recycle();
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetCampusCircle, sendBytes);
        }

        public void GetCampusCircle(IListData<long> listData)
        {
            mLastID = listData[0];
            for (int i = 0; i < listData.Count; i++)
            {
                CampusCircleScrollViewItem campusCircleScrollViewItem = ClassPool<CampusCircleScrollViewItem>.Pop();
                campusCircleScrollViewItem.id = listData[i];
                campusCircleScrollViewItem.ViewItemID = listData[i];
                AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetCampusCircleItemDetail, listData[i].ToBytes());
                AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.HasLikeCampusCircleItem, ByteTools.Concat(AppVarData.Account.ToBytes(), listData[i].ToBytes()));
                mScrollView.Add(campusCircleScrollViewItem);
            }
        }
        public void GetDetailData(CampusCircleData data)
        {
            CampusCircleScrollViewItem scrollViewItem = mScrollView.Get(data.ID) as CampusCircleScrollViewItem;
            scrollViewItem.hasData = true;
            scrollViewItem.id = data.ID;
            scrollViewItem.account = data.Account;
            scrollViewItem.content = data.Content;
            scrollViewItem.images = data.Images;
            scrollViewItem.time = data.Time;
            scrollViewItem.isAnonymous = data.IsAnonymous;
            scrollViewItem.likeCount = data.LikeCount;
            scrollViewItem.commitCount = data.CommitCount;
            scrollViewItem.SetData();
        }
        public void SetLike(long id,bool isLike,bool needUpdate)
        {
            CampusCircleScrollViewItem scrollViewItem = mScrollView.Get(id) as CampusCircleScrollViewItem;
            scrollViewItem.SetIsLike(isLike, needUpdate);
        }
    }
}
