using YFramework;

namespace Game
{
    /// <summary>
    /// 公共的UI配置文件
    /// </summary>
    public class UIMapper : SingleMap<string, UIMapperData, UIMapper>
    {
        #region Root
        /// <summary>
        /// UI根目录
        /// </summary>
        private const string UI_ROOT_DIR = "Prefabs/UI"; 
        #endregion
        #region Panel
        /// <summary>
        /// UI面板目录
        /// </summary>
        private const string UI_PANEL_DIR = UI_ROOT_DIR + "/Panel";
        /// <summary>
        /// 登录场景目录
        /// </summary>
        private const string UI_LOGIN_DIR = UI_PANEL_DIR + "/Login";
        #region Main
        /// <summary>
        /// 主场景目录
        /// </summary>
        private const string UI_MAIN_DIR = UI_PANEL_DIR + "/Main";
        /// <summary>
        /// 校友圈
        /// </summary>
        private const string UI_MAIN_CampusCircle_DIR = UI_MAIN_DIR + "/CampusCircle";
        /// <summary>
        /// 聊天
        /// </summary>
        private const string UI_MAIN_Chat_DIR = UI_MAIN_DIR + "/Chat";
        /// <summary>
        /// 寻物
        /// </summary>
        private const string UI_MAIN_Found_DIR = UI_MAIN_DIR + "/Found";
        /// <summary>
        /// 好友
        /// </summary>
        private const string UI_MAIN_Friend_DIR = UI_MAIN_DIR + "/Friend";
        /// <summary>
        /// 失物
        /// </summary>
        private const string UI_MAIN_Lost_DIR = UI_MAIN_DIR + "/Lost";
        /// <summary>
        /// 主要
        /// </summary>
        private const string UI_MAIN_Main_DIR = UI_MAIN_DIR + "/Main";
        /// <summary>
        /// 兼职
        /// </summary>
        private const string UI_MAIN_PartTimeJob_DIR = UI_MAIN_DIR + "/PartTimeJob";
        /// <summary>
        /// 角色
        /// </summary>
        private const string UI_MAIN_Role_DIR = UI_MAIN_DIR + "/Role";
        /// <summary>
        /// 学校
        /// </summary>
        private const string UI_MAIN_School_DIR = UI_MAIN_DIR + "/School";
        /// <summary>
        /// 设置
        /// </summary>
        private const string UI_MAIN_Setting_DIR = UI_MAIN_DIR + "/Setting";
        /// <summary>
        /// 闲置
        /// </summary>
        private const string UI_MAIN_Unuse_DIR = UI_MAIN_DIR + "/Unuse";
        #endregion
        /// <summary>
        /// 校园目录
        /// </summary>
        private const string UI_METASCHOOL_DIR = UI_PANEL_DIR + "/MetaSchool"; 
        #endregion
        #region Log
        /// <summary>
        /// Log面板目录
        /// </summary>
        private const string UI_LOG_DIR = UI_ROOT_DIR + "/Log"; 
        #endregion
        #region Tip
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
        #endregion
        protected override void Config()
        {
            #region Panel
            #region Login
            //Login
            AddUI<LoginPanel>(UI_LOGIN_DIR);
            AddUI<RegisterPanel>(UI_LOGIN_DIR);
            #endregion
            #region Main
            //Main
            AddUI<MainPanel>(UI_MAIN_Main_DIR);
            AddUI<SelectSchoolPanel>(UI_MAIN_School_DIR);
            AddUI<ChatPanel>(UI_MAIN_Chat_DIR);
            AddUI<FindFriendPanel>(UI_MAIN_Friend_DIR);
            AddUI<SendAddFriendPanel>(UI_MAIN_Friend_DIR);
            AddUI<AddFriendRequestViewPanel>(UI_MAIN_Friend_DIR);
            AddUI<FriendListPanel>(UI_MAIN_Friend_DIR);
            AddUI<PublishCampusCirclePanel>(UI_MAIN_CampusCircle_DIR);
            AddUI<CampusCirclePanel>(UI_MAIN_CampusCircle_DIR);
            AddUI<LostPanel>(UI_MAIN_Lost_DIR);
            AddUI<PublishLostPanel>(UI_MAIN_Lost_DIR);
            AddUI<PartTimeJobPanel>(UI_MAIN_PartTimeJob_DIR);
            AddUI<BusinessPartTimeJobPanel>(UI_MAIN_PartTimeJob_DIR);
            AddUI<ReleasePartTimeJobPanel>(UI_MAIN_PartTimeJob_DIR);
            AddUI<MyReleasePartTimeJobDetailPanel>(UI_MAIN_PartTimeJob_DIR);
            AddUI<PartTimeJobDetailPanel>(UI_MAIN_PartTimeJob_DIR);
            AddUI<PartTimeJobApplicationListPanel>(UI_MAIN_PartTimeJob_DIR);
            AddUI<UnusePanel>(UI_MAIN_Unuse_DIR);
            AddUI<ReleaseUnusePanel>(UI_MAIN_Unuse_DIR);
            AddUI<UnuseDetailPanel>(UI_MAIN_Unuse_DIR);
            AddUI<SelectRolePanel>(UI_MAIN_Role_DIR);
            AddUI<LostDetailPanel>(UI_MAIN_Lost_DIR);
            AddUI<SearchLostPanel>(UI_MAIN_Lost_DIR);
            AddUI<MyLostListPanel>(UI_MAIN_Lost_DIR);
            AddUI<MyLostDetailPanel>(UI_MAIN_Lost_DIR);
            AddUI<FoundPanel>(UI_MAIN_Found_DIR);
            AddUI<PublishFoundPanel>(UI_MAIN_Found_DIR);
            AddUI<MyFoundListPanel>(UI_MAIN_Found_DIR);
            AddUI<FoundDetailPanel>(UI_MAIN_Found_DIR);
            AddUI<SearchFoundPanel>(UI_MAIN_Found_DIR);
            AddUI<MyApplicationPartTimeJobListPanel>(UI_MAIN_PartTimeJob_DIR);
            AddUI<MyCollectionPartTimeJobListPanel>(UI_MAIN_PartTimeJob_DIR);
            AddUI<MyReleaseUnuseListPanel>(UI_MAIN_Unuse_DIR);
            AddUI<MyCollectionUnuseListPanel>(UI_MAIN_Unuse_DIR);
            AddUI<SearchUnusePanel>(UI_MAIN_Unuse_DIR);
            AddUI<SearchPartTimeJobPanel>(UI_MAIN_PartTimeJob_DIR);
            AddUI<UserMainPagePanel>(UI_MAIN_Chat_DIR);
            AddUI<FriendCampusCirclePanel>(UI_MAIN_CampusCircle_DIR);
            AddUI<SettingPanel>(UI_MAIN_Setting_DIR);
            AddUI<AccountSettingPanel>(UI_MAIN_Setting_DIR);
            AddUI<SchoolSettingPanel>(UI_MAIN_Setting_DIR);
            #endregion
            #region MetaSchool
            //MetaSchool
            AddUI<MetaSchoolMainPanel>(UI_METASCHOOL_DIR);
            AddUI<LoadMetaSchoolSceneDataPanel>(UI_METASCHOOL_DIR);
            AddUI<MetaSchoolSetPanel>(UI_METASCHOOL_DIR);  
            AddUI<ChangeSkinPanel>(UI_METASCHOOL_DIR);
            #endregion
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
            AddUI<SelectSexTipUI>(UI_Main_TIPUI_DIR);
            AddUI<SelectBirthdayTipUI>(UI_Main_TIPUI_DIR);
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
