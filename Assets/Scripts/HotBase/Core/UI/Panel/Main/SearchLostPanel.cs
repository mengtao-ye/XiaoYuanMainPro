using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 查找失物招领
    /// </summary>
    public class SearchLostPanel : BaseCustomPanel
    {
        private IScrollView mScrollView;
        private long mLastUpdateTime;
        private InputField mSearchIF;
        private string mSearchKey;
        public SearchLostPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mSearchIF = transform.FindObject<InputField>("SearchIF");
            transform.FindObject<Button>("BackBtn").onClick.AddListener(BackBtnListener);
            mScrollView = transform.FindObject("ScrollView").AddComponent<PoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mScrollView.SetDownFrashCallBack(DownFrashCallback);
            transform.FindObject<Button>("SearchBtn").onClick.AddListener(SearchBtnListener);
        }

        private void BackBtnListener()
        {
            GameCenter.Instance.ShowPanel<LostPanel>();
            mScrollView.ClearItems();
        }

        private void SearchBtnListener() 
        {
            if (mSearchIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入查找内容");
                return;
            }
            mLastUpdateTime = DateTimeTools.GetValueByDateTime(DateTime.MaxValue);
            mSearchKey = mSearchIF.text;
            mScrollView.ClearItems();
            mScrollView.SetDownFrashState(false);
            GetLostList();
        }

        private void DownFrashCallback()
        {
            GetLostList();
        }

        public override void Show()
        {
            base.Show();
            mSearchIF.Select();
            mSearchIF.ActivateInputField();
            mLastUpdateTime = DateTimeTools.GetValueByDateTime(DateTime.MaxValue);
            //GetLostList();
            mScrollView.SetDownFrashState(false);
        }
        private void GetLostList()
        {
            IListData<byte[]> listData = ClassPool<ListData<byte[]>>.Pop();
            listData.Add(SchoolGlobalVarData.SchoolCode.ToBytes());
            listData.Add(mLastUpdateTime.ToBytes());
            listData.Add(mSearchKey.ToBytes());
            byte[] sendBytes = listData.list.ToBytes();
            listData.Recycle();
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.SearchLostList, sendBytes);
        }

        public void SetData(IListData<LostData> data)
        {
            if (data.IsNullOrEmpty())
            {
                mScrollView.SetDownFrashState(false);
            }
            else
            {
                mLastUpdateTime = data.list.GetLastData().updateTime;
                if (data.Count != 3)
                {
                    mScrollView.SetDownFrashState(false);
                }
                else
                {
                    mScrollView.SetDownFrashState(true);
                }
                for (int i = 0; i < data.Count; i++)
                {
                    LostScrollViewItem lostScrollViewItem = ClassPool<LostScrollViewItem>.Pop();
                    lostScrollViewItem.lostData = data[i];
                    mScrollView.Add(lostScrollViewItem);
                }
            }
        }
    }
}
