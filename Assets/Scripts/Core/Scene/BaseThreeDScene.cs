using UnityEngine;
using YFramework;

namespace Game
{
    public abstract class BaseThreeDScene : BaseCustomScene
    {
        public override  void Awake()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Application.targetFrameRate = 60;//设置刷新率
            YFrameworkHelper.Instance.ScreenSize = new UnityEngine.Vector2(1624, 750);
            ThreeDAwake();
            base.Awake();
        }
        protected virtual void ThreeDAwake()
        {

        }
    }
}
