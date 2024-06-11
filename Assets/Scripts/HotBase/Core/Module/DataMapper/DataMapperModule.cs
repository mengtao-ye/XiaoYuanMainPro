using System;
using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// 数据映射模块
    /// </summary>
    public  class DataMapperModule<TKey,TValue> where TValue : IDataMapper<TKey> ,new()
    {
        private static IDictionary<TKey, TValue> mDataMapperDict = new Dictionary<TKey, TValue>();
        private static IDictionary<TKey, Action<TValue>> mActionDict = new Dictionary<TKey, Action<TValue>>();
        public static TValue Get(TKey account)
        {
            return mDataMapperDict[account];
        }
        /// <summary>
        /// 映射信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="call"></param>
        public static void Map(TKey key, Action<TValue> call)
        {
            if (mDataMapperDict.ContainsKey(key))
            {
                TValue data = mDataMapperDict[key];
                if (data == null)
                {
                    SendRequest(key);
                    return;
                }
                if (!data.hasData)
                {
                    SendRequest(key);
                    return;
                }
                call?.Invoke(data);
            }
            else
            {
                if (!mActionDict.ContainsKey(key))
                {
                    mActionDict.Add(key, call);
                }
                else 
                {
                    mActionDict[key] += call;
                }
                SendRequest(key);
            }
        }
        private static void SendRequest(TKey key)
        {
            TValue callBack = new TValue();
            mDataMapperDict.Add(key, callBack);
            callBack.RequestData(key);
        }
        public static void ReceiveData(TKey key, byte[] value)
        {
            if (mDataMapperDict.ContainsKey(key))
            {
                TValue data = mDataMapperDict[key];
                data.SetData(value);
                if (mActionDict.ContainsKey(key))
                {
                    mActionDict[key].Invoke(data);
                    mActionDict.Remove(key);
                }
            }
        }
        public static void Remove(TKey key)
        {
            if (mDataMapperDict.ContainsKey(key)) mDataMapperDict.Remove(key);
            if (mActionDict.ContainsKey(key)) mActionDict.Remove(key);
        }
    }
}
