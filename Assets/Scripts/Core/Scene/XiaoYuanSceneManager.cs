using YFramework;

namespace Game
{
    public class XiaoYuanSceneManager  : SceneManager
    {
        public XiaoYuanSceneManager(Center center, IMap<string, IScene> map) : base(center, map)
        {

        }
        public override void Awake()
        {
            ChangeScene( UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
