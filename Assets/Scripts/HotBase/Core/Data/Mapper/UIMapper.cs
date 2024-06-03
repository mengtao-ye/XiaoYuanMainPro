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
        /// 主场景目录
        /// </summary>
        private const string UI_MAIN_DIR = UI_PANEL_DIR + "/Main";
        /// <summary>
        /// 校园目录
        /// </summary>
        private const string UI_METASCHOOL_DIR = UI_PANEL_DIR + "/MetaSchool";
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

        /// <summary>
        /// Main面板目录
        /// </summary>
        private const string UI_Main_TIPUI_DIR = UI_TIPUI_DIR + "/Main";
        protected override void Config()
        {
            #region Panel
            //Login
            AddUI<LoginPanel>(UI_LOGIN_DIR);
            AddUI<RegisterPanel>(UI_LOGIN_DIR);
            //Main
            AddUI<MainPanel>(UI_MAIN_DIR);
            AddUI<SelectSchoolPanel>(UI_MAIN_DIR);
            AddUI<ChatPanel>(UI_MAIN_DIR);
            AddUI<FindFriendPanel>(UI_MAIN_DIR);
            AddUI<SendAddFriendPanel>(UI_MAIN_DIR);
            AddUI<AddFriendRequestViewPanel>(UI_MAIN_DIR);
            AddUI<FriendListPanel>(UI_MAIN_DIR);
            AddUI<PublishCampusCirclePanel>(UI_MAIN_DIR);
            AddUI<CampusCirclePanel>(UI_MAIN_DIR);
            AddUI<LostPanel>(UI_MAIN_DIR);
            AddUI<PublishLostPanel>(UI_MAIN_DIR);
            AddUI<PartTimeJobPanel>(UI_MAIN_DIR);
            AddUI<BusinessPartTimeJobPanel>(UI_MAIN_DIR);
            AddUI<ReleasePartTimeJobPanel>(UI_MAIN_DIR);
            AddUI<MyReleasePartTimeJobDetailPanel>(UI_MAIN_DIR);
            AddUI<PartTimeJobDetailPanel>(UI_MAIN_DIR);
            AddUI<PartTimeJobApplicationListPanel>(UI_MAIN_DIR);
            AddUI<UnusePanel>(UI_MAIN_DIR);
            AddUI<ReleaseUnusePanel>(UI_MAIN_DIR);
            AddUI<UnuseDetailPanel>(UI_MAIN_DIR);
            AddUI<SelectRolePanel>(UI_MAIN_DIR);
            AddUI<LostDetailPanel>(UI_MAIN_DIR);
            AddUI<SearchLostPanel>(UI_MAIN_DIR);
            AddUI<MyLostListPanel>(UI_MAIN_DIR);
            AddUI<MyLostDetailPanel>(UI_MAIN_DIR);
            AddUI<FoundPanel>(UI_MAIN_DIR);
            AddUI<PublishFoundPanel>(UI_MAIN_DIR);
            AddUI<MyFoundListPanel>(UI_MAIN_DIR);
            AddUI<FoundDetailPanel>(UI_MAIN_DIR);
            AddUI<SearchFoundPanel>(UI_MAIN_DIR);
            AddUI<MyApplicationPartTimeJobListPanel>(UI_MAIN_DIR);
            AddUI<MyCollectionPartTimeJobListPanel>(UI_MAIN_DIR);
            AddUI<MyReleaseUnuseListPanel>(UI_MAIN_DIR);
            AddUI<MyCollectionUnuseListPanel>(UI_MAIN_DIR);
            AddUI<SearchUnusePanel>(UI_MAIN_DIR);
            AddUI<SearchPartTimeJobPanel>(UI_MAIN_DIR);
            AddUI<UserMainPagePanel>(UI_MAIN_DIR);
            AddUI<FriendCampusCirclePanel>(UI_MAIN_DIR);
            //MetaSchool
            AddUI<MetaSchoolMainPanel>(UI_METASCHOOL_DIR);
            AddUI<LoadMetaSchoolSceneDataPanel>(UI_METASCHOOL_DIR);
            AddUI<MetaSchoolSetPanel>(UI_METASCHOOL_DIR); 
            #endregion
            #region Log
            AddUI<MidLogUI>(UI_LOG_DIR);
            AddUI<LoadingLogUI>(UI_LOG_DIR); 
            #endregion

            #region TipUI
            //Common
            AddUI<CommonOneTipsUI>(UI_COMMON_TIPUI_DIR);
            AddUI<CommonTwoTipsUI>(UI_COMMON_TIPUI_DIR);
            AddUI<ChoiceTimeTipUI>(UI_COMMON_TIPUI_DIR);
            AddUI<ShowMultiImageTipUI>(UI_COMMON_TIPUI_DIR);
            AddUI<CommonInputFieldTipUI>(UI_COMMON_TIPUI_DIR);
            //Main
            AddUI<NotifyTipUI>(UI_Main_TIPUI_DIR);
            AddUI<CommitTipUI>(UI_Main_TIPUI_DIR);
            AddUI<ApplicationPartTimeJobTipUI>(UI_Main_TIPUI_DIR);
            AddUI<LostScreenTipUI>(UI_Main_TIPUI_DIR);
            AddUI<ChatListItemTipUI>(UI_Main_TIPUI_DIR);
            AddUI<ContactTipUI>(UI_Main_TIPUI_DIR);
            AddUI<FoundQuestTipUI>(UI_Main_TIPUI_DIR);
            AddUI<ReplayCommitTipUI>(UI_Main_TIPUI_DIR);  
            #endregion

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
