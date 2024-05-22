using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class PlayerEntity : BaseECSEntity
    {
        private float mTimer;
        private float mTime = 0.1f;
        private PositionComponent mPosComponent;
        private RotateionComponent mRotationComponent;
        public PlayerEntity(long id, GameObject target) : base(id, target)
        {

        }

        public override void Awake()
        {
            base.Awake();
            mPosComponent = new PositionComponent();
            mRotationComponent = new RotateionComponent();
            AddComponent(mPosComponent);
            AddComponent(mRotationComponent);
        }

        public override void SetData(byte[] data)
        {
            
        }
        public override void Update()
        {
            base.Update();
            mTimer += Time.deltaTime;
            if (mTimer > mTime) 
            {
                mTimer = 0;
                IListData<byte[]> list = ClassPool<ListData<byte[]>>.Pop();
                list.Add(mPosComponent.GetData());
                list.Add(mRotationComponent.GetData());
                byte[] bytes = list.list.ToBytes();
                list.Recycle();
                AppTools.UdpSend( SubServerType.MetaSchool,(short)MetaSchoolUdpCode.SendPlayerData,ByteTools.ConcatParam(AppVarData.Account.ToBytes(),SchoolGlobalVarData.SchoolCode.ToBytes(),bytes));
            }
        }
    }
}
