using System.Collections;
using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class MultiPlayerChildController : BaseCustomChildController
    {
        private MultiPlayerSystem mMultiPlayerSystem;
        private Transform mClientParent;
        public MultiPlayerChildController(BaseController controller) : base(controller)
        {

        }

        public override void Awake()
        {
            base.Awake();
            mClientParent = new GameObject("ClientParent").transform;
            mMultiPlayerSystem = new MultiPlayerSystem();
            mMultiPlayerSystem.Awake();
            mMultiPlayerSystem.Start();
            IEnumeratorModule.StartCoroutine(DelayInitPlayerEntity());
        }
        private IEnumerator DelayInitPlayerEntity()
        {
            while (MultiPlayerGlobalData.playerTrans == null)
            {
                yield return Yielders.WaitForEndOfFrame;
            }
            mMultiPlayerSystem.AddEntity(new PlayerEntity(AppVarData.Account, MultiPlayerGlobalData.playerTrans.gameObject));
        }

        public override void Update()
        {
            base.Update();
            mMultiPlayerSystem.Update();
        }

        public void SetPlayerData(long account, byte[] data)
        {
            IECSEntity entity = mMultiPlayerSystem.FindEntity(account);
            if (entity == null)
            {
                entity = new ClientEntity(account, UnityTools.CreateGameObject(account.ToString(), mClientParent));
                mMultiPlayerSystem.AddEntity(entity);
            }   
            entity.SetData(data);
        }

        public override void OnDestory()
        {
            base.OnDestory();
            mMultiPlayerSystem.OnDestory();
        }
    }
}
