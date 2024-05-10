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
                int roleID = mTG.GetActiveToggle().name.ToInt();
                AppTools.UdpSend( SubServerType.Login,(short)LoginUdpCode.SetMyMetaSchoolData,ByteTools.Concat(AppVarData.Account.ToBytes(), roleID.ToBytes()));
            });
            
        }


    }
}
