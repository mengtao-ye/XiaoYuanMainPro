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
                if (SchoolGlobalVarData.SchoolCode == 0)
                {
                    AppTools.ToastNotify("请选择学校");
                    return;
                }
                GameCenter.Instance.ShowPanel<CampusCirclePanel>();
            }) ;
            transform.FindObject<Button>("LostFoundItem").onClick.AddListener(() => {
                GameCenter.Instance.ShowPanel<LostPanel>();
            });
            transform.FindObject<Button>("PartJobItem").onClick.AddListener(() => {
                GameCenter.Instance.ShowPanel<PartTimeJobPanel>();
            });
            transform.FindObject<Button>("UnuseItem").onClick.AddListener(() => {
                GameCenter.Instance.ShowPanel<UnusePanel>();
            });

        }

    }
}
