using System.Collections;
using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class RoleComponent : BaseECSComponent
    {
        public override int ComponentID => ECSComponentID.ROLE_ID;
        private int mGetMetaSchoolBoardCastID;
        private bool IsGetRoleID;
        private GameObject mRoleTarget;
        public override void Awake()
        {
            base.Awake();
            IsGetRoleID = false;
            mGetMetaSchoolBoardCastID = BoardCastID.GetUniqueID();
            BoardCastModule.AddListener<byte[]>(mGetMetaSchoolBoardCastID, GetMetaSchoolDataCallBack);
            IEnumeratorModule.StartCoroutine(IEGetRoleID());
        }

        private IEnumerator IEGetRoleID() 
        {
            while (!IsGetRoleID)
            {
                AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetMyMetaSchoolData, ByteTools.Concat(mGetMetaSchoolBoardCastID.ToBytes(), AppVarData.Account.ToBytes()));
                yield return Yielders.GetSeconds(0.2f);
            }
        }


        public override void OnDestory()
        {
            base.OnDestory();
            BoardCastModule.RemoveListener<byte[]>(mGetMetaSchoolBoardCastID, GetMetaSchoolDataCallBack);
        }

        private void GetMetaSchoolDataCallBack(byte[] data)
        {
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                AppTools.ToastNotify("玩家角色获取失败");
            }
            else
            {
                MyMetaSchoolData myMetaSchoolData = ConverterDataTools.ToPoolObject<MyMetaSchoolData>(data);
                if (myMetaSchoolData != null)
                {
                    if (IsGetRoleID) return;
                    IsGetRoleID = true;
                    int roleID = myMetaSchoolData.RoleID;
                    LoadABRoleTools.LoadABRole(roleID.ToString(),null,(error)=> { LogHelper.LogError(error); }, BuildRoleCallback);
                    myMetaSchoolData.Recycle();
                }
            }
        }
        private void BuildRoleCallback(GameObject rolePool)
        {
            if (rolePool != null)
            {
                mRoleTarget = rolePool.InstantiateGameObject(entity.transform) ;
                mRoleTarget.transform.localPosition = Vector3.zero;
                mRoleTarget.transform.localRotation = Quaternion.identity;
                EntityAnimationComponent component = new EntityAnimationComponent();
                entity.AddComponent(component);
                component.Init(mRoleTarget);
            }
        }

    }
}
