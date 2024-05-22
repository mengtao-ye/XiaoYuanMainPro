namespace Game
{

    public enum LoginUdpCode: short
    {
        //目前到了  35

        //Base
        LoginHeartBeat = UdpRequestCode.LoginServer +1,//心跳包
        LoginAccount = UdpRequestCode.LoginServer +2,//账号登录
        RegisterAccount = UdpRequestCode.LoginServer + 3,//注册账号
        GetUserData = UdpRequestCode.LoginServer + 4,//获取用户数据
        GetMySchool = UdpRequestCode.LoginServer + 5,//获取我的学校
        GetSchoolData = UdpRequestCode.LoginServer + 6,//获取学校数据
        SearchSchool = UdpRequestCode.LoginServer + 7,//查找学校    
        JoinSchool = UdpRequestCode.LoginServer + 8,//加入学校
        //Chat
        GetNewChatMsg = UdpRequestCode.LoginServer + 9,//获取最新的聊天信息
        SendChatMsg = UdpRequestCode.LoginServer + 10,//发送聊天消息
        GetFriendList = UdpRequestCode.LoginServer + 11,//获取好友列表
        SearchFriendData = UdpRequestCode.LoginServer + 12,//查找好友
        SendAddFriendRequest = UdpRequestCode.LoginServer + 13,//发送添加好友请求
        GetAddFriendRequest = UdpRequestCode.LoginServer + 14,//获取好友申请请求
        RefuseFriend = UdpRequestCode.LoginServer + 15,//拒绝好友申请
        ConfineFriend = UdpRequestCode.LoginServer + 16,//同意好友申请

        //CampusCircle
        PublishCampusCircle = UdpRequestCode.LoginServer + 17,//发表校友圈
        GetCampusCircle = UdpRequestCode.LoginServer + 18,//获取朋友圈
        GetCampusCircleItemDetail = UdpRequestCode.LoginServer + 19,//获取朋友圈对象详情 
        LikeCampusCircleItem = UdpRequestCode.LoginServer + 20,//朋友圈点赞 
        HasLikeCampusCircleItem = UdpRequestCode.LoginServer + 21,//是否朋友圈点赞 
        GetCommit = UdpRequestCode.LoginServer + 22,//获取评论信息

        //LostAndFound
        PublishLostData = UdpRequestCode.LoginServer + 23,//发表失物招领
        GetMyLostList = UdpRequestCode.LoginServer + 24,//获取我的失物招领
        GetLostList = UdpRequestCode.LoginServer + 34,//获取我的失物招领
        SearchLostList = UdpRequestCode.LoginServer + 35,//查找失物列表


        //PartTimeJob
        ReleasePartTimeJob = UdpRequestCode.LoginServer + 25,//发布兼职
        GetMyReleasePartTimeJob = UdpRequestCode.LoginServer + 26,//获取我发布的兼职
        GetPartTimeJobList = UdpRequestCode.LoginServer + 27,//获取兼职列表
        ApplicationPartTimeJob = UdpRequestCode.LoginServer + 28,//报名兼职
        GetApplicationPartTimeJob = UdpRequestCode.LoginServer + 29,//获取报名兼职列表

        //Unuse
        ReleaseUnuse = UdpRequestCode.LoginServer + 30,//发布闲置
        GetUnuseList = UdpRequestCode.LoginServer + 31,//获取闲置列表


        //MetaSchool
        GetMyMetaSchoolData = UdpRequestCode.LoginServer + 32,//获取我的校园数据
        SetMyMetaSchoolData = UdpRequestCode.LoginServer + 33,//选择我的校园数据
    }
}
