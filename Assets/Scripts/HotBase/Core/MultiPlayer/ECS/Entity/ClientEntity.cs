using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class ClientEntity : BaseECSEntity
    {
        private float mLastTime;
        private float mTimer;
        public ClientEntity(long id, GameObject target) : base(id, target)
        {
             
        }

        public override void Awake()
        {
            base.Awake();
            AddComponent(new RoleComponent());
            AddComponent(new PositionComponent());
            AddComponent(new RotateionComponent());
            AddComponent(new LerpPositionComponent());
            AddComponent(new LerpRotationComponent());
        }

        public override void Update()
        {
            base.Update();
            mTimer += Time.deltaTime;
            if (mTimer > 1) {
                mTimer = 0;
                if (Time.time - mLastTime > 3)
                {
                    //超过3秒没收到消息表示掉线了
                    system.RemoveEntity(this);
                    GameObject.Destroy(gameObject);
                }
            }
         
        }

        public override void SetData(byte[] data)
        {
            mLastTime = Time.time;
            if (data.IsNullOrEmpty()) return;
            IListData<byte[]> list = data.ToListBytes();
            for (int i = 0; i < list.Count; i++)
            {
                int componentID = list[i].ToInt();
                byte[] byteDatas = ByteTools.SubBytes(list[i],4);
                GetComponent(componentID)?.SetData(byteDatas) ;
            }
        }
    }
}
