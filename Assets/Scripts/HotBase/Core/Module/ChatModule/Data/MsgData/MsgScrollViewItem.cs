using UnityEngine;
using YFramework;

namespace Game
{
    public class MsgScrollViewItem : BaseScrollViewItem
    {
        public long id;
        public long account;
        public byte msg_type;
        public string chat_msg;
        public long time;
        public override Vector2 size { get; set; } = new Vector2(100,60);
        public override float anchoredPositionX => 0;
        public override void LoadData(IGameObjectPoolTarget gameObjectPoolTarget)
        {
                
        }
        public override void Recycle()
        {
            ParentRecycle();
        }

        public virtual void ParentRecycle() { 
            
        }
        protected override void StartPopTarget()
        {
            VirtualStartPopTarget();
        }
        protected virtual void VirtualStartPopTarget() 
        { 
                
        }
        
    }
}
