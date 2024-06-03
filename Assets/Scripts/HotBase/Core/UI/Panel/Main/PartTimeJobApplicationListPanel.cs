using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class PartTimeJobApplicationListPanel : BaseCustomPanel
    {
        private RecyclePoolScrollView mScrollView;
        private int mLastID;
        private int mPartTimeJobID;
        public PartTimeJobApplicationListPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<MyReleasePartTimeJobDetailPanel>(); });
            mScrollView = transform.Find("ScrollView").AddComponent<RecyclePoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mScrollView.SetDownFrashCallBack(DownCallBack);
            mScrollView.SetDownFrashState(true);
        }

        private void DownCallBack() 
        { 
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetApplicationPartTimeJob, ByteTools.Concat(mPartTimeJobID.ToBytes(), mLastID.ToBytes()));
        }

        public override void Hide()
        {
            base.Hide();
            mScrollView.ClearItems();
        }
        public override void Show()
        {
            base.Show();
            mScrollView.SetDownFrashState(true);
        }
        public void SetPartTimeJobID(int id)
        {
            mPartTimeJobID = id;
            mLastID = int.MaxValue;
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetApplicationPartTimeJob, ByteTools.Concat(mPartTimeJobID.ToBytes(), mLastID.ToBytes()));
        }
        public void SetData(IListData<PartTimeJobApplicationData> data)
        {
            if (data.IsNullOrEmpty())
            {
                //到底了
                mScrollView.SetDownFrashState(false);
            }
            else
            {
                if (data.Count != 10)
                {
                    //到底了
                    mScrollView.SetDownFrashState(false);
                }
                mLastID = data.list.GetLastData().id;
                for (int i = 0; i < data.Count; i++)
                {
                    PartTimeJobApplicationScrollViewItem item = ClassPool<PartTimeJobApplicationScrollViewItem>.Pop();
                    item.id = data[i].id;
                    item.name = data[i].name;
                    item.isMan = data[i].isMan;
                    item.age = data[i].age;
                    item.call = data[i].call;
                    mScrollView.Add(item);
                }
            }
        }
    }
}
