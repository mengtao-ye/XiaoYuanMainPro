using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class CampusCircleItemPool : BaseGameObjectPoolTarget<CampusCircleItemPool>
    {
        public override string assetPath => "Prefabs/UI/Item/CampusCircle/CampusCircleItem";
        public override bool isUI => true;
        private Image mHead;
        private Text mName;
        private Text mContent;
        private RectTransform mImages;
        private Text mTime;
        private long mCampusCircleID;
        private long mAccount;
        private GameObject mData;
        public RectTransform rectTransform;
        private List<IGameObjectPoolTarget> mPopTarget;
        private Vector2 mImageOriginalPos;
        private Text mLikeCountText;
        private Text mCommitCountText;
        private Button mLikeBtn;
        public bool isLike
        {
            get
            {
                return mIsLike;
            }
            set
            {
                mIsLike = value;
                mLikeBtn.targetGraphic.color = mIsLike ? Color.red : ColorConstData.BottomNormalColor;
            }
        }
        private bool mIsLike;
        private IListData<SelectImageData> mSelectImageDatas;
        private bool mIsFriendCampusCircle;//是否是好友朋友圈
        private int mLikeCount;
        private int mCommitCount;
        public override void Init(GameObject target)
        {
            base.Init(target);
            mPopTarget = new List<IGameObjectPoolTarget>();
            mCampusCircleID = 0;
            mAccount = 0;

            mLikeCountText = transform.FindObject<Text>("LikeCount");
            mCommitCountText = transform.FindObject<Text>("CommitCount");
            rectTransform = transform.GetComponent<RectTransform>();
            mData = transform.Find("Data").gameObject;
            mData.SetActiveExtend(false);
            mHead = transform.FindObject<Image>("Head");
            mName = transform.FindObject<Text>("Name");
            mContent = transform.FindObject<Text>("Content");
            mImages = transform.FindObject<RectTransform>("Images");
            mImageOriginalPos = mImages.anchoredPosition;
            mTime = transform.FindObject<Text>("Time");
            transform.FindObject<Button>("MoreBtn").onClick.AddListener(MoreBtnListener);
            mLikeBtn = transform.FindObject<Button>("LikeBtn");
            mLikeBtn.onClick.AddListener(LikeBtnListener);
            transform.FindObject<Button>("CommitBtn").onClick.AddListener(CommitBtnListener);
        }
        private void CommitBtnListener()
        {
            if (mCampusCircleID == 0 || mAccount == 0)
            {
                AppTools.ToastError("校友圈对象异常");
                return;
            }
            GameCenter.Instance.ShowTipsUI<CommitTipUI>((ui) =>
            {
                ui.ShowContent(mCampusCircleID);
            });
        }
        private void LikeBtnListener()
        {
            if (mCampusCircleID == 0 || mAccount == 0)
            {
                AppTools.ToastError("校友圈对象异常");
                return;
            }
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.LikeCampusCircleItem, ByteTools.ConcatParam(AppVarData.Account.ToBytes(), mCampusCircleID.ToBytes(), mIsFriendCampusCircle.ToBytes()));
        }

        private void MoreBtnListener()
        {
            if (mCampusCircleID == 0 || mAccount == 0)
            {
                AppTools.ToastError("校友圈对象异常");
                return;
            }
        }

        public float SetData(long id, long account, string content, IListData<SelectImageData> images, long time, bool isAnonymous,bool isLike,bool isFriendCampusCircle)
        {
            transform.name =typeof(CampusCircleItemPool).Name +":"+id;
            mIsFriendCampusCircle = isFriendCampusCircle;
            mData.SetActiveExtend(true);
            mCampusCircleID = id;
            mAccount = account;
            if (isAnonymous)
            {
                DefaultSpriteValue.SetValue(DefaultSpriteValue.DEFAULT_ANONYMOUS_HEAD, mHead);
                mName.text = "匿名用户";
            }
            else
            {
                UserDataModule.MapUserData(account, mHead, mName);
            }
            float len = 0;
            if (!content.IsNullOrEmpty())
            {
                mContent.gameObject.SetActive(true);
                mContent.text = content;
                len += mContent.preferredHeight + 10;
            }
            else
            {
                mContent.gameObject.SetActive(false);
            }
            mImages.anchoredPosition = mImageOriginalPos;
            mImages.anchoredPosition -= new Vector2(0, len);
            if (!images.IsNullOrEmpty())
            {
                if (images.Count == 1)
                {
                    CampusCircleImageData campusCircleImageData = ClassPool<CampusCircleImageData>.Pop();
                    campusCircleImageData.imagePath = images[0].name.ToString();
                    campusCircleImageData.size = images[0].size;
                    campusCircleImageData.pos = Vector2.zero;
                    float maxWidth = YFrameworkHelper.Instance.ScreenSize.x * 0.7f;
                    float ratio = maxWidth / campusCircleImageData.size.x;
                    Vector2 size = campusCircleImageData.size * ratio;
                    campusCircleImageData.size = size;
                    len += size.y;
                    campusCircleImageData.listData = images;
                    GameObjectPoolModule.AsyncPop<ImagePool, CampusCircleImageData>(mImages, ImageCallBack, campusCircleImageData);
                }
                else
                {
                    for (int i = 0; i < images.Count; i++)
                    {
                        CampusCircleImageData campusCircleImageData = ClassPool<CampusCircleImageData>.Pop();
                        campusCircleImageData.imagePath = images[i].name.ToString();
                        campusCircleImageData.size = images[i].size;
                        float maxWidth = YFrameworkHelper.Instance.ScreenSize.x * 0.7f * 0.3f;
                        float ratio = maxWidth / campusCircleImageData.size.x;
                        Vector2 size = campusCircleImageData.size * ratio;
                        campusCircleImageData.size = size;
                        float posX = (i % 3) * size.x + (i % 3) * 10;
                        float posY = (i / 3) * size.x + (i / 3) * 10;
                        campusCircleImageData.pos = new Vector2(posX, -posY);
                        if (i % 3 == 0)
                        {
                            len += maxWidth + 5;
                        }
                        campusCircleImageData.listData = images;
                        GameObjectPoolModule.AsyncPop<MipImagePool, CampusCircleImageData>(mImages, MipImageCallBack, campusCircleImageData);
                    }
                }
            }
            SetIsLike(isLike);
            mTime.text = DateTimeTools.UnixTimeToShowTimeStr(time);
            return len;
        }
        private void MipImageCallBack(MipImagePool imagePool, CampusCircleImageData data)
        {
            mPopTarget.Add(imagePool);
            float maxWidth = YFrameworkHelper.Instance.ScreenSize.x * 0.7f * 0.3f;
            imagePool.SetData(OssPathData.GetCampusCircleImage(data.imagePath) + OssPathData.GetSize(data.size.x * 0.5f, data.size.y * 0.5f), Vector2.one * maxWidth, data.size);
            imagePool.rectTransform.anchoredPosition = data.pos;
            IListData<SelectImageData> listData = data.listData;
            imagePool.SetClickCallBack(() =>
            {
                GameCenter.Instance.ShowTipsUI<ShowMultiImageTipUI>((ui) =>
                {
                    ui.SetData(listData);
                });
            });
            data.Recycle();
        }


        private void ImageCallBack(ImagePool imagePool, CampusCircleImageData data)
        {
            mPopTarget.Add(imagePool);
            imagePool.SetData(OssPathData.GetCampusCircleImage(data.imagePath) + OssPathData.GetSize(data.size.x * 0.5f, data.size.y * 0.5f), data.size);
            imagePool.rectTransform.anchoredPosition = data.pos;
            IListData<SelectImageData> listData = data.listData;
            imagePool.SetClickCallBack(() =>
            {
                GameCenter.Instance.ShowTipsUI<ShowMultiImageTipUI>((ui) =>
                {
                    ui.SetData(listData);
                });
            });
            data.Recycle();
        }
        public void SetLoading()
        {
            mData.SetActiveExtend(false);
        }
        public override void Recycle()
        {
            for (int i = 0; i < mPopTarget.Count; i++)
            {
                GameObjectPoolModule.Push(mPopTarget[i]);
            }
            mPopTarget.Clear();
            GameObjectPoolModule.Push(this);
        }
        public void SetIsLike(bool isLike)
        {
            this.isLike = isLike;
            if (isLike)
            {
                SetLikeCount(mLikeCount+1);
            }
            else 
            { 
                SetLikeCount(mLikeCount-1);
            }
        }

        public void SetLikeCount(int likeCount)
        {
            mLikeCount = likeCount;
            mLikeCountText.text = StringTools.ConverterCount(likeCount);
        }
        public void SetCommitCount(int count)
        {
            mCommitCount = count;
            mCommitCountText.text = StringTools.ConverterCount(count);
        }
    }

    public class CampusCircleImageData : IPool
    {
        public bool isPop { get; set; }
        public string imagePath;
        public Vector2 pos;
        public Vector2 size;
        public IListData<SelectImageData> listData;
        public void PopPool()
        {
        }

        public void PushPool()
        {
        }

        public void Recycle()
        {
            ClassPool<CampusCircleImageData>.Push(this);
        }
    }
}
