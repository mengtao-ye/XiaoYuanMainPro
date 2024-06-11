using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class PublishFoundPanel : BaseCustomPanel
    {
        private InputField mDetailIF;
        private InputField mContactIF;
        private InputField mNameIF;
        private InputField mPosIF;
        private InputField mQuestIF;
        private InputField mResultIF;
        private Dropdown mContactType;
        private Text mTimeText;
        private long mTimeLong;
        private string NullStr = "---";
        private RectTransform mImageArea;
        private RectTransform mAddBtn;
        private List<SelectImage> mSelectImageList;
        private NormalVerticalScrollView mScrollView;
        private VerticalLayoutGroup mReleaseVerticalLayoutGroup;
        private Button mSetQuestBtn;
        private GameObject mQuestArea;
        public PublishFoundPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mResultIF = transform.FindObject<InputField>("ResultIF");
            mQuestIF = transform.FindObject<InputField>("QuestIF");
            mDetailIF = transform.FindObject<InputField>("DetailIF");
            mContactIF = transform.FindObject<InputField>("ContactIF");
            mContactType = transform.FindObject<Dropdown>("ContactType");
            transform.FindObject<Button>("PublishBtn").onClick.AddListener(PublishBtnListener);
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { mUICanvas.CloseTopPanel(); });
            transform.FindObject<Button>("TimeBtn").onClick.AddListener(TimeListener);
            mNameIF = transform.FindObject<InputField>("NameIF");
            mPosIF = transform.FindObject<InputField>("PosIF");
            mTimeText = transform.FindObject<Text>("TimeText");
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
            mSetQuestBtn = transform.FindObject<Button>("SetQuestBtn");
            mSetQuestBtn.onClick.AddListener(SetQuestBtnListener);
            mQuestArea = transform.FindObject("QuestArea");
            mQuestArea.SetActive(false);
        }
        private void SetQuestBtnListener()
        {
            mSetQuestBtn.gameObject.SetActive(false);
            mQuestArea.SetActive(true);
        }

        public override void Show()
        {
            base.Show();
            mTimeLong = 0;
            mTimeText.text = NullStr;
            mSetQuestBtn.gameObject.SetActive(true);
            mQuestIF.text = string.Empty;
            mResultIF.text = string.Empty;
            mQuestArea.SetActive(false);
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
            if (mQuestArea.activeInHierarchy)
            {
                if (mQuestIF.text.IsNullOrEmpty())
                {
                    AppTools.ToastNotify("请输入问题内容");
                    return;
                }
                if (mResultIF.text.IsNullOrEmpty())
                {
                    AppTools.ToastNotify("请输入问题答案");
                    return;
                }
            }
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
            if (mContactIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入联系方式");
                return;
            }
            if (mTimeLong == default)
            {
                AppTools.ToastNotify("请输入开始时间");
                return;
            }
          
            IDictionaryData<byte, byte[]> dictionaryData = ClassPool<DictionaryData<byte, byte[]>>.Pop();
            dictionaryData.Add(0, mNameIF.text.ToBytes());
            dictionaryData.Add(1, mPosIF.text.ToBytes());
            dictionaryData.Add(2, mTimeLong.ToBytes());
            dictionaryData.Add(3, AppVarData.Account.ToBytes());
            dictionaryData.Add(4, "111".ToBytes());//TODO 图片
            dictionaryData.Add(5, SchoolGlobalVarData.SchoolCode.ToBytes());
            dictionaryData.Add(6, mDetailIF.text.ToBytes());
            dictionaryData.Add(7, ((byte)mContactType.value).ToBytes());
            dictionaryData.Add(8, mContactIF.text.ToBytes());
            if (mQuestArea.activeInHierarchy)
            {
                dictionaryData.Add(9, mResultIF.text.ToBytes());
                dictionaryData.Add(10, mQuestIF.text.ToBytes());
            }
            byte[] sendBytes = dictionaryData.data.ToBytes();
            dictionaryData.Recycle();
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.PublishFoundData, sendBytes);
        }

        private void TimeListener()
        {
            GameCenter.Instance.ShowTipsUI<ChoiceTimeTipUI>((ui) =>
            {
                ui.SetCallBack(SelectTimeCallback);
                ui.SetData(DateTime.Now);
            });
        }

        private void SelectTimeCallback(DateTime dt)
        {
            long time = DateTimeTools.GetValueByDateTime(dt);
            mTimeLong = time;
            mTimeText.text = dt.ToDateTime();
        }
    }
}
