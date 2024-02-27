using YFramework;

namespace Game
{
    /// <summary>
    /// 公共的UI配置文件
    /// </summary>
    public class UIMapper : SingleMap<string, UIMapperData, UIMapper>
    {
        /// <summary>
        /// UI根目录
        /// </summary>
        private const string UI_ROOT_DIR = "Prefabs/UI";
        /// <summary>
        /// UI面板目录
        /// </summary>
        private const string UI_PANEL_DIR = UI_ROOT_DIR+"/Panel";
        /// <summary>
        /// 登录场景目录
        /// </summary>
        private const string UI_LOGIN_DIR = UI_PANEL_DIR + "/Login";

        /// <summary>
        /// Log面板目录
        /// </summary>
        private const string UI_LOG_DIR = UI_ROOT_DIR + "/Log";

        /// <summary>
        /// Tips面板目录
        /// </summary>
        private const string UI_TIPUI_DIR = UI_ROOT_DIR + "/TipsUI";

        /// <summary>
        /// CommonTips面板目录
        /// </summary>
        private const string UI_COMMON_TIPUI_DIR = UI_TIPUI_DIR + "/CommonTipsUI";
        protected override void Config()
        {
            //Panel
            AddUI<LoginPanel>(UI_LOGIN_DIR);
            AddUI<RegisterPanel>(UI_LOGIN_DIR);
            //Log
            AddUI<MidLogUI>(UI_LOG_DIR);
            //Tip
            AddUI<CommonOneTipsUI>(UI_COMMON_TIPUI_DIR);
            AddUI<CommonTwoTipsUI>(UI_COMMON_TIPUI_DIR);
        }
        /// <summary>
        /// 注册面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentPath"></param>
        protected void AddUI<T>(string parentPath) where T : IUI
        {
            Add(typeof(T).Name, new UIMapperData(parentPath +"/"+ typeof(T).Name));

        }
    }
}
