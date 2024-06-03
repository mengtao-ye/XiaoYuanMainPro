using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class FoundPanelMySubUI : BaseCustomSubUI
    {
        private Image mHeadImg;
        private Text mNameText;
        private Image mBottomImage;
        private Text mBottomText;
        public FoundPanelMySubUI(Transform trans, Image image, Text text) : base(trans)
        {
            mBottomImage = image;
            mBottomText = text;
        }
        public override void Awake()
        {
            base.Awake();
            mHeadImg = transform.FindObject<Image>("Head");
            mNameText = transform.FindObject<Text>("Name");
            transform.FindObject<Button>("MyFoundListBtn").onClick.AddListener(MyLostListBtnListener);
            transform.FindObject<Button>("PublishBtn").onClick.AddListener(() =>
            {
                GameCenter.Instance.ShowPanel<PublishFoundPanel>();
            });
        }

        public override void Show()
        {
            base.Show();
            UserDataModule.MapUserData(AppVarData.Account,mHeadImg,mNameText);
            mBottomImage.color = ColorConstData.BottomSelectColor;
            mBottomText.color = ColorConstData.BottomSelectColor;
        }
        public override void Hide()
        {
            base.Hide();
            mBottomImage.color = ColorConstData.BottomNormalColor;
            mBottomText.color = ColorConstData.BottomNormalColor;
        }

        private void MyLostListBtnListener()
        {
            GameCenter.Instance.ShowPanel<MyFoundListPanel>();
        }
    }
}
