using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 查找寻物
    /// </summary>
    public class SearchFoundPanel : BaseCustomPanel
    {
        private IScrollView mScrollView;
        private long mLastUpdateTime;
        private InputField mSearchIF;
        private string mSearchKey;
        private GameObject mCenterArea;
        private GameObject mNotFindTip;
        public SearchFoundPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mCenterArea = transform.FindObject("CenterArea");
            mNotFindTip = transform.FindObject("NotFindTip");
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
            mLastUpdateTime =  DateTimeTools.MaxValueTime;
            mSearchKey = mSearchIF.text;
            mScrollView.ClearItems();
            mScrollView.SetDownFrashState(false);
            GetFoundList();
        }

        private void DownFrashCallback()
        {
            GetFoundList();
        }

        public override void Show()
        {
            base.Show();
            mSearchIF.Select();
            mSearchIF.ActivateInputField();
            mLastUpdateTime =  DateTimeTools.MaxValueTime;
            mScrollView.SetDownFrashState(false);
            mCenterArea.SetAvtiveExtend(false);
            mNotFindTip.SetAvtiveExtend(false);
        }
        private void GetFoundList()
        {
            IListData<byte[]> listData = ClassPool<ListData<byte[]>>.Pop();
            listData.Add(SchoolGlobalVarData.SchoolCode.ToBytes());
            listData.Add(mLastUpdateTime.ToBytes());
            listData.Add(mSearchKey.ToBytes());
            byte[] sendBytes = listData.list.ToBytes();
            listData.Recycle();
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.SearchFoundList, sendBytes);
        }

        public void SetData(IListData<FoundData> data)
        {
            if (data.IsNullOrEmpty())
            {
                if (mLastUpdateTime == DateTimeTools.MaxValueTime) 
                {
                    mNotFindTip.SetAvtiveExtend(true);
                }
                mScrollView.SetDownFrashState(false);
            }
            else
            {
                mCenterArea.SetAvtiveExtend(true);
                mNotFindTip.SetAvtiveExtend(false);
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
                    FoundScrollViewItem lostScrollViewItem = ClassPool<FoundScrollViewItem>.Pop();
                    lostScrollViewItem.foundData = data[i];
                    lostScrollViewItem.isMy = false;
                    mScrollView.Add(lostScrollViewItem);
                }
            }
        }
    }
}
