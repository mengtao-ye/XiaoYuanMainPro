using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class LostPanelMySubUI : BaseCustomSubUI
    {
        private Image mHeadImg;
        private Text mNameText;
        private Image mBottomImage;
        private Text mBottomText;
        public LostPanelMySubUI(Transform trans, Image image, Text text) : base(trans)
        {
            mBottomImage = image;
            mBottomText = text;
        }
        public override void Awake()
        {
            base.Awake();
            mHeadImg = transform.FindObject<Image>("Head");
            mNameText = transform.FindObject<Text>("Name");
            transform.FindObject<Button>("MyLostListBtn").onClick.AddListener(MyLostListBtnListener);
            transform.FindObject<Button>("PublishBtn").onClick.AddListener(() =>
            {
                GameCenter.Instance.ShowPanel<PublishLostPanel>();
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
            GameCenter.Instance.ShowPanel<MyLostListPanel>();
        }
    }
}
