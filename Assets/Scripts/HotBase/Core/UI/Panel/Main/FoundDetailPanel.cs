using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class FoundDetailPanel : BaseCustomPanel
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
        private FoundData mFoundData;
        private GameObject mDeleteBtn;
        private GameObject mContact;
        public FoundDetailPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mImageList = new List<Image>();
            mScrollView = transform.FindObject("ScrollView").AddComponent<NormalVerticalScrollView>();
            mScrollView.Init();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<FoundPanel>(); });
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
            mContact = transform.FindObject("Contact");
            mContact.transform.FindObject<Button>("ContactBtn").onClick.AddListener(ContactBtnListener);
            mDeleteBtn = transform.FindObject("DeleteBtn");
            mDeleteBtn.GetComponent<Button>().onClick.AddListener(DeleteBtnListener);

        }

        public override void Show()
        {
            base.Show();
            mContact.SetAvtiveExtend(false);
            mDeleteBtn.SetAvtiveExtend(false);
        }
        private void DeleteBtnListener()
        {
            GameCenter.Instance.ShowTipsUI<CommonTwoTipsUI>((ui) =>
            {
                ui.ShowContent("删除该失物招领?", string.Empty, "取消", null, "删除", DeleteLost);
            });
        }

        private void DeleteLost()
        {
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.DeleteFound, mFoundData.id.ToBytes());
        }
        private void ContactBtnListener() 
        {
            if (mFoundData.quest != string.Empty)
            {
                GameCenter.Instance.ShowTipsUI<FoundQuestTipUI>((ui) => {
                    ui.SetData(mFoundData);
                });
            }
            else 
            {
                GameCenter.Instance.ShowTipsUI<ContactTipUI>((ui) => {
                    ui.SetData(mFoundData.contactType, mFoundData.contact);
                });
            }
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

        public void SetData(FoundData foundData,bool isMy)
        {
            mContact.SetAvtiveExtend(!isMy);
            mDeleteBtn.SetAvtiveExtend(isMy);
            mFoundData = foundData;
            mTipName.text = "名称:"+ foundData.name;
            mPos.text = "地点:"+ foundData.pos;
            mTime.text = "时间:"+DateTimeTools.GetDateTimeByValue(foundData.time).ToDateTime();
            UserDataModule.MapUserData(foundData.account, mHead, mName);
            mDetail.text = foundData.detail;
            mDetailArea.sizeDelta = new Vector2(mDetailArea.sizeDelta.x, mDetail.preferredHeight + 40);
            ShowImages(foundData.imageListData);
            LayoutRebuilder.ForceRebuildLayoutImmediate(mScrollView.content);
            mScrollView.SetSize(mScrollView.content.rect.size.y);
        }
    }
}
