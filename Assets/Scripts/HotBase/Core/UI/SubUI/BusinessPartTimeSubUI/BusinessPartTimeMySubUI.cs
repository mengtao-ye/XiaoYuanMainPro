using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BusinessPartTimeMySubUI : BaseCustomSubUI
    {
        private Image mBottomImage;
        private Text mBottomText;
        public BusinessPartTimeMySubUI(Transform trans, Image image, Text text) : base(trans)
        {
            mBottomImage = image;
            mBottomText = text;
        }
        public override void Show()
        {
            base.Show();
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
