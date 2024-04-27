using YFramework;

namespace Game
{
    public class MetaSchoolScene : BaseCustomScene
    {
        protected override string mSceneName => SceneID.MetaSchoolScene.ToString();
        public override void Awake()
        {
            Init();
            canvas = new MetaSchoolCanvas(this,UIMapper.Instance);
            model = new MetaSchoolModel(this,new UnityEngine.GameObject("_Model"));
            controller = new MetaSchoolController(this);
            base.Awake();
        }

        private void Init() 
        {
            YFrameworkHelper.Instance.ScreenSize = new UnityEngine.Vector2(1624, 750);
        }

        public override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            IProcess process = GameCenter.Instance.processController.Create()
                   .Concat(new CheckMainServerIsInitProcess())
                .Concat(new GetLoginServerPointProcess())
                ;
            process.processManager.Launcher();
#endif
        }
    }
}
