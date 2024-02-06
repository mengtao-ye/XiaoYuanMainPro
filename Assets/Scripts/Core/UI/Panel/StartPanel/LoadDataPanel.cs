using UnityEngine.UI;
using YFramework;

namespace Game
{
    /// <summary>
    /// 游戏运行时加载数据面板
    /// </summary>
    public class LoadDataPanel : BaseCustomPanel
    {
        private Text mContent;
        private Image mSlider;
        public LoadDataPanel()
        {

        }
        public override void Start()
        {
            base.Start();
            mContent = transform.FindObject<Text>("Content");
            mSlider = transform.FindObject<Image>("SilderValue");
            mSlider.fillAmount = 0;
            //IProcess process =  GameCenter.Instance.processController.Create()
            //       .Concat(new CheckUdpServerIsInitProcess())
            //    .Concat(new GetLoginServerPointProcess())
            //    .Concat(new LoadSuccessProcess())
            //    ;
            //process.processManager.Launcher();
        }
    }
}