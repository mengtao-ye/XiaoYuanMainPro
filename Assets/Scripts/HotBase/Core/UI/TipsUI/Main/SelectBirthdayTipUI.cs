using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class SelectBirthdayTipUI : BaseCustomTipsUI
    {
        private InputField mYearIF;
        private InputField mMonthIF;
        private InputField mDayIF;
        public SelectBirthdayTipUI()
        {

        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("CloseBtn").onClick.AddListener(Hide);
            mYearIF = transform.FindObject<InputField>("YearIF");
            mMonthIF = transform.FindObject<InputField>("MonthIF");
            mDayIF = transform.FindObject<InputField>("DayIF");
            mYearIF.onEndEdit.AddListener(YearEndEditorListener);
            mMonthIF.onEndEdit.AddListener(MonthEndEditorListener);
            mDayIF.onEndEdit.AddListener(DayEndEditorListener);
            transform.FindObject<Button>("SureBtn").onClick.AddListener(SureBtnListener);
        }
        private void YearEndEditorListener(string value)
        {
            int year = value.ToShort();
            year = Mathf.Clamp(year, 1000, 9999);
            mYearIF.text = year.ToString();
        }
        private void MonthEndEditorListener(string value)
        {
            int month = value.ToShort();
            month = Mathf.Clamp(month,1,12);
            mMonthIF.text = month.ToString();
        }
        private void DayEndEditorListener(string value)
        {
            int day = value.ToShort();
            day = Mathf.Clamp(day, 1, 31);
            mDayIF.text = day.ToString();
        }
        private void SureBtnListener()
        {
            if (mYearIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入年份");
                return;
            }
            if (mMonthIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入月份");
                return;
            }
            if (mDayIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入日份");
                return;
            }
            int year = mYearIF.text.ToInt();
            int month = mMonthIF.text.ToInt();
            int day = mDayIF.text.ToInt();
            int brithday = UserDataTools.GetBrithdayInt(year,month,day);
            byte[] sendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), brithday.ToBytes());
            GameCenter.Instance.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.ModifyBrithday, sendBytes);
            Hide();
        }
        public void ShowContent(int birthday)
        {
            if (birthday == 0)
            {
                mYearIF.text =string.Empty;
                mMonthIF.text = string.Empty;
                mDayIF.text = string.Empty;
                return;
            }
            var tempBrithday = UserDataTools.ValueToBirthday(birthday);
            mYearIF.text = tempBrithday.Item1.ToString();
            mMonthIF.text = tempBrithday.Item2.ToString();
            mDayIF.text = tempBrithday.Item3.ToString();
        }
    }
}
