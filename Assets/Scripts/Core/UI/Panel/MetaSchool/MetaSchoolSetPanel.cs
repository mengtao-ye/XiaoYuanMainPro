using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class MetaSchoolSetPanel : BaseCustomPanel
    {
        public MetaSchoolSetPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(()=> { GameCenter.Instance.ShowPanel<MetaSchoolMainPanel>(); });
            transform.FindObject<Button>("ExitSceneBtn").onClick.AddListener(()=> 
            {
                GameCenter.Instance.LoadScene( SceneID.MainScene, ABTagEnum.Main);
            });

        }
    }
}
