using YFramework;

namespace Game
{
    public class SchoolMapperData : IDataMapper<long>
    {
        public bool hasData { get; private set; }
        public int schoolID;
        public string name;
        public long schoolCode;//学校编码
        public string assetBundleName;//ab包资源名称
        public void RequestData(long key)
        {
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetSchoolData, key.ToBytes());
        }

        public void SetData(byte[] data)
        {
            hasData = true;
            IListData<byte[]> list = data.ToListBytes();
            schoolID = list[0].ToInt();
            name = list[1].ToStr();
            schoolCode = list[2].ToLong();
            assetBundleName = list[3].ToStr();
            list.Recycle();
        }
    }
}
