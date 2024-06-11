using UnityEngine;
using YFramework;

namespace Game
{
    public class PlayerChildModel : BaseCustomChildModel
    {
        private CharacterController mPlayerControlller;
        private Transform mPlayerRoot;
        private Transform mCameraRoot;
        private Transform mRoleRoot;
        private GameObject mRoleTarget;
        private float mMoveSpeed = 2f;
        private float mRotateSpeed = 0.1f;
        private float mUpRotateMax = 40;
        private float mUpRotateMin = -30;
        private float mUpRotateValue;
        private IFSM<RoleFSMData> mRoleFSM;
        public PlayerChildModel(IModel model, GameObject target) : base(model, target)
        {

        }
        public override void Start()
        {
            base.Start();
            ResourceHelper.AsyncLoadAsset<GameObject>("Prefabs/Models/MetaSchool/PlayerRoot", InstantiatePlayerRoot);
        }
        private void InstantiatePlayerRoot(GameObject playerRoot)
        {
            mPlayerRoot = playerRoot.InstantiateGameObject(transform).transform;
            mCameraRoot = mPlayerRoot.Find("CameraPos");
            mPlayerControlller = mPlayerRoot.GetComponent<CharacterController>();
            MultiPlayerGlobalData.playerTrans = mPlayerRoot;
            mUpRotateValue = mCameraRoot.transform.localEulerAngles.x;
            Camera mainCamera = mCameraRoot.Find("Camera").GetComponent<Camera>();
            MetaSchoolGlobalVarData.SetMainCamera(mainCamera);
            MetaSchoolMainPanel metaSchoolMainPanel = GameCenter.Instance.GetPanel<MetaSchoolMainPanel>();
            metaSchoolMainPanel.move.SetCallBack(Move);
            metaSchoolMainPanel.move.SetMoveSpeedBack(MoveSpeed);
            metaSchoolMainPanel.rotate.SetCallBack(Rotate);
            MetaSchoolDataMapper.Map(AppVarData.Account, MetaSchoolDataCallBack);
        }
        private void MetaSchoolDataCallBack(MetaSchoolMapperData metaSchoolMapperData) 
        {
            string roleName = metaSchoolMapperData.RoleID.ToString();
            LoadABRoleTools.LoadABRole(
                roleName,
                null,
                (error) =>
                {
                    LogHelper.LogError(error);
                },
                InitRole
                );
        }
        


        private void MoveSpeed(float value) 
        {
            if (mRoleFSM == null) return;
            mRoleFSM.condition .moveSpeed = value;
        }

        public override void Update()
        {
            base.Update();
            if (mRoleFSM!=null)
            {
                mRoleFSM.Update();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
#if UNITY_EDITOR
            float horizontal = 0;
            float vertical = 0;
            if (Input.GetKey(KeyCode.A))
            {
                horizontal = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                horizontal = 1;
            }
            if (Input.GetKey(KeyCode.W))
            {
                vertical = 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                vertical = -1;
            }
            Move(new Vector2(horizontal, vertical));
#endif
        }
        public override void OnDestory()
        {
            base.OnDestory();
            MultiPlayerGlobalData.playerTrans = null;
        }


        private void Move(Vector2 v2)
        {
            if (mPlayerRoot == null) return;
            mPlayerControlller.SimpleMove(mPlayerRoot.TransformDirection(new Vector3(v2.x * mMoveSpeed * 0.4f, 0, v2.y * mMoveSpeed))/*, Space.Self*/);
        }
        private void Rotate(Vector2 v2)
        {
            if (mPlayerRoot == null) return;
            mPlayerRoot.localEulerAngles += Vector3.up * v2.x * mRotateSpeed;
            mUpRotateValue -= v2.y * mRotateSpeed;
            mUpRotateValue = Mathf.Clamp(mUpRotateValue, mUpRotateMin, mUpRotateMax);
            mCameraRoot.localEulerAngles = Vector3.right * mUpRotateValue;
        }
        private void InitRole(GameObject role)
        {
            mRoleRoot = mPlayerRoot.transform.Find("Instantiate");
            mRoleTarget = role.InstantiateGameObject(mRoleRoot);
            mRoleTarget.transform.localPosition = Vector3.zero;
            mRoleTarget.transform.localRotation = Quaternion.identity;
            mRoleFSM = new RoleFSM();
            mRoleFSM.condition = new RoleFSMData() { moveSpeed = 0, animator = mRoleTarget.GetComponent<Animator>() };
            mRoleFSM.AddState(new RoleIdleState((int)RoleFSMStateID.Idle));
            mRoleFSM.AddState(new RoleWalkState((int)RoleFSMStateID.Walk));
            mRoleFSM.Performance((int)RoleFSMStateID.Idle);
        }
    }
}
