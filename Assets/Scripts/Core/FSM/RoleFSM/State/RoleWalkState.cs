using UnityEngine;
using YFramework;

namespace Game
{
    public class RoleWalkState : BaseFSMState<RoleFSMData>
    {
        public RoleWalkState(int stateID) : base(stateID)
        {

        }

        public override void Init()
        {

        }

        public override void Enter()
        {
            fsm.condition.animator.SetBool("Walk",true);
        }

        public override void Check()
        {
            if (fsm.condition.moveSpeed < 0.01f)
            {
                fsm.Performance((int)RoleFSMStateID.Idle);    
            }
        }
        public override void Stay()
        {
        }
        public override void Exit()
        {
            fsm.condition.animator.SetBool("Walk",false);
        }
    }
}
