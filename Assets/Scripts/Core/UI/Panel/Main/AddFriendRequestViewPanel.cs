using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class AddFriendRequestViewPanel : BaseCustomPanel
    {
        public IScrollView scrollView { get; private set; }
        public AddFriendRequestViewPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            scrollView = transform.FindObject("NewFriendScrollView").AddComponent<CustomScrollView>();
            scrollView.Init();
            scrollView.SetSpace(10,10,10);
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<MainPanel>(); });
        }
        public override void Show()
        {
            base.Show();
            ChatModule.LoadAddFriendList(scrollView);
        }
    }
}
