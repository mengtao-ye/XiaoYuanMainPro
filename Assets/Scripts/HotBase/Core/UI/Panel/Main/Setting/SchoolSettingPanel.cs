using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class SchoolSettingPanel : BaseCustomPanel
    {
        private Image mSchoolHead;
        private Text mSchoolNameText;
        private GameObject mScrollView;
        private GameObject mJoinSchoolBtn;
        public SchoolSettingPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mScrollView = transform.FindObject("ScrollView");
            mJoinSchoolBtn = transform.FindObject("JoinSchoolBtn");
            mJoinSchoolBtn.GetComponent<Button>().onClick.AddListener(JoinSchoolBtnListener);
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { mUICanvas.CloseTopPanel(); });
            mSchoolHead = transform.FindObject<Image>("SchoolHeadImg");
            mSchoolNameText = transform.FindObject<Text>("SchoolName");
            transform.FindObject<Button>("ExitSchoolBtn").onClick.AddListener(ExitSchoolBtnListener);
        }

        public override void Show()
        {
            base.Show();
            if (SchoolGlobalVarData.SchoolCode == 0)
            {
                IsJoinSchool(false);
            }
            else 
            { 
                IsJoinSchool(true);
                SchoolDataMapper.Map(SchoolGlobalVarData.SchoolCode, SchoolDataMapperCallback);
            }
        }

        private void SchoolDataMapperCallback(SchoolMapperData schoolMapperData) {
            string ossPath = OssPathData.GetSchoolIcon(schoolMapperData.schoolCode);
            HttpTools.LoadSprite(ossPath,(sp)=>
            {
                mSchoolHead.sprite = sp;
            });
            mSchoolNameText.text = schoolMapperData.name;
        }

        private void ExitSchoolBtnListener()
        {
            GameCenter.Instance.ShowTipsUI<CommonTwoTipsUI>((ui)=> {
                ui.ShowContent("是否退出该学校?","退出学校","取消",null,"确认", SureExitSchoolCallback);
            });
        }

        private void SureExitSchoolCallback() {
            GameCenter.Instance.HideTipsUI<CommonTwoTipsUI>();
            byte[] sendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), SchoolGlobalVarData.SchoolCode.ToBytes());
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.ExitSchool, sendBytes);
        }

        private void JoinSchoolBtnListener() 
        {
            GameCenter.Instance.ShowPanel<SelectSchoolPanel>();
        }
        private void  IsJoinSchool(bool isJoin)
        {
            mScrollView.SetActiveExtend(isJoin);
            mJoinSchoolBtn.SetActiveExtend(!isJoin);
        }
        public void ExitSchool() 
        {
            IsJoinSchool(false);
        }
    }
}
