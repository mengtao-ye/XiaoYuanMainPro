using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class LostPanelListSubUI : BaseCustomSubUI
    {
        private IScrollView mScrollView;
        private long mLastUpdateTime;
        private Image mBottomImage;
        private Text mBottomText;
        public LostPanelListSubUI(Transform trans, Image image,Text text) : base(trans)
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
            transform.FindObject<Button>("SearchBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<SearchLostPanel>(); });
            mBottomImage.color = ColorConstData.BottomNormalColor;
            mBottomText.color = ColorConstData.BottomNormalColor;

        }


        private void DownFrashCallback()
        {
            GetMyLost();
        }

        public override void FirstShow()
        {
            base.FirstShow();
            mLastUpdateTime =  DateTimeTools.MaxValueTime;
            GetMyLost();
            mScrollView.SetDownFrashState(true);
        }

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
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetLostList, sendBytes);
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
                    lostScrollViewItem.isMy = false;

                    mScrollView.Add(lostScrollViewItem);
                }
            }
        }

    }
}
