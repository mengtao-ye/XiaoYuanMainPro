namespace Game
{
    public enum LoginUdpCode: short
    {
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
    }
}
