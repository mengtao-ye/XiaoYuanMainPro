namespace Game
{
    /// <summary>
    /// 记录映射
    /// </summary>
    public interface IDataMapper<TKey>
    {
        bool hasData { get; }
        void RequestData(TKey key);
        void SetData(byte[] data);
    }
}
