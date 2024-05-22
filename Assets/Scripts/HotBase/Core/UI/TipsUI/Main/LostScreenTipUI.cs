using System;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class LostScreenTipUI : BaseCustomTipsUI
    {
        private Text mStartTime;
        private Text mEndTime;
        private DateTime mStartDateTime;
        private DateTime mEndDateTime;
        public LostScreenTipUI()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mStartTime = transform.FindObject<Text>("StartTimeText");
            mEndTime = transform.FindObject<Text>("EndTimeText");
            transform.FindObject<Button>("StartTime").onClick.AddListener(StartTimeListener);
            transform.FindObject<Button>("EndTime").onClick.AddListener(EndTimeListener);
            transform.FindObject<Button>("SureBtn").onClick.AddListener(SureBtnListener) ;
        }

        private void SureBtnListener()
        {
            if (mEndDateTime == default(DateTime))
            {
                mEndDateTime = DateTime.Now;
            }
            long startTime = DateTimeTools.GetValueByDateTime(mStartDateTime);
            long endTime = DateTimeTools.GetValueByDateTime(mEndDateTime);
            Hide();
        }

        private void StartTimeListener() 
        {
            GameCenter.Instance.ShowTipsUI<ChoiceTimeTipUI>((ui)=> {
                ui.SetCallBack(ChoiceStartTimeCallBack);
                ui.SetData(DateTime.Now);
            });
        }

        private void ChoiceStartTimeCallBack(DateTime dateTime) 
        {
            if (mEndDateTime == default(DateTime))
            {
                if (dateTime > DateTime.Now)
                {
                    AppTools.ToastNotify("起始时间不能大于今日");
                    return;
                }
            }
            else
            {
                if (mEndDateTime < dateTime)
                {
                    AppTools.ToastNotify("起始时间不能大于结束时间");
                    return;
                }
            }
            mStartDateTime = dateTime;
            mStartTime.text = dateTime.ToDateTime();
        }
        private void ChoiceEndTimeCallBack(DateTime dateTime)
        {
            if (mStartDateTime == default(DateTime))
            {
                if (dateTime > DateTime.Now)
                {
                    AppTools.ToastNotify("结束时间不能大于今日");
                    return;
                }
            }
            else
            {
                if (mStartDateTime > dateTime)
                {
                    AppTools.ToastNotify("结束时间不能小于开始时间");
                    return;
                }
            }
            mEndDateTime = dateTime;
            mEndTime.text = dateTime.ToDateTime();
        }

        private void EndTimeListener()
        {
            GameCenter.Instance.ShowTipsUI<ChoiceTimeTipUI>((ui) => {
                ui.SetCallBack(ChoiceEndTimeCallBack);
                ui.SetData(DateTime.Now);
            });
        }
    }
}
