using UnityEngine;
using YFramework;

namespace Game
{
    public class MsgScrollViewItem : BaseScrollViewItem< MsgScrollViewItem>
    {
        public long id;
        public long account;
        public byte msg_type;
        public string chat_msg;
        public long time;
        public override Vector2 size { get; set; } = new Vector2(100,60);
        public bool isPop { get; set; }
        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
                
        }
        public void PopPool()
        {
        }

        public void PushPool()
        {
        }

        public virtual void Recycle()
        {
        }

        protected override IGameObjectPoolTarget PopTarget()
        {
            return null;
        }
    }
}
