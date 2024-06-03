using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class LostDetailPanel : BaseCustomPanel
    {
        private Text mTime;
        private Text mTipName;
        private Text mPos;
        private Image mHead;
        private Text mName;
        private Text mDetail;
        private RectTransform mDetailArea;
        private List<Image> mImageList;
        private NormalVerticalScrollView mScrollView;
        private LostData mLostData;
        public LostDetailPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mImageList = new List<Image>();
            mScrollView = transform.FindObject("ScrollView").AddComponent<NormalVerticalScrollView>();
            mScrollView.Init();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<LostPanel>(); });
            mTime = transform.FindObject<Text>("Time");
            mTipName = transform.FindObject<Text>("TipName");
            mPos = transform.FindObject<Text>("Pos");
            mHead = transform.FindObject<Image>("Head");
            mName = transform.FindObject<Text>("Name");
            mDetail = transform.FindObject<Text>("Detail");
            mDetailArea = transform.FindObject<RectTransform>("DetailArea");
            for (int i = 1; i <= 9; i++)
            {
                mImageList.Add(mScrollView.content.Find("Images" + i).GetComponent<Image>());
            }
            transform.FindObject<Button>("ContactBtn").onClick.AddListener(ContactBtnListener);
        }
        private void ContactBtnListener() {
            GameCenter.Instance.ShowTipsUI<ContactTipUI>((ui)=> {
                ui.SetData(mLostData.contactType,mLostData.contact);
            });
        }
        private void ShowImages(IListData<SelectImageData> listData)
        {
            int startIndex = 0;
            if (!listData.IsNullOrEmpty()) 
            {
                startIndex = listData.Count;
                for (int i = 0; i < listData.Count; i++)
                {
                    float ratio = YFrameworkHelper.Instance.ScreenSize.x/ Mathf.Min(listData[i].sizeX,listData[i].sizeY);
                    mImageList[i].gameObject.SetActive(true);
                    mImageList[i].rectTransform.sizeDelta = new Vector2(listData[i].sizeX * ratio, listData[i].sizeY * ratio);
                    string path = OssPathData.GetUnuseImage(listData[i].name.ToString());
                    Image image = mImageList[i];
                    HttpTools.LoadSprite(path,(sprite)=> 
                    {
                        image.sprite = sprite;
                    });
                }
            }
            for (int i = startIndex; i < 9; i++)
            {
                mImageList[i].gameObject.SetActive(false);
            }
        }

        public void SetData(LostData lostData)
        {
            mLostData = lostData;
            mTipName.text = "名称:"+ lostData.name;
            mPos.text = "地点:"+ lostData.pos;
            mTime.text = "时间:"+DateTimeTools.GetDateTimeByValue(lostData.startTime).ToDateTime() +"-" + DateTimeTools.GetDateTimeByValue(lostData.endTime).ToDateTime();
            UserDataModule.MapUserData(lostData.account, mHead, mName);
            mDetail.text = lostData.detail;
            mDetailArea.sizeDelta = new Vector2(mDetailArea.sizeDelta.x, mDetail.preferredHeight + 40);
            ShowImages(lostData.imageListData);
            LayoutRebuilder.ForceRebuildLayoutImmediate(mScrollView.content);
            mScrollView.SetSize(mScrollView.content.rect.size.y);
        }
        
    }
}
