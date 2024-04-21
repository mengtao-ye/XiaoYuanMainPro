using System;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class PublishLostPanel : BaseCustomPanel
    {
        private InputField mNameIF;
        private InputField mPosIF;
        private Text mStartTimeText;
        private Text mEndTimeText;
        private long mStartTimeLong;
        private long mEndTimeLong;
        private string NullStr = "---";
        public PublishLostPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("PublishBtn").onClick.AddListener(PublishBtnListener);
            transform.FindObject<Button>("BackBtn").onClick.AddListener(Hide);
            transform.FindObject<Button>("StartTime").onClick.AddListener(StartTimeListener);
            transform.FindObject<Button>("EndTime").onClick.AddListener(EndTimeListener);
            mNameIF = transform.FindObject<InputField>("NameIF");
            mPosIF = transform.FindObject<InputField>("PosIF");
            mStartTimeText = transform.FindObject<Text>("StartTimeText");
            mEndTimeText = transform.FindObject<Text>("EndTimeText");
        }

        private void PublishBtnListener()
        {
            if (mNameIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入名称");
                return;
            }
            if (mStartTimeLong == default) 
            {
                AppTools.ToastNotify("请输入开始时间");
                return;
            }
            if (mEndTimeLong == default)
            {
                AppTools.ToastNotify("请输入结束时间");
                return;
            }
            if (mEndTimeLong < mStartTimeLong)
            {
                AppTools.ToastNotify("开始时间应大于结束时间");
                return;
            }
            IDictionaryData<byte, byte[]> dictionaryData = ClassPool<DictionaryData<byte, byte[]>>.Pop();
            dictionaryData.Add(0, mNameIF.text.ToBytes());
            if (!mPosIF.text.IsNullOrEmpty())
            {
                dictionaryData.Add(1, mPosIF.text.ToBytes());
            }
            dictionaryData.Add(2, mStartTimeLong.ToBytes());
            dictionaryData.Add(3, mEndTimeLong.ToBytes());
            dictionaryData.Add(4, AppVarData.Account.ToBytes());
            dictionaryData.Add(5, "1111".ToBytes());//TODO 图片

            byte[] sendBytes = dictionaryData.data.ToBytes();
            dictionaryData.Recycle();
            AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.PublishLostData, sendBytes);
        }

        private void StartTimeListener()
        {
            GameCenter.Instance.ShowTipsUI<SelectDateTimeTipUI>((ui) => {
                ui.SetCallBack(SelectStartTimeCallbacl);
            });
        }

        private void SelectStartTimeCallbacl(long time)
        {
            mStartTimeLong = time;
            if (mEndTimeLong!=0 &&  mEndTimeLong < mStartTimeLong)
            {
                AppTools.ToastNotify("开始时间应小于结束时间");
                return;
            }
            DateTime dateTime = DateTimeTools.GetDateTimeByValue(time);
            mStartTimeText.text = dateTime.ToDateTime();
        }
        private void SelectEndTimeCallbacl(long time)
        {
            mEndTimeLong = time;
            if (mStartTimeLong != 0 && mEndTimeLong < mStartTimeLong) 
            {
                AppTools.ToastNotify("结束时间应大于开始时间");
                return;
            }
            DateTime dateTime = DateTimeTools.GetDateTimeByValue(time);
            mEndTimeText.text = dateTime.ToDateTime();
        }
        private void EndTimeListener()
        {
            GameCenter.Instance.ShowTipsUI<SelectDateTimeTipUI>((ui) => {
                ui.SetCallBack(SelectEndTimeCallbacl);
            });
        }

        public override void Show()
        {
            base.Show();
            mEndTimeLong = mStartTimeLong = 0;
            mStartTimeText.text = NullStr;
            mEndTimeText.text = NullStr;
        }
    }
}
