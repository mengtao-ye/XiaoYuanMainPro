using UnityEngine;
using YFramework;

namespace Game
{
    public class LoginScene : BaseTwoDScene
    {
        protected override string mSceneName => SceneID.LoginScene.ToString();
        protected override void TwoDAwake()
        {
            base.TwoDAwake();
            canvas = new LoginCanvas(this, UIMapper.Instance);
            model = new LoginModel(this, new GameObject("_Model"));
        }
    }
}
