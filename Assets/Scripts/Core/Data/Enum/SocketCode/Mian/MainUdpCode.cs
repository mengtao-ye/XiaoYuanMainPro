namespace Game
{
    public enum MainUdpCode : short
    {
        //Common
        ServerBigDataResponse = UdpRequestCode.Common + 1,//服务器端大数据下标反馈
        ClientBigDataResponse = UdpRequestCode.Common + 2,//客户端大数据下标反馈

        //帧同步数据
        LockStep_ClientData = UdpRequestCode.LockStep + 1,//客户端帧同步数据
        LockStep_ServerData = UdpRequestCode.LockStep + 2,//服务器端帧同步数据

        //SubServer
        BaseSubServerRegister = UdpRequestCode.SubServer + 1,//基础分布式服务器注册
        BaseSubServerHeartBeat = UdpRequestCode.SubServer + 2,//基础分布式服务器心跳包

        //MainServer
        MainServerHeartBeat = UdpRequestCode.MainServer + 1,//主服务器心跳包
        GetBaseServerPoint = UdpRequestCode.MainServer + 2,//获取基础服务器point
    }
}
