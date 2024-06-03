using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class FoundPanelListSubUI : BaseCustomSubUI
    {
        private IScrollView mScrollView;
        private long mLastUpdateTime;
        private Image mBottomImage;
        private Text mBottomText;
        public FoundPanelListSubUI(Transform trans, Image image, Text text) : base(trans)
        {
            mBottomImage = image;
            mBottomText = text;
        }
        public override void Awake()
        {
            base.Awake();
         
            mScrollView = transform.FindObject("ScrollView").AddComponent<RecyclePoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mScrollView.SetDownFrashCallBack(DownFrashCallback);
            transform.FindObject<Button>("SearchBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<SearchFoundPanel>(); });
        }


        private void DownFrashCallback()
        {
            GetMyFound();
        }

        public override void FirstShow()
        {
            base.FirstShow();
            mLastUpdateTime =  DateTimeTools.MaxValueTime;
            GetMyFound();
            mScrollView.SetDownFrashState(true);
        }
        //public override void Hide()
        //{
        //    base.Hide();
        //    mScrollView.ClearItems();
        //}
        public override void Show()
        {
            base.Show();
            mBottomImage.color = ColorConstData.BottomSelectColor;
            mBottomText.color = ColorConstData.BottomSelectColor;
        }
        public override void Hide()
        {
            base.Hide();
            mBottomImage.color = ColorConstData.BottomNormalColor;
            mBottomText.color = ColorConstData.BottomNormalColor;
        }
        private void GetMyFound()
        {
            byte[] sendBytes = ByteTools.Concat(SchoolGlobalVarData.SchoolCode.ToBytes(), mLastUpdateTime.ToBytes());
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetFoundList, sendBytes);
        }

        public void SetData(IListData<FoundData> data)
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
                    FoundScrollViewItem foundScrollViewItem = ClassPool<FoundScrollViewItem>.Pop();
                    foundScrollViewItem.foundData = data[i];
                    foundScrollViewItem.isMy = false;
                    mScrollView.Add(foundScrollViewItem);
                }
            }
        }

    }
}
