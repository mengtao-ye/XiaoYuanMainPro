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
        private Image mHeadImg;
        private Text mName;
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
            mHeadImg = transform.FindObject<Image>("HeadImg");
            mName = transform.FindObject<Text>("Name");
            mSchoolBG = transform.FindObject<Image>("SchoolBG");
            mSchoolName = transform.FindObject<Text>("SchoolName");
            mJoinSchoolBtn = transform.FindObject<Button>("JoinSchoolBtn");
            mJoinSchoolBtn.onClick.AddListener(JoinSchoolBtnListener);
            mJoinSchoolBtn.gameObject.SetAvtiveExtend(false);
            mSchoolBG.gameObject.SetAvtiveExtend(false);
            transform.FindObject<Button>("EnterMetaSchoolBtn").onClick.AddListener(EnterMetaSchoolBtnListener);
        }

        private void EnterMetaSchoolBtnListener() 
        {
            AppTools.UdpSend( SubServerType.Login,(short)LoginUdpCode.GetMyMetaSchoolData, ByteTools.Concat(BoardCastID.GetMetaSchoolData.ToBytes(),  AppVarData.Account.ToBytes()));
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
            UserDataModule.MapUserData(AppVarData.Account, mHeadImg, mName);
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetMySchool, AppVarData.Account.ToBytes());
        }
        public override void Refresh()
        {
            base.Refresh();
            UserDataModule.MapUserData(AppVarData.Account, mHeadImg, mName);
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetMySchool, AppVarData.Account.ToBytes());
        }
        public void SetMySchoolID(long schoolCode)
        {
            mJoinSchoolBtn.gameObject.SetAvtiveExtend(schoolCode == 0);
            mSchoolBG.gameObject.SetAvtiveExtend(schoolCode != 0);
            if (schoolCode != 0)
            {
                //加入了学校
                AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetSchoolData, schoolCode.ToBytes());
            }
        }
        public void SetSchoolData(SchoolData data)
        {
            mJoinSchoolBtn.gameObject.SetAvtiveExtend(false);
            mSchoolBG.gameObject.SetAvtiveExtend(true);
            string bgUrl =  OssPathData.GetSchoolBG(data.schoolCode);
            HttpTools.LoadSprite(bgUrl, LoadSchoolBGCallBack);
            mSchoolName.text = data.name;
        }

        private void LoadSchoolBGCallBack(Sprite sprite ) {
            mSchoolBG.sprite = sprite;
        }

    }
}
