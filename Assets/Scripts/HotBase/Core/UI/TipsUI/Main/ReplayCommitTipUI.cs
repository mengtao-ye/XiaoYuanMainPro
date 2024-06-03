using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class ReplayCommitTipUI : BaseCustomTipsUI
    {
        private IScrollView mScrollView;
        private long mCommitID;
        private long mLastID;
        protected override ShowAnimEnum ShowAnim => ShowAnimEnum.BottomToTopPos;
        protected override HideAnimEnum HideAnim => HideAnimEnum.TopToBottomPos;
        protected override float ShowAnimTime => 0.25f;
        protected override float HideAnimTime => 0.25f;
        private InputField mCommitIF;
        public ReplayCommitTipUI()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mCommitID = 0;
            mLastID = long.MaxValue;
            rectTransform.FindObject<Button>("CloseBtn").onClick.AddListener(Hide);
            mScrollView = rectTransform.FindObject("ScrollView").AddComponent<RecyclePoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mScrollView.SetDownFrashCallBack(GetReplayCommitData);
            mCommitIF = transform.FindObject<InputField>("CommitIF");
            rectTransform.FindObject<Button>("SendBtn").onClick.AddListener(SendCommitBtnListener);
        }

        private void SendCommitBtnListener() 
        {
            if (mCommitIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("评论不能为空");
                return;
            }
            IListData<byte[]> list = ClassPool<ListData<byte[]>>.Pop();
            list.Add(AppVarData.Account.ToBytes());
            list.Add(mCommitID.ToBytes());
            list.Add(mCommitIF.text.ToBytes());
            list.Add(((long)0).ToBytes());
            byte[] sendBytes = list.list.ToBytes();
            list.Recycle();
            GameCenter.Instance.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.SendCampCircleReplayCommit, sendBytes);
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="campusCircleID"></param>
        public void ShowContent(long commitID)
        {
            mCommitID = commitID;
            GetReplayCommitData();
        }

        private void GetReplayCommitData()
        {
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetReplayCommit, ByteTools.ConcatParam(mCommitID.ToBytes(), mLastID.ToBytes()));
        }

        public override void Hide()
        {
            base.Hide();
            mScrollView.ClearItems();
            mCommitID = 0;
            mLastID = long.MaxValue;
        }
        public override void Show()
        {
            base.Show();
            mScrollView.SetDownFrashState(true);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="listData"></param>
        public void SetData(IListData<CampusCircleReplayCommitData> listData)
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
                    CampusCircleReplayCommitScrollViewItem campusCircleCommitScrollViewItem = ClassPool<CampusCircleReplayCommitScrollViewItem>.Pop();
                    campusCircleCommitScrollViewItem.ID = listData[i].ID;
                    campusCircleCommitScrollViewItem.ViewItemID = listData[i].ID;
                    campusCircleCommitScrollViewItem.ReplayCommitID = listData[i].ReplayCommitID;
                    campusCircleCommitScrollViewItem.Account = listData[i].Account;
                    campusCircleCommitScrollViewItem.Content = listData[i].Content;
                    campusCircleCommitScrollViewItem.ReplayID = listData[i].ReplayID;
                    campusCircleCommitScrollViewItem.ReplayContent = listData[i].ReplayContent;
                    mScrollView.Add(campusCircleCommitScrollViewItem);
                }
            }
        }
        /// <summary>
        /// 添加我评论的数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <param name="commitID"></param>
        /// <param name="commit"></param>
        public void AddCommit(long id,long account,string commit,string replayContent)
        {
            CampusCircleReplayCommitScrollViewItem campusCircleCommitScrollViewItem = ClassPool<CampusCircleReplayCommitScrollViewItem>.Pop();
            campusCircleCommitScrollViewItem.ID = id;
            campusCircleCommitScrollViewItem.ViewItemID = id;
            campusCircleCommitScrollViewItem.Account = account;
            campusCircleCommitScrollViewItem.Content = commit;
            campusCircleCommitScrollViewItem.ReplayContent = replayContent;
            mScrollView.Insert(campusCircleCommitScrollViewItem,0);
        }
        public void DeleteCommit(long id)
        {
            mScrollView.Delete(id);
        }
    }
}
