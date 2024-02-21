namespace Game
{
    public enum LoginUdpCode: short 
    {
        LoginHeartBeat = UdpRequestCode.LoginServer +1,//心跳包
        LoginAccount = UdpRequestCode.LoginServer +2,//账号登录
        RegisterAccount = UdpRequestCode.LoginServer + 3,//注册账号
        GetUserData = UdpRequestCode.LoginServer + 4,//获取用户数据
    }
}
