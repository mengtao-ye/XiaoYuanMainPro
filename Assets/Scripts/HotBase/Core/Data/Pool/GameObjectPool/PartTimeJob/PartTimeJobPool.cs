using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class PartTimeJobPool : BaseGameObjectPoolTarget<PartTimeJobPool>
    {
        public override string assetPath => "Prefabs/UI/Item/PartTimeJob/PartTimeJobItem";
        public override bool isUI => true;
        private Text mTitleText;
        private Text mTimeText;
        private Text mPositionText;
        private Text mPriceText;
        private string mDetail;
        private string mTitle;
        private string mTime;
        private string mPosition;
        private int mPrice;
        private byte mPriceType;
        private bool mIsMyApplication;//是否是我的报名列表
        public override void Init(GameObject target)
        {
            base.Init(target);
            mTitleText = transform.FindObject<Text>("Title");
            mTimeText = transform.FindObject<Text>("Time");
            mPositionText = transform.FindObject<Text>("Position");
            mPriceText = transform.FindObject<Text>("Price");
            transform.GetComponent<Button>().onClick.AddListener(ClickBtnListener);
        }

        private void ClickBtnListener()
        {
            if (ID == 0)
            {
                AppTools.ToastError("未找到该兼职对象");
                return;
            }
            GameCenter.Instance.ShowPanel<PartTimeJobDetailPanel>((ui) =>
            {
                ui.SetData(mTitle, mPrice, mPriceType, mTime, mPosition, mDetail, (int)ID, mIsMyApplication);
            });
        }
        public void SetData(string title, string time, string pos, int price, byte priceType, string detail, bool isMyApplication)
        {
            mIsMyApplication = isMyApplication;
            mTitle = title;
            mTime = time;
            mPosition = pos;
            mPrice = price;
            mPriceType = priceType;
            mDetail = detail;
            mTitleText.text = title;
            mTimeText.text = time;
            mPositionText.text = pos;
            mPriceText.text = price + "/" + PartTimeJobTools.GetPriceType(priceType);

        }
        public override void Recycle()
        {
            GameObjectPoolModule.Push(this);
        }
    }
}
