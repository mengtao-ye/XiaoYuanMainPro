using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 失物招领/寻物启事
    /// </summary>
    public class LostPanel : BaseCustomPanel
    {
        private IScrollView mScrollView;
        private long mLastUpdateTime;
        public LostPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<MainPanel>(); });
            transform.FindObject<Button>("PublishBtn").onClick.AddListener(() =>
            {
                GameCenter.Instance.ShowPanel<PublishLostPanel>();
            });
            mScrollView = transform.FindObject("ScrollView").AddComponent<PoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mScrollView.SetDownFrashCallBack(DownFrashCallback);

            //Button button = transform.FindObject<Button>("ScreenBtn");
            //button.onClick.AddListener(ScreenBtnListener);
            //mLostScreenPos = button.GetComponent<RectTransform>().position;
            transform.FindObject<Button>("SearchBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<SearchLostPanel>(); });
        }


        //private void ScreenBtnListener()
        //{
        //    GameCenter.Instance.ShowTipsUI<LostScreenTipUI>((ui) =>
        //    {
        //        ui.transform.position = mLostScreenPos;
        //    });
        //}

        private void DownFrashCallback()
        {
            GetMyLost();
        }

        public override void FirstShow()
        {
            base.FirstShow();
            mLastUpdateTime = DateTimeTools.GetValueByDateTime(DateTime.MaxValue);
            GetMyLost();
            mScrollView.SetDownFrashState(true);
        }
        //public override void Hide()
        //{
        //    base.Hide();
        //    mScrollView.ClearItems();
        //}
        private void GetMyLost()
        {
            IDictionaryData<byte, byte[]> dict = ClassPool<DictionaryData<byte, byte[]>>.Pop();
            dict.Add(0, SchoolGlobalVarData.SchoolCode.ToBytes());
            dict.Add(1, mLastUpdateTime.ToBytes());
            byte[] sendBytes = dict.data.ToBytes();
            dict.Recycle();
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetLostList, sendBytes);
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
