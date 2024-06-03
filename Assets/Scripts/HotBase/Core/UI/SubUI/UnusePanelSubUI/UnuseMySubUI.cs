using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class UnuseMySubUI : BaseCustomSubUI
    {
        private Image mHead;
        private Text mName;
        private Image mBottomImage;
        private Text mBottomText;
        public UnuseMySubUI(Transform trans, Image image, Text text) : base(trans)
        {
            mBottomImage = image;
            mBottomText = text;
        }
        public override void Awake()
        {
            base.Awake();
            mHead = transform.FindObject<Image>("Head");
            mName = transform.FindObject<Text>("Name");
            transform.FindObject<Button>("MyApplicationBtn").onClick.AddListener(MyApplicationBtnListener);
            transform.FindObject<Button>("MyCollectionBtn").onClick.AddListener(MyCollectionBtnListener);
        }

        private void MyApplicationBtnListener()
        {
            GameCenter.Instance.ShowPanel<MyReleaseUnuseListPanel>();
        }
        private void MyCollectionBtnListener()
        {
            GameCenter.Instance.ShowPanel<MyCollectionUnuseListPanel>();
        }
        public override void Show()
        {
            base.Show();
            UserDataModule.MapUserData(AppVarData.Account, mHead, mName);
            mBottomImage.color = ColorConstData.BottomSelectColor;
            mBottomText.color = ColorConstData.BottomSelectColor;
        }
        public override void Hide()
        {
            base.Hide();
            mBottomImage.color = ColorConstData.BottomNormalColor;
            mBottomText.color = ColorConstData.BottomNormalColor;
        }
    }
}
