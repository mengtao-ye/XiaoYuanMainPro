

using YFramework;

namespace Game
{
    public class UnuseTypeMapper : SingleMap<int, string, UnuseTypeMapper>
    {
        protected override void Config()
        {
            Add(1,"手机");
            Add(2, "男装");
            Add(3, "女装");
            Add(4, "零食");
            Add(5, "鞋包");
            Add(6, "百货");
            Add(7, "饰品");
            Add(8, "电脑");
            Add(9, "美妆");
            Add(10, "电器");
            Add(11, "其他");
        }
    }
}
