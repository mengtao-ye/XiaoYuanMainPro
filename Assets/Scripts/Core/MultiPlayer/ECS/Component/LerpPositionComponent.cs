using UnityEngine;
using YFramework;

namespace Game
{
    public class LerpPositionComponent : BaseECSComponent
    {
        public override int ComponentID => ECSComponentID.LERP_POSITION_ID;
        private PositionComponent mPosition;
        private float mSpeed;
        public override void Awake()
        {
            base.Awake();
            mPosition = entity.GetComponent<PositionComponent>();
        }
        public override void Update()
        {
            float time = Mathf.Clamp( 0.1f-( Time.time - mPosition.mTimer),0.02f,0.1f);
            float dis = Vector3.Distance(entity.transform.position, mPosition.position);
            entity.transform.position = Vector3.MoveTowards(entity.transform.position,mPosition.position, time* dis );
        }
    }
}
