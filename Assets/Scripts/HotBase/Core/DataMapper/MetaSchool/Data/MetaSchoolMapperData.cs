using YFramework;

namespace Game
{
    public class MetaSchoolMapperData : IDataMapper<long>
    {
        public bool hasData { get; private set; }
        public long Account;//账号信息
        public int RoleID;//角色信息
        public void RequestData(long key)
        {
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetMyMetaSchoolData, key.ToBytes());
        }

        public void SetData(byte[] data)
        {
            hasData = true;
            IListData<byte[]> bytes = data.ToListBytes();
            Account = bytes[0].ToLong();
            RoleID = bytes[1].ToInt();
            bytes.Recycle();
        }
    }
}
