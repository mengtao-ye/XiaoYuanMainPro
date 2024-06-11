using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class PublishLostPanel : BaseCustomPanel
    {
        private InputField mDetailIF;
        private InputField mContactIF;
        private InputField mNameIF;
        private InputField mPosIF;
        private Dropdown mContactType;
        private Text mStartTimeText;
        private Text mEndTimeText;
        private long mStartTimeLong;
        private long mEndTimeLong;
        private string NullStr = "---";
        private RectTransform mImageArea;
        private RectTransform mAddBtn;
        private List<SelectImage> mSelectImageList;
        private NormalVerticalScrollView mScrollView;
        private VerticalLayoutGroup mReleaseVerticalLayoutGroup;

        public PublishLostPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mDetailIF = transform.FindObject<InputField>("DetailIF");
            mContactIF = transform.FindObject<InputField>("ContactIF");
            mContactType = transform.FindObject<Dropdown>("ContactType");
            transform.FindObject<Button>("PublishBtn").onClick.AddListener(PublishBtnListener);
            transform.FindObject<Button>("BackBtn").onClick.AddListener(()=> { mUICanvas.CloseTopPanel(); });
            transform.FindObject<Button>("StartTime").onClick.AddListener(StartTimeListener);
            transform.FindObject<Button>("EndTime").onClick.AddListener(EndTimeListener);
            mNameIF = transform.FindObject<InputField>("NameIF");
            mPosIF = transform.FindObject<InputField>("PosIF");
            mStartTimeText = transform.FindObject<Text>("StartTimeText");
            mEndTimeText = transform.FindObject<Text>("EndTimeText");
            mReleaseVerticalLayoutGroup = transform.FindObject<VerticalLayoutGroup>("ReleaseContent");
            mSelectImageList = new List<SelectImage>();
            mScrollView = transform.Find("ScrollView").AddComponent<NormalVerticalScrollView>();
            mScrollView.Init();
            mImageArea = transform.FindObject<RectTransform>("ImageArea");
            float size = YFrameworkHelper.Instance.ScreenSize.x * 0.3f;
            mImageArea.SetSizeDeltaY(size + 40);
            float len = Game.UITools.GetVerticalSize(mReleaseVerticalLayoutGroup);
            mScrollView.SetSize(len);
            mReleaseVerticalLayoutGroup.gameObject.SetAvtiveHideAndShow();
            mAddBtn = mImageArea.transform.FindObject<RectTransform>("AddBtn");
            SetSelectImagePos(0, mAddBtn);
            mAddBtn.GetComponent<Button>().onClick.AddListener(AddBtnListener);
        }

        public override void Show()
        {
            base.Show();
            mEndTimeLong = mStartTimeLong = 0;
            mStartTimeText.text = NullStr;
            mEndTimeText.text = NullStr;
        }

        private void SetSelectImagePos(int index, RectTransform target)
        {
            float size = YFrameworkHelper.Instance.ScreenSize.x * 0.3f;
            target.sizeDelta = Vector2.one * size;
            int raw = index % 3;
            int col = index / 3;
            target.anchoredPosition = new Vector2(raw * size + (raw + 1) * 10 + 20, -(col * size + (col + 1) * 10 + 20));
        }

        private void AddBtnListener()
        {
            GameObjectPoolModule.AsyncPop<SelectImage>(mImageArea, (selectImage) =>
            {
                selectImage.SetDeleteCallback(DeleteCallBack);
                SetSelectImagePos(mSelectImageList.Count, selectImage.rectTransform);
                mSelectImageList.Add(selectImage);
                if (mSelectImageList.Count == 9)
                {
                    mAddBtn.gameObject.SetActive(false);
                }
                else
                {
                    SetSelectImagePos(mSelectImageList.Count, mAddBtn);
                    float size = YFrameworkHelper.Instance.ScreenSize.x * 0.3f;
                    int col = mSelectImageList.Count / 3;
                    mImageArea.SetSizeDeltaY((col + 1) * size + col * 10 + 40);
                    float len = Game.UITools.GetVerticalSize(mReleaseVerticalLayoutGroup);
                    mScrollView.SetSize(len);
                    mReleaseVerticalLayoutGroup.gameObject.SetAvtiveHideAndShow();

                }
            });
        }


        private void DeleteCallBack(SelectImage selectImage)
        {
            if (mSelectImageList.Contains(selectImage))
            {
                mSelectImageList.Remove(selectImage);
                for (int i = 0; i < mSelectImageList.Count; i++)
                {
                    SetSelectImagePos(i, mSelectImageList[i].rectTransform);
                }
            }
            if (!mAddBtn.gameObject.activeInHierarchy)
            {
                mAddBtn.gameObject.SetActive(true);
            }
            SetSelectImagePos(mSelectImageList.Count, mAddBtn);
        }

        private void PublishBtnListener()
        {
            if (mNameIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入名称");
                return;
            }
            if (mPosIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入丢失地点");
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
                AppTools.ToastNotify("开始时间应小于结束时间");
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
            dictionaryData.Add(5, "111".ToBytes());//TODO 图片
            dictionaryData.Add(6, SchoolGlobalVarData.SchoolCode.ToBytes());
            dictionaryData.Add(7, mDetailIF.text.ToBytes());
            dictionaryData.Add(8, ((byte) mContactType.value).ToBytes());
            dictionaryData.Add(9, mContactIF.text.ToBytes());

            byte[] sendBytes = dictionaryData.data.ToBytes();
            dictionaryData.Recycle();
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.PublishLostData, sendBytes);
        }

        private void StartTimeListener()
        {
            GameCenter.Instance.ShowTipsUI<ChoiceTimeTipUI>((ui) => {
                ui.SetCallBack(SelectStartTimeCallback);
                ui.SetData(DateTime.Now);
            });
        }

        private void SelectStartTimeCallback(DateTime dt)
        {
            long startTime = DateTimeTools.GetValueByDateTime(dt);
            if (mEndTimeLong!=0 &&  mEndTimeLong < startTime)
            {
                AppTools.ToastNotify("开始时间应小于结束时间");
                return;
            }
            mStartTimeLong = startTime;
            mStartTimeText.text = dt.ToDateTime();
        }
        private void SelectEndTimeCallback(DateTime time)
        {
            long endTime = DateTimeTools.GetValueByDateTime(time);
            if (mStartTimeLong != 0 && endTime < mStartTimeLong) 
            {
                AppTools.ToastNotify("结束时间应大于开始时间");
                return;
            }
            mEndTimeLong = endTime;
            mEndTimeText.text = time.ToDateTime();
        }
        private void EndTimeListener()
        {
            GameCenter.Instance.ShowTipsUI<ChoiceTimeTipUI>((ui) => {
                ui.SetCallBack(SelectEndTimeCallback);
                ui.SetData(DateTime.Now);
            });
        }
    }
}
