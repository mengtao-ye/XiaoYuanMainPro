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

        protected override void Config()
        {
            AddUI<LoginPanel>(UI_LOGIN_DIR);
            AddUI<MidLogUI>(UI_LOG_DIR);
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
