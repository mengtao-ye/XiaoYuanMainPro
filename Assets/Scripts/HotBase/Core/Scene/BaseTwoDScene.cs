using UnityEngine;
using YFramework;

namespace Game
{
    public abstract class BaseTwoDScene : BaseCustomScene
    {
        public override void Awake()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Application.targetFrameRate = 120;//设置刷新率
            YFrameworkHelper.Instance.ScreenSize = new UnityEngine.Vector2(1080, 1920);
            TwoDAwake();
            base.Awake();
        }
        protected virtual void TwoDAwake() 
        { 
           
        }
    }
}
