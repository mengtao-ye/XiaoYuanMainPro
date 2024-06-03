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
            mJoinSchoolBtn.gameObject.SetAvtiveExtend(false);
            mSchoolBG.gameObject.SetAvtiveExtend(false);
            transform.FindObject<Button>("EnterMetaSchoolBtn").onClick.AddListener(EnterMetaSchoolBtnListener);
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


        private void EnterMetaSchoolBtnListener() 
        {
            AppTools.TcpSend(TcpSubServerType.Login,(short)TcpLoginUdpCode.GetMyMetaSchoolData, ByteTools.Concat(BoardCastID.GetMetaSchoolData.ToBytes(),  AppVarData.Account.ToBytes()));
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
            mJoinSchoolBtn.gameObject.SetAvtiveExtend(schoolCode == 0);
            mSchoolBG.gameObject.SetAvtiveExtend(schoolCode != 0);
            if (schoolCode != 0)
            {
                //加入了学校
                AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetSchoolData, schoolCode.ToBytes());
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
