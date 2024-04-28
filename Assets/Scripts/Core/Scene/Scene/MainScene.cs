using UnityEngine;
using YFramework;

namespace Game
{
    public class MainScene : BaseCustomScene
    {
        protected override string mSceneName =>SceneID.MainScene.ToString();
        public override void Awake()
        {
            Application.targetFrameRate = 120;//设置刷新率
            YFrameworkHelper.Instance.ScreenSize = new UnityEngine.Vector2(1080, 1920);
            canvas = new MainCanvas(this,UIMapper.Instance);
            model = new MainModel(this, new GameObject("_Model"));
            base.Awake();
        }
    }
}
