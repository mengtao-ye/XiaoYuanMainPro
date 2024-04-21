using System.Collections.Generic;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public static class SelectImageDataTools
    {
        public static byte[] GetBytes(IList<SelectImageData> list )
        {
            if (list.IsNullOrEmpty())
            {
                return null;
            }
            IListData<byte[]> byteList = ClassPool<ListData<byte[]>>.Pop();
            for (int i = 0; i < list.Count; i++)
            {
                byteList.Add(list[i].ToBytes());
            }
            byte[] returnBytes = ByteTools.Concat(byteList.list);
            byteList.Recycle();
            return returnBytes;
        }

        public static IListData<SelectImageData> GetData(byte[] data)
        {
            if (data.IsNullOrEmpty())
            {
                return null;
            }
            int count = data.Length / SelectImageData.LEN;
            IListData<SelectImageData> list = ClassPool<ListPoolData<SelectImageData>>.Pop();
            for (int i = 0; i < count; i++)
            {
                list.Add(ConverterDataTools.ToPoolObject<SelectImageData>(data,i * SelectImageData.LEN));
            }
            return list;
        }

    }
}
