using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class MenuPageSubUI : BaseCustomSubUI
    {
        private MainPanel mMainPanel;
        public MenuPageSubUI(Transform trans, MainPanel mainPanel) : base(trans)
        {
            mMainPanel = mainPanel;
        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("CampusCircle").onClick.AddListener(() => {
                if (SchoolVarData.SchoolCode == 0)
                {
                    AppTools.ToastNotify("请选择学校");
                    return;
                }
                GameCenter.Instance.ShowPanel<CampusCirclePanel>();
            }) ;
        }
    }
}
