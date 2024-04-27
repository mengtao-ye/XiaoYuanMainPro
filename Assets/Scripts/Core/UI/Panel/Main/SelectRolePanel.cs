using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class SelectRolePanel : BaseCustomPanel
    {
        private ToggleGroup mTG;
        public SelectRolePanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mTG = transform.FindObject<ToggleGroup>("RoleArea");
            transform.FindObject<Button>("CloseBtn").onClick.AddListener(() =>
            {
                GameCenter.Instance.ShowPanel<MainPanel>();
            });
            transform.FindObject<Button>("EnterBtn").onClick.AddListener(() =>
            {
                byte roleID = mTG.GetActiveToggle().name.ToByte();
                AppTools.UdpSend( SubServerType.Login,(short)LoginUdpCode.SetMyMetaSchoolData,ByteTools.Concat(AppVarData.Account.ToBytes(), roleID));
            });
            BoardCastModule.AddListener<byte[]>(BoardCastID.GetMetaSchoolData, MetaSchoolDataCallBack);
        }

        private void MetaSchoolDataCallBack(byte[] data)
        {
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.ShowPanel<SelectRolePanel>();
            }
            else
            {
                MyMetaSchoolData myMetaSchoolData = ConverterDataTools.ToPoolObject<MyMetaSchoolData>(data);
                MetaSchoolGlobalVarData.SetMyMetaSchoolData(myMetaSchoolData);
                GameCenter.Instance.LoadScene(SceneID.MetaSchoolScene, ABTagEnum.Main);
            }
        }
        public override void OnDestory()
        {
            base.OnDestory();
            BoardCastModule.RemoveListener<byte[]>(BoardCastID.GetMetaSchoolData, MetaSchoolDataCallBack);
        }

    }
}
