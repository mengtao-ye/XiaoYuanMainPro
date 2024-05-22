using YFramework;

namespace Game
{
    public class ShowMultiImageTipUI : BaseCustomTipsUI
    {
        private ShowMultiImageUI mShowMultiImageUI;
        protected override ShowAnimEnum ShowAnim => ShowAnimEnum.None;
        public ShowMultiImageTipUI()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mShowMultiImageUI = gameObject.AddComponent<ShowMultiImageUI>();
            mShowMultiImageUI.SetTipUI(this);
        }

        public void SetData(IListData<SelectImageData> imageList)
        {
            mShowMultiImageUI.SetData(imageList);
        }

        public override void Hide()
        {
            base.Hide();
            mShowMultiImageUI.Hide();
        }
    }
}
