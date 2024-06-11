using YFramework;
using static YFramework.Utility;

namespace Game
{
    public static class SocketTools
    {
        /// <summary>
        /// 获取发送到服务器的数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetSendServerData(byte[] data) 
        {
            if (AppVarData.Token == 0) 
            {
                AppTools.ToastError("登录异常");
                return null;
            }
            return ByteTools.Concat(AppVarData.Token.ToBytes(), data);
        }

        public static SocketReturnMsg GetSocketData(byte[] data)
        {
            if (data.IsNullOrEmpty())
            {
                LogHelper.LogError("SocketTools.GetSocketData(byte[] data)传入的参数为空");
                return null;
            }
            SocketReturnMsg socketReturnMsg = ConverterDataTools.ToPoolObject<SocketReturnMsg>(data);
            if (socketReturnMsg.resultCode != (byte)SocketResultCode.Success)
            {
                AppTools.ToastError(socketReturnMsg.msg);
            }
            return socketReturnMsg;
        }
    }
}
