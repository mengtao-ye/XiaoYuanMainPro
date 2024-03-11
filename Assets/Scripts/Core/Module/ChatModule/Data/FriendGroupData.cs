using YFramework;

namespace Game
{
    public class FriendGroupData : IPool
    {
        public bool isPop { get ; set ; }
        public void PopPool()
        {
        }
        public void PushPool()
        {
        }
        public void Recycle()
        {
            ClassPool<FriendGroupData>.Push(this);
        }
    }
}
