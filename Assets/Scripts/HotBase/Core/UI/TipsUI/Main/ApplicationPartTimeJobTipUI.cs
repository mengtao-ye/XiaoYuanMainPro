using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class ApplicationPartTimeJobTipUI : BaseCustomTipsUI
    {
        private InputField mNameIF;
        private ToggleGroup mSexTG;
        private InputField mAgeIF;
        private InputField mCallIF;
        private int PartTimeJobID;
        public ApplicationPartTimeJobTipUI()
        {
        }
        public override void Awake()
        {
            base.Awake();

            mNameIF = transform.FindObject<InputField>("NameIF");
            mSexTG = transform.FindObject<ToggleGroup>("SexTG");
            mAgeIF = transform.FindObject<InputField>("AgeIF");
            mCallIF = transform.FindObject<InputField>("CallIF");
            transform.FindObject<Button>("CloseBtn").onClick.AddListener(Hide);
            transform.FindObject<Button>("SureBtn").onClick.AddListener(SureBtnListener);
        }
        private void SureBtnListener() {
            if (mNameIF.text.IsNullOrEmpty()) 
            {
                AppTools.ToastNotify("请填写真实姓名");
                return;
            }
            if (mAgeIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请填写年龄");
                return;
            }
            if (mCallIF.text.IsNullOrEmpty() || mCallIF.text.Length !=11)
            {
                AppTools.ToastNotify("联系方式格式错误");
                return;
            }
            IListData<byte[]> listData = ClassPool<ListData<byte[]>>.Pop();
            listData.Add(AppVarData.Account.ToBytes());
            listData.Add(PartTimeJobID.ToBytes());
            listData.Add(mNameIF.text.ToBytes());
            bool isMan = mSexTG.GetActiveToggle().name == "Man";
            listData.Add(isMan.ToBytes());
            int age = mAgeIF.text.ToInt();
            listData.Add(age.ToBytes());
            listData.Add(mCallIF.text.ToBytes());
            byte[] sendBytes = listData.list.ToBytes();
            listData.Recycle();
            AppTools.UdpSend( SubServerType.Login,(short) LoginUdpCode.ApplicationPartTimeJob, sendBytes);
        }
        public void SetPartTimeJobID(int id) 
        {
            PartTimeJobID = id;
        }
    }
}
