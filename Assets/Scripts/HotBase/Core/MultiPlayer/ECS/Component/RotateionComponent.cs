using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class RotateionComponent : BaseECSComponent
    {
        public override int ComponentID => ECSComponentID.ROTATEION_ID;
        public Quaternion rotation;
        public override byte[] GetData()
        {
            int x = (int) (entity.transform.rotation.x * 1000);
            int y = (int) (entity.transform.rotation.y * 1000);
            int z = (int) (entity.transform.rotation.z * 1000);
            int w = (int) (entity.transform.rotation.w * 1000);
            return ByteTools.ConcatParam(ComponentID.ToBytes(), x.ToBytes(), y.ToBytes(), z.ToBytes(), w.ToBytes());
        }
        public override void SetData(byte[] data)
        {
            int x = data.ToInt();
            int y = data.ToInt(4);
            int z = data.ToInt(8);
            int w = data.ToInt(12);
            rotation = new Quaternion(x / 1000f, y / 1000f, z / 1000f, w / 1000f);
        }
    }
}
