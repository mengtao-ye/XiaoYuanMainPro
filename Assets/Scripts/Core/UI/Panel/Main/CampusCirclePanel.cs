using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class CampusCirclePanel : BaseCustomPanel
    {
        public CampusCirclePanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() =>
            {
                GameCenter.Instance.ShowPanel<MainPanel>();
            });
            transform.FindObject<Button>("AddBtn").onClick.AddListener(()=>
            {
                GameCenter.Instance.ShowPanel<PublishCampusCirclePanel>();
            }) ;
        }
    }
}
