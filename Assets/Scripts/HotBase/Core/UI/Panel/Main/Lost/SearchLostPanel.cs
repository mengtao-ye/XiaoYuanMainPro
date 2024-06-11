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
        private GameObject mCententArea;
        private GameObject mNotFindTip;
        public SearchLostPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mCententArea = transform.Find("CententArea").gameObject;
            mNotFindTip = transform.Find("NotFindTip").gameObject;
          
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
            mLastUpdateTime = DateTimeTools.MaxValueTime;
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
            mLastUpdateTime =  DateTimeTools.MaxValueTime;
            //GetLostList();
            mScrollView.SetDownFrashState(false);
            mCententArea.SetActive(false);
            mNotFindTip.SetActive(false);
        }
        private void GetLostList()
        {
            IListData<byte[]> listData = ClassPool<ListData<byte[]>>.Pop();
            listData.Add(SchoolGlobalVarData.SchoolCode.ToBytes());
            listData.Add(mLastUpdateTime.ToBytes());
            listData.Add(mSearchKey.ToBytes());
            byte[] sendBytes = listData.list.ToBytes();
            listData.Recycle();
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.SearchLostList, sendBytes);
        }

        public void SetData(IListData<LostData> data)
        {
            if (data.IsNullOrEmpty())
            {
                mScrollView.SetDownFrashState(false);
                if (mLastUpdateTime == DateTimeTools.MaxValueTime) 
                {
                    mCententArea.SetAvtiveExtend(false);
                    mNotFindTip.SetAvtiveExtend(true);
                }
            }
            else
            {
                mCententArea.SetAvtiveExtend(true);
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
                    LostScrollViewItem lostScrollViewItem = ClassPool<LostScrollViewItem>.Pop();
                    lostScrollViewItem.lostData = data[i];
                    lostScrollViewItem.isMy = false;
                    mScrollView.Add(lostScrollViewItem);
                }
            }
        }
    }
}
