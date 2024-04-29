using UnityEngine;
using YFramework;

namespace Game
{
    public class EntityAnimationComponent : BaseECSComponent
    {
        public override int ComponentID => ECSComponentID.ENTITY_ANIMATION_ID;
        public IFSM<RoleFSMData> mRoleFSM;
        public void Init(GameObject role) 
        {
            mRoleFSM = new RoleFSM();
            mRoleFSM.condition = new RoleFSMData() { moveSpeed = 0, animator = role.GetComponent<Animator>() };
            mRoleFSM.AddState(new RoleIdleState((int)RoleFSMStateID.Idle));
            mRoleFSM.AddState(new RoleWalkState((int)RoleFSMStateID.Walk));
            mRoleFSM.Performance((int)RoleFSMStateID.Idle);
            entity.GetComponent<LerpPositionComponent>().roleFSM = mRoleFSM;
        }

        public override void Update()
        {
            if (mRoleFSM != null)
            {
                mRoleFSM.Update();
            }
        }
    }
}
