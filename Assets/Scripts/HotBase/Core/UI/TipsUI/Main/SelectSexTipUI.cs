using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class SelectSexTipUI : BaseCustomTipsUI
    {
        private ToggleGroup mTG;
        private Toggle[] mSexTG;
        public SelectSexTipUI()
        {

        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("CloseBtn").onClick.AddListener(Hide);
            mTG = transform.FindObject<ToggleGroup>("Sex");
            mSexTG = new Toggle[3];
            for (int i = 0; i < mTG.transform.childCount; i++)
            {
                mSexTG[i] = mTG.transform.Find(i.ToString()).GetComponent<Toggle>();
            }
            transform.FindObject<Button>("SureBtn").onClick.AddListener(SureBtnListener);
        }
        private void SureBtnListener() 
        {
            Toggle sex = mTG.GetActiveToggle();
            byte sexValue = sex.name.ToByte();
            byte[] sendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), sexValue);
            GameCenter.Instance.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.ModifySex, sendBytes);
            Hide();
        }
        public void ShowContent(byte sex)
        {
            Debug.Log(sex);
            if (sex == 1)
                mSexTG[1].isOn = true;
            else if (sex == 2)
                mSexTG[2].isOn = true;
            else 
                mSexTG[0].isOn = true;
        }
    }
}
