using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class MinePageSubUI : BaseCustomSubUI
    {
        private MainPanel mMainPanel;
        public MinePageSubUI(Transform trans, MainPanel mainPanel) : base(trans)
        {
            mMainPanel = mainPanel;
        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("SetBtn").onClick.AddListener(() => { });

        }
    }
}
