namespace Game
{
    public static class ChatPathData
    {
        #region ChatMsg
        /// <summary>
        /// 聊天列表目录
        /// </summary>
        /// <returns></returns>
        public static string ChatMsgDetailDir(long account, long friendAccount)
        {
            return ChatMsgDir(account) + "/" + friendAccount;
        }
        /// <summary>
        /// 聊天列表目录
        /// </summary>
        /// <returns></returns>
        public static string ChatMsgDir(long account)
        {
            return ChatDir + "/ChatMsg/" + account.ToString();
        }
        #endregion
        #region FirendList
        /// <summary>
        /// 好友申请列表目录
        /// </summary>
        /// <returns></returns>
        public static string AddFriendListDir()
        {
            return ChatDir + "/AddFriendList";
        }
        /// <summary>
        /// 好友列表目录
        /// </summary>
        /// <returns></returns>
        public static string FriendListDir()
        {
            return ChatDir + "/FriendList";
        }

        /// <summary>
        /// 最新的消息ID地址
        /// </summary>
        public static string AddFriendIDPath
        {
            get
            {
                return ChatDir + "/AddFriendID.txt";
            }
        }
        /// <summary>
        /// 最新的消息ID地址
        /// </summary>
        public static string FriendListIDPath
        {
            get
            {
                return ChatDir + "/FriendListID.txt";
            }
        } 
        #endregion
        #region ChatList
        /// <summary>
        /// 最新的消息ID地址
        /// </summary>
        public static string ChatListIDPath
        {
            get
            {
                return ChatDir + "/ChatID.txt";
            }
        }
        /// <summary>
        /// 聊天列表目录
        /// </summary>
        /// <returns></returns>
        public static string ChatListDir()
        {
            return ChatDir + "/ChatList";
        } 
        #endregion
        /// <summary>
        /// 获取最上级目录
        /// </summary>
        /// <returns></returns>
        public static string ChatDir
        {
            get 
            {
#if UNITY_EDITOR
                return ApplicationData.ProjectPath + "/ChatData";
#else
            return Application.persistentDataPath + "/ChatData";
#endif
            }
        }
    }
}
