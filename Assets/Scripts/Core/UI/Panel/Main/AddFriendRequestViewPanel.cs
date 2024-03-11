using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class AddFriendRequestViewPanel : BaseCustomPanel
    {
        private Transform mContent;
        public AddFriendRequestViewPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<MainPanel>(); });
            mContent = transform.FindObject<Transform>("Content");
        }
        public override void Show()
        {
            base.Show();
            ChatModule.LoadAddFriendList(mContent);
        }
    }
}
