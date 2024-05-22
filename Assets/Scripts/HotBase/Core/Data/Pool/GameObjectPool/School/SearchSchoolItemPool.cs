using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class SearchSchoolItemPool : BaseGameObjectPoolTarget<SearchSchoolItemPool>
    {
        public override string assetPath => "Prefabs/UI/Item/School/SearchSchoolItem";
        public override bool isUI { get; } = true;
        private Image mIcon;
        private Text mName;
        private SchoolData mCurSchoolData;
        public override void Init(GameObject target)
        {
            base.Init(target);
            mIcon = target.transform.FindObject<Image>("Icon");
            mName =  target.transform.FindObject<Text>("Name");
            target.transform.FindObject<Button>("JoinBtn").onClick.AddListener(JoinBtnListener);
        }
        private void JoinBtnListener()
        {
            if (mCurSchoolData != null)
            {
                GameCenter.Instance.ShowTipsUI<CommonTwoTipsUI>((ui)=> 
                {
                    ui.SetType(CommonTwoTipID.JoinSchool);
                    ui.ShowContent("是否加入学校:" + mCurSchoolData.name,"加入学校","取消",null,"加入",()=> {
                        byte[] datas = ByteTools.Concat(AppVarData.Account.ToBytes(), mCurSchoolData.schoolCode.ToBytes());
                        AppTools.UdpSend( SubServerType.Login,(short)LoginUdpCode.JoinSchool, datas);
                    });
                });
            }
        }

        public void SetData(SchoolData data) 
        {
            mCurSchoolData = data;
            mName.text = data.name;
            string iconUrl = OssPathData.GetSchoolIcon(data.schoolCode);
            HttpTools.LoadSprite(iconUrl, GetIconCallback);
        }
        private void GetIconCallback(Sprite sprite) 
        {
            mIcon.sprite = sprite;
        }

        public override void Recycle()
        {
            GameObjectPoolModule.Push(this);
        }
    }
}
