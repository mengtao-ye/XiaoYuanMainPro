using System.Collections.Generic;
using System.Text;
using YFramework;

namespace Game
{
    public class SkinData : IDataConverter
    {
        private IDictionaryData<byte, byte[]> mSkinDataDict;
        public IDictionary<byte, byte[]> skinDict { get { return mSkinDataDict.data; } }
        public SkinData()
        {
            mSkinDataDict =null;
            //mSkinDataDict = ClassPool<DictionaryData<byte, byte[]>>.Pop();
            //mSkinDataDict.Add(1, new byte[] { 1, 1 });
            //mSkinDataDict.Add(2, new byte[] { 1, 1 });
            //mSkinDataDict.Add(3, new byte[] { 0, 1 });
            //mSkinDataDict.Add(4, new byte[] { 0, 1 });
            //mSkinDataDict.Add(5, new byte[] { 0, 1 });
            //mSkinDataDict.Add(6, new byte[] { 0, 1 });
            //mSkinDataDict.Add(8, new byte[] { 0, 1 });
            //mSkinDataDict.Add(7, new byte[] { 1, 1 });
            //mSkinDataDict.Add(9, new byte[] { 0, 1 });
            //mSkinDataDict.Add(10, new byte[] { 1, 1 });
            //mSkinDataDict.Add(11, new byte[] { 1, 1 });
            //mSkinDataDict.Add(13, new byte[] { 1, 1 });
        }
        public byte[] GetData(byte type) 
        {
            if (mSkinDataDict.ContainsKey(type))
            {
                return mSkinDataDict[type];
            }
            return null;
        }

        public byte[] ToBytes()
        {
            if (mSkinDataDict == null ||  mSkinDataDict.data.IsNullOrEmpty()) return null;
            return mSkinDataDict.ToBytes();
        }

        public void ToValue(byte[] data)
        {
            mSkinDataDict =  data.ToBytesDictionary();
        }
        //public override string ToString()
        //{
        //    if (mSkinDataDict == null || mSkinDataDict.data.IsNullOrEmpty()) return "";
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var item in mSkinDataDict.data)
        //    {
        //        sb.Append(" Key:"+item.Key);
        //        sb.Append(" Value"+ item.Value.Length);
        //    }
        //    return sb.ToString() ;
        //}
    }
}
