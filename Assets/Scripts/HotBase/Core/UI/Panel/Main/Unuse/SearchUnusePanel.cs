using UnityEngine.UI;
using YFramework;

namespace Game
{
    /// <summary>
    /// 查找闲置
    /// </summary>
    public class SearchUnusePanel : BaseCustomPanel
    {
        private IScrollView mScrollView;
        private int mLastID;
        private InputField mSearchIF;
        private string mSearchKey;
        public SearchUnusePanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mSearchIF = transform.FindObject<InputField>("SearchIF");
            transform.FindObject<Button>("BackBtn").onClick.AddListener(BackBtnListener);
            mScrollView = transform.FindObject("ScrollView").AddComponent<RecyclePoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mScrollView.SetDownFrashCallBack(DownFrashCallback);
            transform.FindObject<Button>("SearchBtn").onClick.AddListener(SearchBtnListener);
        }

        private void BackBtnListener()
        {
            mUICanvas.CloseTopPanel();
            mScrollView.ClearItems();
        }

        private void SearchBtnListener() 
        {
            if (mSearchIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入查找内容");
                return;
            }
            mLastID = int.MaxValue;
            mSearchKey = mSearchIF.text;
            mScrollView.ClearItems();
            mScrollView.SetDownFrashState(false);
            GetUnuseList();
        }

        private void DownFrashCallback()
        {
            GetUnuseList();
        }

        public override void Show()
        {
            base.Show();
            mSearchIF.Select();
            mSearchIF.ActivateInputField();
            mLastID = int.MaxValue;
            mScrollView.SetDownFrashState(false);
        }
        private void GetUnuseList()
        {
            IListData<byte[]> listData = ClassPool<ListData<byte[]>>.Pop();
            listData.Add(SchoolGlobalVarData.SchoolCode.ToBytes());
            listData.Add(mLastID.ToBytes());
            listData.Add(mSearchKey.ToBytes());
            byte[] sendBytes = listData.list.ToBytes();
            listData.Recycle();
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.SearchUnuseList, sendBytes);
        }

        public void SetData(IListData<UnuseData> data)
        {
            if (data.IsNullOrEmpty())
            {
                mScrollView.SetDownFrashState(false);
            }
            else
            {
                mLastID = data.list.GetLastData().id;
                if (data.Count != 10)
                {
                    mScrollView.SetDownFrashState(false);
                }
                else
                {
                    mScrollView.SetDownFrashState(true);
                }
                for (int i = 0; i < data.Count; i++)
                {
                    UnuseScrollViewItemData lostScrollViewItem = ClassPool<UnuseScrollViewItemData>.Pop();
                    lostScrollViewItem.SetData(data[i]);
                    mScrollView.Add(lostScrollViewItem);
                }
            }
        }
    }
}
