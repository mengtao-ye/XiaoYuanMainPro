using UnityEngine.UI;
using YFramework;

namespace Game
{
    /// <summary>
    /// 查找兼职
    /// </summary>
    public class SearchPartTimeJobPanel : BaseCustomPanel
    {
        private IScrollView mScrollView;
        private int mLastID;
        private InputField mSearchIF;
        private string mSearchKey;
        public SearchPartTimeJobPanel()
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
            GetPartTimeJobList();
        }

        private void DownFrashCallback()
        {
            GetPartTimeJobList();
        }

        public override void Show()
        {
            base.Show();
            mSearchIF.Select();
            mSearchIF.ActivateInputField();
            mLastID = int.MaxValue;
            mScrollView.SetDownFrashState(false);
        }
        private void GetPartTimeJobList()
        {
            IListData<byte[]> listData = ClassPool<ListData<byte[]>>.Pop();
            listData.Add(mLastID.ToBytes());
            listData.Add(mSearchKey.ToBytes());
            byte[] sendBytes = listData.list.ToBytes();
            listData.Recycle();
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.SearchPartTimeJobList, sendBytes);
        }

        public void SetData(IListData<PartTimeJobData> data)
        {
            if (data.IsNullOrEmpty())
            {
                mScrollView.SetDownFrashState(false);
            }
            else
            {
                mLastID = data.list.GetLastData().ID;
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
                    PartTimeJobScrollViewItem item = ClassPool<PartTimeJobScrollViewItem>.Pop();
                    item.title = data[i].Title;
                    item.time = data[i].JobTime;
                    item.position = data[i].Position;
                    item.price = data[i].Price;
                    item.priceType = data[i].PriceType;
                    item.id = data[i].ID;
                    item.detail = data[i].Detail;
                    item.ViewItemID = data[i].ID;
                    item.isMyApplication = false;
                    mScrollView.Add(item);
                }
            }
        }
    }
}
