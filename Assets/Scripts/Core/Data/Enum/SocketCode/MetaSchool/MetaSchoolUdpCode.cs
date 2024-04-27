namespace Game
{
    public enum MetaSchoolUdpCode : short
    {
        //Base
        MetaSchoolHeartBeat = UdpRequestCode.MetaSchoolServer + 1,//心跳包
        SendPlayerData = UdpRequestCode.MetaSchoolServer + 2,//发送玩家的数据
        SendOtherPlayerDataToSelf = UdpRequestCode.MetaSchoolServer + 3,//发送其他玩家的数据给自己

    }
}
