using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class ReleasePartTimeJobPanel : BaseCustomPanel
    {
        private InputField mTitleIF;
        private InputField mPriceIF;
        private Dropdown mProceType;
        private InputField mJobTimeIF;
        private InputField mPositionIF;
        private InputField mDetailIF;
        public ReleasePartTimeJobPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            NormalVerticalScrollView normalScrollView = transform.FindObject("ScrollView").AddComponent<NormalVerticalScrollView>();
            normalScrollView.Init();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<BusinessPartTimeJobPanel>(); });
            mTitleIF = transform.FindObject<InputField>("TitleIF");
            mPriceIF = transform.FindObject<InputField>("PriceIF");
            mProceType = transform.FindObject<Dropdown>("PriceType");
            mJobTimeIF = transform.FindObject<InputField>("JobTimeIF");
            mPositionIF = transform.FindObject<InputField>("PositionIF");
            mDetailIF = transform.FindObject<InputField>("DetailIF");
            transform.FindObject<Button>("SubmitBtn").onClick.AddListener(SubmitBtnListener) ;
        }
        private void SubmitBtnListener()
        {
            if (mTitleIF.text.IsNullOrEmpty()) { 
                AppTools.ToastNotify("请输入标题");
                return;
            }
            if (mPriceIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入价格");
                return;
            }
            if (mJobTimeIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入工作时间");
                return;
            }
            if (mPositionIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入工作地点");
                return;
            }
            if (mDetailIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入工作详情");
                return;
            }
            IListData<byte[]> listData = ClassPool<ListData<byte[]>>.Pop();
            listData.Add(mTitleIF.text.ToBytes());
            listData.Add(mPriceIF.text.ToInt().ToBytes());
            listData.Add(mProceType.value.ToBytes());
            listData.Add(mJobTimeIF.text.ToBytes());
            listData.Add(mPositionIF.text.ToBytes());
            listData.Add(mDetailIF.text.ToBytes());
            listData.Add(AppVarData.Account.ToBytes());
            byte[] bytes = listData.list.ToBytes();
            listData.Recycle();
            AppTools.TcpSend(TcpSubServerType.Login,(short)TcpLoginUdpCode.ReleasePartTimeJob,bytes);
        }
    }
}
