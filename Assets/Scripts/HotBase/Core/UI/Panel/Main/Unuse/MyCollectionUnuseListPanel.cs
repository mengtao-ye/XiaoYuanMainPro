using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class MyCollectionUnuseListPanel : BaseCustomPanel
    {
        private IScrollView mScrollView;
        private int mLastID;
        private bool mIsClear;
        public MyCollectionUnuseListPanel()
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
            if (mIsClear)
            {
                mLastID = int.MaxValue ;
                GetMyCollectionUnuse();
                mScrollView.SetDownFrashState(true);
                mIsClear = false;
            }
        }

        private void DownFrashCallback()
        {
            GetMyCollectionUnuse();
        }

        private void GetMyCollectionUnuse()
        {
            byte[] sendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), mLastID.ToBytes());
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetMyCollectionUnuseList, sendBytes);
        }
        public void SetData(IListData<UnuseData> data)
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
                mLastID = data.list.GetLastData().id;
                for (int i = 0; i < data.Count; i++)
                {
                    UnuseScrollViewItemData item = ClassPool<UnuseScrollViewItemData>.Pop();
                    item.SetData(data[i]);
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
