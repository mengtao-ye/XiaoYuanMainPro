using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class FriendListPanel : BaseCustomPanel
    {
        private IScrollView mScrollView;
        public FriendListPanel()
        {
              
        }
        public override void Awake()
        {

            base.Awake();
            mScrollView = transform.FindObject("FriendScrollView").AddComponent<CustomScrollView>() ;
            mScrollView.Init();
            mScrollView.SetSpace(10,10,10);
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<MainPanel>(); });
            transform.FindObject<Button>("SetBtn").onClick.AddListener(() => { });
        }

        public override void Show()
        {
            base.Show();
            ChatModule.LoadFriendList(mScrollView);
        }
    }
}
