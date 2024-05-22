using UnityEngine;
using YFramework;

namespace Game
{
    public class RoleIdleState : BaseFSMState<RoleFSMData>
    {
        public RoleIdleState(int stateID) : base(stateID)
        {

        }

        public override void Init()
        {

        }

        public override void Enter()
        {
          
        }

        public override void Check()
        {
       
            if (fsm.condition.moveSpeed > 0.01f) 
            {
                fsm.Performance((int)RoleFSMStateID.Walk);    
            }
        }
        public override void Stay()
        {
          
        }
        public override void Exit()
        {
           
        }
    }
}
