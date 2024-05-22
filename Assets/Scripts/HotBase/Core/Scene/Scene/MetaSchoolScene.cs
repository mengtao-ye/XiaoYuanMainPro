using UnityEngine;
using YFramework;

namespace Game
{
    public class MetaSchoolScene : BaseThreeDScene
    {
        protected override string mSceneName => SceneID.MetaSchoolScene.ToString();
        protected override void ThreeDAwake()
        {
            base.ThreeDAwake();
            canvas = new MetaSchoolCanvas(this, UIMapper.Instance);
            model = new MetaSchoolModel(this, new UnityEngine.GameObject("_Model"));
            controller = new MetaSchoolController(this);
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
