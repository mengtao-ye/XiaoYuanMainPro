using YFramework;

namespace Game
{
    public class MetaSchoolScene : BaseCustomScene
    {
        protected override string mSceneName => SceneID.MetaSchoolScene.ToString();
        public override void Awake()
        {
            YFrameworkHelper.Instance.ScreenSize = new UnityEngine.Vector2(1624,750);
            canvas = new MetaSchoolCanvas(this,UIMapper.Instance);
            model = new MetaSchoolModel(this,new UnityEngine.GameObject("_Model"));
            controller = new MetaSchoolController(this);
            base.Awake();
        }
    }
}
