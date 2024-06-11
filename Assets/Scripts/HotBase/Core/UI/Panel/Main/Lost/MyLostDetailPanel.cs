using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class MyLostDetailPanel : BaseCustomPanel
    {
        private Text mTime;
        private Text mTipName;
        private Text mPos;
        private Text mDetail;
        private RectTransform mDetailArea;
        private List<Image> mImageList;
        private NormalVerticalScrollView mScrollView;
        private LostData mLostData;
        public MyLostDetailPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mImageList = new List<Image>();
            mScrollView = transform.FindObject("ScrollView").AddComponent<NormalVerticalScrollView>();
            mScrollView.Init();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { mUICanvas.CloseTopPanel(); });
            mTime = transform.FindObject<Text>("Time");
            mTipName = transform.FindObject<Text>("TipName");
            mPos = transform.FindObject<Text>("Pos");
            mDetail = transform.FindObject<Text>("Detail");
            mDetailArea = transform.FindObject<RectTransform>("DetailArea");
            for (int i = 1; i <= 9; i++)
            {
                mImageList.Add(mScrollView.content.Find("Images" + i).GetComponent<Image>());
            }
            transform.FindObject<Button>("DeleteBtn").onClick.AddListener(DeleteBtnListener);
        }
        private void DeleteBtnListener() {
            GameCenter.Instance.ShowTipsUI<CommonTwoTipsUI>((ui)=> 
            {
                ui.ShowContent("删除该失物招领?",string.Empty,"取消",null,"删除", DeleteLost);
            });
        }

        private void DeleteLost() 
        {
            
            AppTools.TcpSend(TcpSubServerType.Login,(short)TcpLoginUdpCode.DeleteLost,mLostData.id.ToBytes());
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
            mDetail.text = lostData.detail;
            mDetailArea.sizeDelta = new Vector2(mDetailArea.sizeDelta.x, mDetail.preferredHeight + 40);
            ShowImages(lostData.imageListData);
            LayoutRebuilder.ForceRebuildLayoutImmediate(mScrollView.content);
            mScrollView.SetSize(mScrollView.content.rect.size.y);
        }
        
    }
}
