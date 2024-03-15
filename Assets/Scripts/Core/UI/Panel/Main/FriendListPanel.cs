using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class FriendListPanel : BaseCustomPanel
    {
        private Transform mContent;
        public FriendListPanel()
        {

        }
        public override void Awake()
        {
            
            base.Awake();
            mContent = transform.FindObject<Transform>("Content");
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<MainPanel>(); });
            transform.FindObject<Button>("SetBtn").onClick.AddListener(() => { });
        }

        public override void Show()
        {
            base.Show();
            ChatModule.LoadFriendList(mContent);
        }
    }
}
