namespace Game
{

    public enum TcpLoginUdpCode : short
    {
        //目前到了  52  

        //Base
        LoginHeartBeat = TcpRequestCode.LoginServer +1,//心跳包
        LoginAccount = TcpRequestCode.LoginServer +2,//账号登录
        RegisterAccount = TcpRequestCode.LoginServer + 3,//注册账号
        GetUserData = TcpRequestCode.LoginServer + 4,//获取用户数据
        GetMySchool = TcpRequestCode.LoginServer + 5,//获取我的学校
        GetSchoolData = TcpRequestCode.LoginServer + 6,//获取学校数据
        SearchSchool = TcpRequestCode.LoginServer + 7,//查找学校    
        JoinSchool = TcpRequestCode.LoginServer + 8,//加入学校
        //Chat
        GetNewChatMsg = TcpRequestCode.LoginServer + 9,//获取最新的聊天信息
        SendChatMsg = TcpRequestCode.LoginServer + 10,//发送聊天消息
        GetFriendList = TcpRequestCode.LoginServer + 11,//获取好友列表
        SearchFriendData = TcpRequestCode.LoginServer + 12,//查找好友
        SendAddFriendRequest = TcpRequestCode.LoginServer + 13,//发送添加好友请求
        GetAddFriendRequest = TcpRequestCode.LoginServer + 14,//获取好友申请请求
        RefuseFriend = TcpRequestCode.LoginServer + 15,//拒绝好友申请
        ConfineFriend = TcpRequestCode.LoginServer + 16,//同意好友申请
        ChangeNotes = TcpRequestCode.LoginServer +53,//修改备注
        DeleteFriend = TcpRequestCode.LoginServer + 61,//删除好友
        IsFriend = TcpRequestCode.LoginServer + 62,//是否是好友

        //CampusCircle
        PublishCampusCircle = TcpRequestCode.LoginServer + 17,//发表校友圈
        GetCampusCircle = TcpRequestCode.LoginServer + 18,//获取朋友圈
        LikeCampusCircleItem = TcpRequestCode.LoginServer + 20,//朋友圈点赞 
        HasLikeCampusCircleItem = TcpRequestCode.LoginServer + 21,//是否朋友圈点赞 
        GetCommit = TcpRequestCode.LoginServer + 22,//获取评论信息
        GetFriendCampusCircle = TcpRequestCode.LoginServer + 54,//获取好友的校友圈
        GetCampusCircleLikeCount = TcpRequestCode.LoginServer + 55,//获取朋友圈点赞个数
        GetCampusCircleCommitCount = TcpRequestCode.LoginServer + 56,//获取朋友圈评论个数
        SendCampCircleCommit = TcpRequestCode.LoginServer + 19,//发送朋友圈评论
        SendCampCircleReplayCommit = TcpRequestCode.LoginServer + 57,//发送朋友圈回复评论
        GetReplayCommit = TcpRequestCode.LoginServer + 58,//获取回复评论信息
        DeleteCommit = TcpRequestCode.LoginServer + 59,//删除评论
        DeleteReplayCommit = TcpRequestCode.LoginServer + 60,//删除回复评论

        //Lost
        PublishLostData = TcpRequestCode.LoginServer + 23,//发表失物招领
        GetMyLostList = TcpRequestCode.LoginServer + 24,//获取我的失物招领
        GetLostList = TcpRequestCode.LoginServer + 34,//获取我的失物招领
        SearchLostList = TcpRequestCode.LoginServer + 35,//查找失物列表
        DeleteLost = TcpRequestCode.LoginServer + 36,//删除失物招领

        //Found
        PublishFoundData = TcpRequestCode.LoginServer + 37,//发表寻物招领
        GetFoundList = TcpRequestCode.LoginServer + 38,//获取我的寻物招领
        GetMyFoundList = TcpRequestCode.LoginServer + 39,//获取我的寻物
        DeleteFound = TcpRequestCode.LoginServer + 40,//删除寻物
        SearchFoundList = TcpRequestCode.LoginServer +41,//查找寻物

        //PartTimeJob
        ReleasePartTimeJob = TcpRequestCode.LoginServer + 25,//发布兼职
        GetMyReleasePartTimeJob = TcpRequestCode.LoginServer + 26,//获取我发布的兼职
        GetPartTimeJobList = TcpRequestCode.LoginServer + 27,//获取兼职列表
        ApplicationPartTimeJob = TcpRequestCode.LoginServer + 28,//报名兼职
        GetApplicationPartTimeJob = TcpRequestCode.LoginServer + 29,//获取报名兼职列表
        GetMyApplicationPartTimeJobList = TcpRequestCode.LoginServer + 42,//获取我的兼职报名列表
        CancelApplicationPartTimeJob = TcpRequestCode.LoginServer + 43,//取消报名
        CollectionPartTimeJob = TcpRequestCode.LoginServer + 44,//操作收藏兼职
        IsCollectionPartTimeJob = TcpRequestCode.LoginServer + 45,//是否收藏兼职
        GetMyCollectionPartTimeJobList = TcpRequestCode.LoginServer + 46,//获取我收藏的兼职报名列表
        SearchPartTimeJobList = TcpRequestCode.LoginServer + 52,//查找兼职列表

        //Unuse
        ReleaseUnuse = TcpRequestCode.LoginServer + 30,//发布闲置
        GetUnuseList = TcpRequestCode.LoginServer + 31,//获取闲置列表
        GetMyReleaseUnuseList = TcpRequestCode.LoginServer + 47,//获取我的闲置列表
        IsCollectionUnuse = TcpRequestCode.LoginServer + 48,//是否收藏闲置
        CollectionUnuse = TcpRequestCode.LoginServer + 49,//操作收藏闲置
        GetMyCollectionUnuseList = TcpRequestCode.LoginServer + 50,//获取我收藏的闲置
        SearchUnuseList = TcpRequestCode.LoginServer + 51,//查找闲置列表


        //MetaSchool
        GetMyMetaSchoolData = TcpRequestCode.LoginServer + 32,//获取我的校园数据
        SetMyMetaSchoolData = TcpRequestCode.LoginServer + 33,//选择我的校园数据
    }
}
