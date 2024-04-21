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
        private GameObject mLoading;
        private GameObject mData;
        public RectTransform rectTransform;
        private List<IGameObjectPoolTarget> mPopTarget;
        private Vector2 mImageOriginalPos;
        private Text mLikeCount;
        private Text mCommitCount;
        private GameObject mIsLikeGo;
        public bool mIsLike;
        public override void Init(GameObject target)
        {
            base.Init(target);
            mPopTarget = new List<IGameObjectPoolTarget>();
            mCampusCircleID = 0;
            mAccount = 0;
            mIsLikeGo = transform.FindObject("IsLike");
            mIsLikeGo.SetActive(false);
            mLikeCount = transform.FindObject<Text>("LikeCount");
            mCommitCount = transform.FindObject<Text>("CommitCount");
            rectTransform = transform.GetComponent<RectTransform>();
            mData = transform.Find("Data").gameObject;
            mLoading = transform.Find("Loading").gameObject;
            mLoading.SetAvtiveExtend(true);
            mData.SetAvtiveExtend(false);
            mHead = transform.FindObject<Image>("Head");
            mName = transform.FindObject<Text>("Name");
            mContent = transform.FindObject<Text>("Content");
            mImages = transform.FindObject<RectTransform>("Images");
            mImageOriginalPos = mImages.anchoredPosition;
            mTime = transform.FindObject<Text>("Time");
            transform.FindObject<Button>("MoreBtn").onClick.AddListener(MoreBtnListener);
            transform.FindObject<Button>("LikeBtn").onClick.AddListener(LikeBtnListener);
            transform.FindObject<Button>("CommitBtn").onClick.AddListener(CommitBtnListener);
        }
        private void CommitBtnListener()
        {
            if (mCampusCircleID ==0 || mAccount ==0)
            {
                AppTools.ToastError("校友圈对象异常");
                return;
            }
            GameCenter.Instance.ShowTipsUI<CommitTipUI>((ui)=> {
                ui.SetCampusCircleID(mCampusCircleID);
            });
        }
        private void LikeBtnListener()
        {
            if (mCampusCircleID == 0 || mAccount == 0)
            {
                AppTools.ToastError("校友圈对象异常");
                return;
            }
            AppTools.UdpSend( SubServerType.Login,(short)LoginUdpCode.LikeCampusCircleItem,ByteTools.ConcatParam(AppVarData.Account.ToBytes(), mCampusCircleID.ToBytes(),mIsLike.ToBytes()));
        }

        private void MoreBtnListener()
        {
            if (mCampusCircleID == 0 || mAccount == 0)
            {
                AppTools.ToastError("校友圈对象异常");
                return;
            }
        }

        public float SetData(long id, long account, string content, string images, long time,bool isAnonymous,int likeCount,int commitCount,bool isLike)
        {
            mLoading.SetAvtiveExtend(false);
            mData.SetAvtiveExtend(true);
            mCampusCircleID = id;
            mAccount = account;
            if (isAnonymous)
            {
                mHead.sprite = DefaultValue.anonymousHead;
                mName.text = "匿名用户";
            }
            else 
            {
                UserDataModule.MapUserData(account, mHead, mName);
            }
            SetLikeCount(likeCount);
            mCommitCount.text = StringTools.ConverterCount(commitCount);
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
                string[] splits = images.Split('&');
                if (splits.Length == 1)
                {
                    string[] datas = splits[0].Split('_');
                    CampusCircleImageData campusCircleImageData = ClassPool<CampusCircleImageData>.Pop();
                    campusCircleImageData.imagePath = datas[0];
                    campusCircleImageData.size = ImageTools.GetSize(datas[1]);
                    campusCircleImageData.pos = Vector2.zero;
                    float maxWidth = YFrameworkHelper.Instance.ScreenSize.x * 0.7f;
                    float ratio = maxWidth / campusCircleImageData.size.x;
                    Vector2 size = campusCircleImageData.size * ratio;
                    campusCircleImageData.size = size;
                    len += size.y;
                    GameObjectPoolModule.AsyncPop<ImagePool, CampusCircleImageData>(mImages, ImageCallBack, campusCircleImageData);
                }
                else
                {
                    for (int i = 0; i < splits.Length; i++)
                    {
                        string[] datas = splits[i].Split('_');
                        CampusCircleImageData campusCircleImageData = ClassPool<CampusCircleImageData>.Pop();
                        campusCircleImageData.imagePath = datas[0];
                        campusCircleImageData.size = ImageTools.GetSize(datas[1]);
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
            imagePool.SetData(OssPathData.GetCampusCircleImage(data.imagePath) + OssPathData.GetSize(data.size.x * 0.5f, data.size.y * 0.5f) , Vector2.one * maxWidth, data.size);
            imagePool.rectTransform.anchoredPosition = data.pos;
            data.Recycle();
        }
        private void ImageCallBack(ImagePool imagePool, CampusCircleImageData data)
        {
            mPopTarget.Add(imagePool);
            imagePool.SetData(OssPathData.GetCampusCircleImage(data.imagePath) + OssPathData.GetSize(data.size.x * 0.5f, data.size.y * 0.5f), data.size);
            imagePool.rectTransform.anchoredPosition = data.pos;
            data.Recycle();
        }
        public void SetLoading()
        {
            mLoading.SetAvtiveExtend(true);
            mData.SetAvtiveExtend(false);
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
            mIsLike = isLike;
            mIsLikeGo.SetAvtiveExtend(isLike);
        }
        public void SetLikeCount(int likeCount)
        {
            mLikeCount.text = StringTools.ConverterCount(likeCount);
        }
    }

    public class CampusCircleImageData : IPool
    {
        public bool isPop { get; set; }
        public string imagePath;
        public Vector2 pos;
        public Vector2 size;
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
