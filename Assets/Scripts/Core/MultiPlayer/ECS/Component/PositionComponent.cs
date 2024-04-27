using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class PositionComponent : BaseECSComponent
    {
        public override int ComponentID => ECSComponentID.POSITION_ID;
        public Vector3 position;
        private bool mIsFirstSetData;
        public float mTimer;
        public override byte[] GetData()
        {
            Vector3 pos = entity.transform.position * 1000;
            return ByteTools.ConcatParam(ComponentID.ToBytes(), ((int)pos.x).ToBytes(), ((int)pos.y).ToBytes(), ((int)pos.z).ToBytes());
        }
        public override void SetData(byte[] data)
        {
            int x = data.ToInt();
            int y = data.ToInt(4);
            int z = data.ToInt(8);
            mTimer = Time.time;
            position = new Vector3(x /1000f,y/1000f,z/1000f);
            if (!mIsFirstSetData) 
            {
                mIsFirstSetData = true;
                entity.transform.position = position;
            }
        }
    }
}
