using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{

    public class UnusePanel : BaseCustomPanel
    {
        public UnuseListSubUI UnuseListSubUI { get; private set; }
        public UnuseMySubUI UnuseMySubUI { get; private set; }
        private ISubUI mCur;

        public UnusePanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            Transform typeContent = transform.Find("TypeContent");
            UnuseListSubUI = new UnuseListSubUI(typeContent.Find("UnuseList"));
            UnuseMySubUI = new UnuseMySubUI(typeContent.Find("My"));
            AddSubUI(UnuseListSubUI);
            AddSubUI(UnuseMySubUI);
            mCur = UnuseListSubUI;
            mCur.Show();
            UnuseMySubUI.Hide();
            Transform type = transform.Find("Type");
            type.FindObject<Button>("UnuseListBtn").onClick.AddListener(UnuseListBtnListener);
            type.FindObject<Button>("MyBtn").onClick.AddListener(MyBtnListener);
            type.FindObject<Button>("ReleaseBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<ReleaseUnusePanel>(); });

            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<MainPanel>(); });
        }
        private void UnuseListBtnListener()
        {
            if (mCur.uiName == UnuseListSubUI.uiName) return;
            mCur.Hide();
            mCur = UnuseListSubUI;
            mCur.Show();
        }
        private void MyBtnListener()
        {
            if (mCur.uiName == UnuseMySubUI.uiName) return;
            mCur.Hide();
            mCur = UnuseMySubUI;
            mCur.Show();
        }
    }
}
