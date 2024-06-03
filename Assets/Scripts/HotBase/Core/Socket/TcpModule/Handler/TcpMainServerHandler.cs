using UnityEngine;
using YFramework;

namespace Game
{
    public class TcpMainServerHandler : BaseTcpRequestHandle
    {
        protected override short mRequestCode =>(short) TcpRequestCode.MainServer;
        protected override void ComfigActionCode()
        {
            Add((short)MainTcpCode.TestCode, TestCode);   
        }
        private void TestCode(byte[] data)
        {
            Debug.Log("接收到数据:"+data.ToStr());
        }
    }
}
