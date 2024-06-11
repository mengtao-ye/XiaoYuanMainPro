using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class FriendListPanel : BaseCustomPanel
    {
        private IRecycleScrollView mScrollView;
        private Dictionary<char, Text> mCharDict;
        private char mCurPinYin;
        private GameObject mFriendScrollView;
        private GameObject mNotFindTip;
        public FriendListPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mFriendScrollView =  transform.FindObject("FriendScrollView");
            mNotFindTip =  transform.FindObject("NotFindTip");
            mFriendScrollView.SetAvtiveExtend(false);
            mNotFindTip.SetAvtiveExtend(false);
            mCurPinYin = PinYinConstData.PinYinArray[PinYinConstData.LEN - 1];
            mCharDict = new Dictionary<char, Text>();
            Transform pinYinArea = transform.Find("PinYinSortArea");
            for (int i = 0; i < pinYinArea.childCount; i++)
            {
                Transform pinYin = pinYinArea.GetChild(i);
                char ch = pinYin.name.ToCharArray()[0];
                if (ch == '#')
                {
                    ch = PinYinConstData.DEFAULT;
                }
                mCharDict.Add(ch, pinYin.Find("T").GetComponent<Text>());
            }

            mScrollView = transform.FindObject("FriendScrollView").AddComponent<RecyclePoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { mUICanvas.CloseTopPanel(); });
            mScrollView.SetDragCallBack(DragScrollViewCallBack);
        }

        private void DragScrollViewCallBack()
        {
            SetCurPinYin();
        }

        public override void Show()
        {
            base.Show();
            bool res = ChatModule.LoadFriendList(mScrollView);
            mFriendScrollView.SetAvtiveExtend(res);
            mNotFindTip.SetAvtiveExtend(!res);
        }

        public override void Hide()
        {
            base.Hide();
            mScrollView.ClearItems();
        }
        private void SetCurPinYin()
        {
            if (mScrollView.listData.IsNullOrEmpty()) return;
            IScrollViewItem scrollViewItem = mScrollView.topScrollViewItem;
            if (scrollViewItem == null) return;
            FriendScrollViewItem friendScrollViewItem = scrollViewItem as FriendScrollViewItem;
            if (mCurPinYin != friendScrollViewItem.pinYinChar)
            {
                mCharDict[mCurPinYin].fontStyle = FontStyle.Normal;
                mCharDict[friendScrollViewItem.pinYinChar].fontStyle = FontStyle.Bold;
                mCurPinYin = friendScrollViewItem.pinYinChar;
            }
        }
        public void DeleteFriend(long friendAccount)
        {
            if (mScrollView.Contains(friendAccount))
            {
                mScrollView.Delete(friendAccount);
            }
        }
    }
}
