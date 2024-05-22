using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class ChoiceTimeTipUI : BaseCustomTipsUI
    {
        /// <summary>
        /// 选择完成后的回调
        /// </summary>
        private Action<DateTime> mSelectTimeCallback;
        /// <summary>
        /// 最小日期和最大日期
        /// </summary>
        public DateTime minDateTime, maxDateTime;
        /// <summary>
        /// 最小日期和最大日期
        /// </summary>
        private DateTime mDefaultMinDateTime, mDefaultMaxDateTime;
        /// <summary>
        /// 选择的日期（年月日时分秒）
        /// </summary>
        public DateTime _selectDate;
        /// <summary>
        /// 时间选择器列表
        /// </summary>
        private List<DatePicker> _datePickerList;
    
        public ChoiceTimeTipUI()
        {
        }


        public override void Awake()
        {
            base.Awake();
            _selectDate = DateTime.Now;
            _datePickerList = new List<DatePicker>();
            mDefaultMinDateTime = new DateTime(1999, 1, 1, 0, 0, 0); 
            mDefaultMaxDateTime = new DateTime(2049, 1, 1, 0, 0, 0);

            maxDateTime = mDefaultMinDateTime;
            minDateTime = mDefaultMaxDateTime;
            Transform pickers = transform.FindObject<Transform>("Pickers");
            AddPicker(pickers.Find("Year_Picker"), DateType._year);
            AddPicker(pickers.Find("Month_Picker"), DateType._month);
            AddPicker(pickers.Find("Day_Picker"), DateType._day);
            AddPicker(pickers.Find("Hour_Picker"), DateType._hour);
            AddPicker(pickers.Find("Minute_Picker"), DateType._minute);
            AddPicker(pickers.Find("Second_Picker"), DateType._second);
            transform.FindObject<Button>("SureBtn").onClick.AddListener(()=> { mSelectTimeCallback?.Invoke(_selectDate); Hide(); });
        }

        public override void Show()
        {
            base.Show();
            SetBGClickCallBack(Hide);
        }

        public override void Hide()
        {
            base.Hide();
            mSelectTimeCallback = null;
            maxDateTime = mDefaultMinDateTime;
            minDateTime = mDefaultMaxDateTime;
        }

        public void SetCallBack(Action<DateTime> action)
        {
            mSelectTimeCallback = action;
        }
        private void AddPicker(Transform target,DateType dateType) 
        {
            DatePicker datePicker =  target.AddComponent<DatePicker>();
            datePicker.InitData(dateType, this);
            datePicker.Init();
            datePicker._onDateUpdate += onDateUpdate;
            _datePickerList.Add(datePicker);
        }

        public void SetData( DateTime curDateTime, DateTime minDateTime, DateTime maxDateTime) {
            _selectDate = curDateTime;
            this.minDateTime = minDateTime;
            this.maxDateTime = maxDateTime;
            for (int i = 0; i < _datePickerList.Count; i++)
            {
                _datePickerList[i].RefreshDateList();
            }
        }
        public void SetData(DateTime curDateTime)
        {
            SetData(curDateTime, mDefaultMinDateTime, mDefaultMaxDateTime);
        }
        public void SetData()
        {
            SetData(DateTime.Now);
        }
        /// <summary>
        /// 当选择的日期更新
        /// </summary>
        public void onDateUpdate()
        {
            for (int i = 0; i < _datePickerList.Count; i++)
            {
                _datePickerList[i].RefreshDateList();
            }
        }
    }
}