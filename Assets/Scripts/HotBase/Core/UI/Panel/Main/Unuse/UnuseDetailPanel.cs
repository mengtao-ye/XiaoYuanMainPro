using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class UnuseDetailPanel : BaseCustomPanel
    {
        private Text mTime;
        private Text mPrice;
        private Image mHead;
        private Text mName;
        private Text mDetail;
        private RectTransform mDetailArea;
        private UnuseData mUnuseData;
        private List<Image> mImageList;
        private NormalVerticalScrollView mScrollView;
        private bool mIsCollection;
        public bool isCollection
        {
            get { return mIsCollection; }
            set
            {
                mIsCollection = value;
                mCollectionBtn.targetGraphic.color = mIsCollection ? Color.red : Color.white;
            }
        }
        private Button mCollectionBtn;
        public UnuseDetailPanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mCollectionBtn = transform.FindObject<Button>("CollectionBtn");
            mCollectionBtn.onClick.AddListener(CollectionBtnListener);
            mImageList = new List<Image>();
            mScrollView = transform.FindObject("ScrollView").AddComponent<NormalVerticalScrollView>();
            mScrollView.Init();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { mUICanvas.CloseTopPanel(); });

            mTime = transform.FindObject<Text>("Time");
            mPrice = transform.FindObject<Text>("Price");
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
        public override void Show()
        {
            base.Show();
            isCollection = false;
        }

        private void CollectionBtnListener() 
        {
            byte[] sendBytes = ByteTools.ConcatParam(AppVarData.Account.ToBytes(), mUnuseData.id.ToBytes(),mIsCollection.ToBytes());
            AppTools.TcpSend( TcpSubServerType.Login,(short)TcpLoginUdpCode.CollectionUnuse, sendBytes);
        }

        private void ContactBtnListener()
        {
            GameCenter.Instance.ShowTipsUI<ContactTipUI>((ui) =>
            {
                ui.SetData(mUnuseData.contactType, mUnuseData.contact);
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
                    float ratio = YFrameworkHelper.Instance.ScreenSize.x / Mathf.Min(listData[i].sizeX, listData[i].sizeY);
                    mImageList[i].gameObject.SetActive(true);
                    mImageList[i].rectTransform.sizeDelta = new Vector2(listData[i].sizeX * ratio, listData[i].sizeY * ratio);
                    string path = OssPathData.GetUnuseImage(listData[i].name.ToString());
                    Image image = mImageList[i];
                    HttpTools.LoadSprite(path, (sprite) =>
                     {
                         image.sprite = sprite;
                     });
                }
            }
            for (int i = startIndex; i < 9; i++)
            {
                mImageList[i].gameObject.SetActive(false);
            }
            byte[] sendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(),mUnuseData.id.ToBytes());
            AppTools.TcpSend( TcpSubServerType.Login,(short)TcpLoginUdpCode.IsCollectionUnuse, sendBytes);
        }


        public void SetData(UnuseData data)
        {
            mUnuseData = data;
            mPrice.text = data.price + "¥";
            UserDataModule.MapUserData(data.account, mHead, mName);
            mTime.text = DateTimeTools.UnixTimeToShowTimeStr(data.time);
            mDetail.text = data.content;
            mDetailArea.sizeDelta = new Vector2(mDetailArea.sizeDelta.x, mDetail.preferredHeight + 40);
            ShowImages(data.imageTargets);
            LayoutRebuilder.ForceRebuildLayoutImmediate(mScrollView.content);
            mScrollView.SetSize(mScrollView.content.rect.size.y);
        }

    }
}
