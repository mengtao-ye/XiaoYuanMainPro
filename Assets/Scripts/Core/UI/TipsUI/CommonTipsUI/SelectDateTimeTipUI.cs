using Game;
using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

public enum TimeType
{
    Year,
    Month,
    Day,
    Hour,
    Minute,
    Second
}

public class TimeSelect
{
    private Text mContent;
    public int curValue;
    private TimeType type;
    private int mMax;
    private int mMin;
    private SelectDateTimeTipUI mSelectDateTimeTipUI;
    public TimeSelect(Transform trans, TimeType timeType, int curValue, SelectDateTimeTipUI selectDateTimeTipUI)
    {
        mSelectDateTimeTipUI = selectDateTimeTipUI;
        mContent = trans.GetComponent<Text>();
        trans.Find("Add").GetComponent<Button>().onClick.AddListener(AddBtnListener);
        trans.Find("Desc").GetComponent<Button>().onClick.AddListener(DescBtnListener);
        type = timeType;
        SetCurValue(curValue);
    }

    public void SetCurValue(int curValue)
    {
        this.curValue = curValue;
        mContent.text = this.curValue.ToString();
    }

    public void SetValue(int min,int max)
    {
        mMax = max;
        mMin = min;
        curValue = Mathf.Clamp(curValue, mMin, mMax);
        mContent.text = curValue.ToString();
    }
    private void AddBtnListener()
    {
        curValue++;
        if (curValue > mMax)
        {
            curValue = mMin;
        }
        mContent.text = curValue.ToString();
        if (type == TimeType.Month || type == TimeType.Year) 
        {
            int year = mSelectDateTimeTipUI.mYear.curValue;
            int month = mSelectDateTimeTipUI.mMonth.curValue;
            int maxDay = DateTimeTools.GetDaysInCurrentMonth(year, month);
            mSelectDateTimeTipUI.mDay.SetValue(1, maxDay);
        }
    }
    private void DescBtnListener()
    {
        curValue--;
        if (curValue < mMin)
        {
            curValue =mMax;
        }
        mContent.text = curValue.ToString();
        if (type == TimeType.Month || type == TimeType.Year)
        {
            int year = mSelectDateTimeTipUI.mYear.curValue;
            int month = mSelectDateTimeTipUI.mMonth.curValue;
            int maxDay = DateTimeTools.GetDaysInCurrentMonth(year, month);
            mSelectDateTimeTipUI.mDay.SetValue(1, maxDay);
        }
    }
}
namespace Game
{
    public class SelectDateTimeTipUI : BaseCustomTipsUI
    {
        public TimeSelect mYear;
        public TimeSelect mMonth;
        public TimeSelect mDay;
        public TimeSelect mHour;
        public TimeSelect mMinute;
        public TimeSelect mSecond;
        private Action<long> mCallBack;
        private float mHeight = 600;

        protected override ShowAnimEnum ShowAnim => ShowAnimEnum.None;
        protected override HideAnimEnum HideAnim =>  HideAnimEnum.None;

        public override void Awake()
        {
            Transform timeArea = transform.Find("TimeArea");
            mYear = AddValue(timeArea.Find("Year"), TimeType.Year, System.DateTime.Now.Year, 0000, 9999, this);
            mMonth = AddValue(timeArea.Find("Month"), TimeType.Month, System.DateTime.Now.Month, 1, 12, this);
            mDay = AddValue(timeArea.Find("Day"), TimeType.Day, System.DateTime.Now.Day, 1, 31, this);
            mHour = AddValue(timeArea.Find("Hour"), TimeType.Hour, 0, 0, 23, this);
            mMinute = AddValue(timeArea.Find("Minute"), TimeType.Minute, 0, 0, 59, this);
            mSecond = AddValue(timeArea.Find("Second"), TimeType.Second, 0, 0, 59, this);
            transform.FindObject<Button>("SureBtn").onClick.AddListener(SureBtnListener);
            transform.FindObject<Button>("CloseBtn").onClick.AddListener(Hide);

        }
        private void SureBtnListener()
        {
            int year = mYear.curValue;
            int month = mMonth.curValue;
            int day = mDay.curValue;
            int hour = mHour.curValue;
            int minute = mMinute.curValue;
            int second = mSecond.curValue;
            long value = DateTimeTools.GetValueByDateTime(year, month, day, hour, minute, second);
            mCallBack?.Invoke(value);
            Hide();
            //DateTime dateTime = DateTimeTools.GetDateTimeByValue(value);
            //mYear.SetCurValue(dateTime.Year);
            //mMonth.SetCurValue(dateTime.Month);
            //mDay.SetCurValue(dateTime.Day);
            //mHour.SetCurValue(dateTime.Hour);
            //mMinute.SetCurValue(dateTime.Minute);
            //mSecond.SetCurValue(dateTime.Second);
        }

        public override void Show()
        {
            base.Show();
            rectTransform.anchoredPosition = new Vector2(0,-mHeight);
            rectTransform.ToAnchorPositionMoveY(0,0.1f);
        }

        public override void Hide()
        {
            base.Hide();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.ToAnchorPositionMoveY(-mHeight, 0.1f);
            mCallBack = null;
        }

        public void SetCallBack(Action<long> callBack)
        {
            mCallBack = callBack;
        }

        private TimeSelect AddValue(Transform target, TimeType timeType, int curValue, int min, int max, SelectDateTimeTipUI selectDateTimeTipUI)
        {
            TimeSelect timeSelect = new TimeSelect(target, timeType, curValue, selectDateTimeTipUI);
            timeSelect.SetValue(min, max);
            return timeSelect;
        }
    } 
}
