using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class SelectSchoolPanel : BaseCustomPanel
    {
        private InputField mSchoolNameIF;
        private IScrollView mScrollView;
        public SelectSchoolPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mScrollView = transform.FindObject("SchoolView").AddComponent<RecyclePoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10,10,10);

            mSchoolNameIF = transform.FindObject<InputField>("SchoolNameIF");
            transform.FindObject<Button>("SearchSchoolBtn").onClick.AddListener(SearchBtnListener);
            transform.FindObject<Button>("BackBtn").onClick.AddListener(()=> { GameCenter.Instance.ShowPanel<MainPanel>(); });
        }
        public override void Hide()
        {
            base.Hide();
            mScrollView.ClearItems();
            mSchoolNameIF.text = string.Empty;
        }

        private void SearchBtnListener()
        {
            if (mSchoolNameIF.text.IsNullOrEmpty())
            {
                AppTools.Toast("请输入学校名称");
                return;
            }
            mScrollView.ClearItems();
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.SearchSchool, mSchoolNameIF.text.ToBytes());
        }
        public void ShowSchoolItem(IListData<SchoolData> datas)
        {
            for (int i = 0; i < datas.list.Count; i++)
            {
                SearchSchoolScrollViewItem searchSchoolScrollViewItem = ClassPool<SearchSchoolScrollViewItem>.Pop();
                searchSchoolScrollViewItem.schoolData = datas.list[i];
                mScrollView.Add(searchSchoolScrollViewItem);
            }
        }
    }
}
