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
        private InputField mCommitIF;
        public CommitTipUI()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mCampusCircleID = 0;
            mLastID = long.MaxValue;
            rectTransform.FindObject<Button>("CloseBtn").onClick.AddListener(Hide);
            mScrollView = rectTransform.FindObject("ScrollView").AddComponent<RecyclePoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mScrollView.SetDownFrashCallBack(GetCommitData);
            mCommitIF = transform.FindObject<InputField>("CommitIF");
            rectTransform.FindObject<Button>("SendBtn").onClick.AddListener(SendCommitBtnListener);
        }

        private void SendCommitBtnListener() 
        {
            if (mCommitIF.text.IsNullOrEmpty()) {
                AppTools.ToastNotify("评论不能为空");
                return;
            }
            IListData<byte[]> list = ClassPool<ListData<byte[]>>.Pop();
            list.Add(AppVarData.Account.ToBytes());
            list.Add(mCampusCircleID.ToBytes());
            list.Add(mCommitIF.text.ToBytes());
            byte[] sendBytes = list.list.ToBytes();
            list.Recycle();
            AppTools.TcpSend( TcpSubServerType.Login,(short)TcpLoginUdpCode.SendCampCircleCommit,sendBytes);
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="campusCircleID"></param>
        public void ShowContent(long campusCircleID)
        {
            mCampusCircleID = campusCircleID;
            mScrollView.SetDownFrashState(true);
            GetCommitData();
        }

        private void GetCommitData()
        {
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetCommit, ByteTools.ConcatParam(mCampusCircleID.ToBytes(), mLastID.ToBytes()));
        }

        public override void Hide()
        {
            base.Hide();
            mScrollView.ClearItems();
            mCampusCircleID = 0;
            mLastID = long.MaxValue;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="listData"></param>
        public void SetData(IListData<CampusCircleCommitData> listData)
        {
            if (listData.IsNullOrEmpty())
            {
                //到底了
                mScrollView.SetDownFrashState(false);
            }
            else
            {
                if (listData.Count != 10)
                {
                    //到底了
                    mScrollView.SetDownFrashState(false);
                }
                mLastID = listData.list.GetLastData().ID;
                for (int i = 0; i < listData.Count; i++)
                {
                    CampusCircleCommitScrollViewItem campusCircleCommitScrollViewItem = ClassPool<CampusCircleCommitScrollViewItem>.Pop();
                    campusCircleCommitScrollViewItem.ID =listData[i].ID;
                    campusCircleCommitScrollViewItem.ViewItemID = listData[i].ID;
                    campusCircleCommitScrollViewItem.Account = listData[i].Account;
                    campusCircleCommitScrollViewItem.Content = listData[i].Content;
                    campusCircleCommitScrollViewItem.ReplayCount = listData[i].ReplayCount;
                    mScrollView.Add(campusCircleCommitScrollViewItem);
                }
            }
        }
        /// <summary>
        /// 添加我评论的数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <param name="campusCircleID"></param>
        /// <param name="commit"></param>
        public void AddCommit(long id,long account,string commit)
        {
            CampusCircleCommitScrollViewItem campusCircleCommitScrollViewItem = ClassPool<CampusCircleCommitScrollViewItem>.Pop();
            campusCircleCommitScrollViewItem.ID = id;
            campusCircleCommitScrollViewItem.ViewItemID = id;
            campusCircleCommitScrollViewItem.Account = account;
            campusCircleCommitScrollViewItem.Content = commit;
            campusCircleCommitScrollViewItem.ReplayCount = 0;
            mScrollView.Insert(campusCircleCommitScrollViewItem,0);
        }

        public void DeleteCommit(long id)
        {
            mScrollView.Delete(id);           
        }
    }
}
