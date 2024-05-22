using UnityEngine;
using YFramework;

namespace Game
{
    public class LerpRotationComponent : BaseECSComponent
    {
        public override int ComponentID => ECSComponentID.LERP_ROTATE_ID;
        private RotateionComponent mRotation;
        public override void Awake()
        {
            base.Awake();
            mRotation = entity.GetComponent<RotateionComponent>();
        }
        public override void Update()
        {
            entity.transform.rotation = Quaternion.Lerp(entity.transform.rotation, mRotation.rotation,0.3f);
        }
    }
}
