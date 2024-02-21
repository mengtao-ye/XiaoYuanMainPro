namespace Game
{
    public enum UdpRequestCode : short
    {
        Common = CommonConstData.REQUESTCODE_SPAN * 0,//共同数据相关请求  系统的，不要删除
        LockStep = CommonConstData.REQUESTCODE_SPAN * 1,//帧同步相关请求  系统的，不要删除
        SubServer = CommonConstData.REQUESTCODE_SPAN * 2,//分布式服务器  系统的，不要删除
        //这里的64是空出位置给系统用
        MainServer = CommonConstData.REQUESTCODE_SPAN * 65,//主服务器
        LoginServer = CommonConstData.REQUESTCODE_SPAN * 66,//登录服务器
    }
}
