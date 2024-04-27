using UnityEngine;
using YFramework;

namespace Game
{
    public class LoginScene : BaseCustomScene
    {
        protected override string mSceneName => SceneID.LoginScene.ToString();
        public override void Awake()
        {
            YFrameworkHelper.Instance.ScreenSize = new UnityEngine.Vector2(1080, 1920);
            canvas = new LoginCanvas(this,UIMapper.Instance);
            model = new LoginModel(this, new GameObject("_Model"));
            base.Awake();
        }
    }
}
