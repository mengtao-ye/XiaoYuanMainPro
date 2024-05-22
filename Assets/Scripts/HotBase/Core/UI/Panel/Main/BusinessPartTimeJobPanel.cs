using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{

    public class BusinessPartTimeJobPanel : BaseCustomPanel
    {
        public RecruitSubUI RecruitSubUI { get; private set; }
        public BusinessPartTimeMySubUI BusinessPartTimeMySubUI { get; private set; }
        private ISubUI mCur;
        
        public BusinessPartTimeJobPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            Transform typeContent = transform.Find("TypeContent");
            RecruitSubUI = new RecruitSubUI(typeContent.Find("Recruit"));
            BusinessPartTimeMySubUI = new BusinessPartTimeMySubUI(typeContent.Find("My"));
            AddSubUI(RecruitSubUI);
            AddSubUI(BusinessPartTimeMySubUI);
            mCur = RecruitSubUI;
            mCur.Show();
            BusinessPartTimeMySubUI.Hide();
            Transform type = transform.Find("Type");
            type.FindObject<Button>("RecruitBtn").onClick.AddListener(RecruitBtnListener);
            type.FindObject<Button>("MyBtn").onClick.AddListener(MyBtnListener);
            type.FindObject<Button>("ReleaseBtn").onClick.AddListener(()=> { GameCenter.Instance.ShowPanel<ReleasePartTimeJobPanel>(); });

            transform.FindObject<Button>("BackBtn").onClick.AddListener(()=> { GameCenter.Instance.ShowPanel<PartTimeJobPanel>(); });
        }
        private void RecruitBtnListener()
        {
            if (mCur.uiName == RecruitSubUI.uiName) return;
            mCur.Hide();
            mCur = RecruitSubUI;
            mCur.Show();
        }
        private void MyBtnListener()
        {
            if (mCur.uiName == BusinessPartTimeMySubUI.uiName) return;
            mCur.Hide();
            mCur = BusinessPartTimeMySubUI;
            mCur.Show();
        }
    }
}
