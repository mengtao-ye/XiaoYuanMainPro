using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class MainPageSubUI : BaseCustomSubUI
    {
        private MainPanel mMainPanel;
        private Image mSchoolBG;
        private Text mSchoolName;
        private Button mJoinSchoolBtn;

        public MainPageSubUI(Transform trans, MainPanel mainPanel) : base(trans)
        {
            mMainPanel = mainPanel;
        }
        public override void Awake()
        {
            base.Awake();
            mSchoolBG = transform.FindObject<Image>("SchoolBG");
            mSchoolName = transform.FindObject<Text>("SchoolName");
            mJoinSchoolBtn = transform.FindObject<Button>("JoinSchoolBtn");
            mJoinSchoolBtn.onClick.AddListener(JoinSchoolBtnListener);
            mJoinSchoolBtn.gameObject.SetActiveExtend(false);
            mSchoolBG.gameObject.SetActiveExtend(false);
            transform.FindObject<Button>("EnterMetaSchoolBtn").onClick.AddListener(EnterMetaSchoolBtnListener);
        }
        private void EnterMetaSchoolBtnListener()
        {
            MetaSchoolDataMapper.Map(AppVarData.Account, MetaSchoolDataMapperCallBack);
        }

        private void MetaSchoolDataMapperCallBack(MetaSchoolMapperData schoolMapperData)
        {
            if (schoolMapperData.RoleID == 0)
            {
                GameCenter.Instance.ShowPanel<SelectRolePanel>();
            }
            else
            {
                GameCenter.Instance.LoadScene(SceneID.MetaSchoolScene, ABTagEnum.Main);
            }
        }

        /// <summary>
        /// 加入学校按钮
        /// </summary>
        private void JoinSchoolBtnListener()
        {
            GameCenter.Instance.ShowPanel<SelectSchoolPanel>();
        }

        public override void Show()
        {
            base.Show();
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetMySchool, AppVarData.Account.ToBytes());
        }
        public override void Refresh()
        {
            base.Refresh();
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetMySchool, AppVarData.Account.ToBytes());
        }
        public void SetMySchoolID(long schoolCode)
        {
            mJoinSchoolBtn.gameObject.SetActiveExtend(schoolCode == 0);
            mSchoolBG.gameObject.SetActiveExtend(schoolCode != 0);
            if (schoolCode != 0)
            {
                //加入了学校
                SchoolDataMapper.Map(schoolCode, SchoolDataCallback);
            }
        }

        private void SchoolDataCallback(SchoolMapperData data)
        {
            mJoinSchoolBtn.gameObject.SetActiveExtend(false);
            mSchoolBG.gameObject.SetActiveExtend(true);
            string bgUrl = OssPathData.GetSchoolBG(data.schoolCode);
            HttpTools.LoadSprite(bgUrl, LoadSchoolBGCallBack);
            mSchoolName.text = data.name;
        }
        private void LoadSchoolBGCallBack(Sprite sprite)
        {
            mSchoolBG.sprite = sprite;
        }
    }
}
