using UnityEngine;
using YFramework;

namespace Game
{
    public class CheckUdpServerIsInitProcess : BaseProcess
    {
        private float mTimer;
        private float mTime=1;
        public override void Enter()
        {
            mTimer = 1;
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            mTimer += Time.deltaTime;
            if (mTimer > mTime)
            {
                mTimer = 0;
                if (GameCenter.Instance.CenterUdpServerIsConnect)
                {
                    DoNext();
                }
            }
        }
    }
}
