using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class SelectSchoolPanel : BaseCustomPanel
    {
        private InputField mSchoolNameIF;
        private Transform mContent;
        public SelectSchoolPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mSchoolNameIF = transform.FindObject<InputField>("SchoolNameIF");
            mContent = transform.FindObject<Transform>("Content");
            transform.FindObject<Button>("SearchSchoolBtn").onClick.AddListener(SearchBtnListener);
            transform.FindObject<Button>("BackBtn").onClick.AddListener(()=> { GameCenter.Instance.ShowPanel<MainPanel>(); });
        }
        private void SearchBtnListener()
        {
            GameObjectPoolModule.PushTarget((int)GameObjectPoolID.SchoolItem);
            if (mSchoolNameIF.text.IsNullOrEmpty())
            {
                AppTools.Toast("请输入学校名称");
                return;
            }
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.SearchSchool, mSchoolNameIF.text.ToBytes());
        }
        public void ShowSchoolItem(IListData<SchoolData> datas)
        {
            for (int i = 0; i < datas.list.Count; i++)
            {
                SchoolData schoolData = datas.list[i];
                GameObjectPoolModule.AsyncPop<SchoolItemPool>((int)GameObjectPoolID.SchoolItem,mContent,(value)=> {
                    value.SetData(schoolData);
                });
            }
        }
    }
}
