using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class ShowMultiImageUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private float mHorizontal;
        private bool mIsSilde;
        private int mImageCount;
        private int mCurImageIndex;
        private RectTransform mImageArea;
        private bool mIsDrag;
        private List<SliderShowImageData> mImageList;
        private ShowImageUIEvent UIEvent;//UI操作对象
        private float mCurScale;//当前缩放大小
        private float mMinScale = 0.8f;//最小缩放
        private float mMaxScale = 3f;//最大缩放
        private SliderShowImageData mCurTarget { get { return mImageList[mCurImageIndex]; } }
        private bool mIsView => mCurScale != 1;
        private float initialDistance;//双指操作初始化值
        private Vector2 initialPosition1;//双指操作第一个值
        private Vector2 initialPosition2;//双指操作第二个值
        private bool mIsFirstDownScreen;//双指操作记录是否时刚开始点击屏幕
        private bool mIsDoubleFinger;
        private bool mIsDoubleFingerMinState;//是否是处于双指缩小时松开回弹的状态
        private bool mIsHorizontalDrag => mHorizontal != 0;//是否在水平拖动
        private float mStartDoubleFingerScale ;
        private ShowMultiImageTipUI mShowMultiImageTipUI;
        private void Awake()
        {
            mStartDoubleFingerScale = 1;
            mCurScale = 1;
            mImageList = new List<SliderShowImageData>();
            mImageCount = 0;
            mCurImageIndex = 0;
            mImageArea = transform.Find("ImageArea").GetComponent<RectTransform>();
            UIEvent = gameObject.AddComponent<ShowImageUIEvent>();
            UIEvent.SetClickAction(() => { mShowMultiImageTipUI.Hide(); });
            UIEvent.SetDoubleClickAction(() =>
            {
                if (!mIsView)
                {
                    SetScale(3f);
                    mCurTarget.image.rectTransform.anchoredPosition = mCurImageIndex * Vector2.right * Screen.width;
                }
                else
                {
                    SetToDefaultSize();
                    mCurTarget.image.rectTransform.anchoredPosition = mCurImageIndex * Vector2.right * Screen.width;
                }
            });
        }

        private void Update()
        {
            CheckDoubleFinger();
            CheckPos();
        }

        public void SetData(IListData<SelectImageData> imageList)
        {
            if (!imageList.IsNullOrEmpty())
            {
                for (int i = 0; i < imageList.Count; i++)
                {
                    SelectImageData selectImageData = imageList[i];
                    GameObjectPoolModule.AsyncPop<ShowImagePool>(mImageArea, (ui) =>
                     {
                         SliderShowImageData sliderShowImageData = ClassPool<SliderShowImageData>.Pop();
                         sliderShowImageData.SetData(ui, selectImageData,i);
                         mImageList.Add(sliderShowImageData);
                     });
                }
                mImageCount = imageList.Count;
            }
        }

        public void Hide()
        {
            mImageCount = 0;
            for (int i = 0; i < mImageList.Count; i++)
            {
                mImageList[i].Recycle();
            }
            mImageList.Clear();
        }
        public void SetTipUI(ShowMultiImageTipUI showMultiImageTipUI)
        {
            mShowMultiImageTipUI = showMultiImageTipUI;
        }
        private void SetScale(float scale)
        {
            mCurScale = Mathf.Clamp(scale, mMinScale, mMaxScale);
            mCurTarget.image.transform.localScale = Vector3.one * mCurScale;
        }
        private void SetToDefaultSize()
        {
            mCurScale = 1;
            mCurTarget.image.transform.localScale = Vector3.one;
        }
        private void SetDoubleFingerScale(float scale)
        {
            mCurScale = Mathf.Clamp(scale, mMinScale, mMaxScale);
            mCurTarget.image.transform.localScale = Vector3.one * mCurScale;
        }
       

        private void CheckPos() {
            if (!mIsDrag)
            {
                if (!mIsView|| mIsDoubleFingerMinState)
                {
                    if (Vector2.Distance(mImageArea.anchoredPosition, -mCurImageIndex * Vector2.right * Screen.width) > 0.1f)
                    {
                        mImageArea.anchoredPosition = Vector2.Lerp(mImageArea.anchoredPosition, -mCurImageIndex * Vector2.right * Screen.width, Time.deltaTime * 10);
                    }
                    if (Vector2.Distance(mCurTarget.image.rectTransform.anchoredPosition, mCurImageIndex * Vector2.right * Screen.width) > 0.1f)
                    {
                        mCurTarget.image.rectTransform.anchoredPosition = Vector2.Lerp(mCurTarget.image.rectTransform.anchoredPosition, mCurImageIndex * Vector2.right * Screen.width, Time.deltaTime * 10);
                    }
                }
                else
                {
                    if (!mIsDoubleFingerMinState)
                    {
                        float maxX = Screen.width * mCurScale / 2 - Screen.width / 2 + (mCurImageIndex * Screen.width);
                        float minX = -maxX + (mCurImageIndex * Screen.width * 2);
                        float maxY = 0;
                        float minY = 0;
                        if (mCurTarget.size.y * mCurScale < Screen.height)
                        {
                            minY = maxY = 0;
                        }
                        else
                        {
                            maxY = mCurTarget.size.y * mCurScale / 2 - Screen.height / 2;
                            minY = -maxY;
                        }
                        Vector2 targetPos = new Vector2(Mathf.Clamp(mCurTarget.image.rectTransform.anchoredPosition.x, minX, maxX), Mathf.Clamp(mCurTarget.image.rectTransform.anchoredPosition.y, minY, maxY));
                        if (Vector2.Distance(targetPos, mCurTarget.image.rectTransform.anchoredPosition) > 0.1f)
                        {
                            mCurTarget.image.rectTransform.anchoredPosition = Vector2.Lerp(mCurTarget.image.rectTransform.anchoredPosition, targetPos, Time.deltaTime * 20);
                        }
                    }
                }
            }
        }

        private void CheckDoubleFinger() 
        {
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(1))
#else
             if (Input.touchCount == 2)
#endif
            {
                mIsDoubleFinger = true;
                // 获取两个指针的当前位置
#if UNITY_EDITOR
                Vector2 position1 = Vector2.zero;
                Vector2 position2 = Input.mousePosition;
#else
                Vector2 position1 = Input.touches[0].position;
                Vector2 position2 = Input.touches[1].position;
#endif

                if (Vector2.Distance(position1, initialPosition1) != 0 || Vector2.Distance(position2, initialPosition2) != 0)
                {
                    initialPosition1 = position1;
                    initialPosition2 = position2;
                    // 开始第一次拖动时记录数据
                    if (!mIsFirstDownScreen)
                    {
                        mStartDoubleFingerScale = mCurScale;
                        mIsFirstDownScreen = true;
                        initialDistance = Vector2.Distance(position1, position2);
                    }
                    else
                    {
                        float currentDistance = Vector2.Distance(position1, position2);
                        float scaleFactor = currentDistance / initialDistance;
                        SetDoubleFingerScale(mStartDoubleFingerScale *  scaleFactor);
                    }
                }
            }
            else
            {
                mIsDoubleFinger = false;
                mCurScale = Mathf.Clamp(mCurTarget.image.transform.localScale.x, mMinScale, mMaxScale);
                mIsFirstDownScreen = false;
                if (mCurScale < 1)
                {
                    mIsDoubleFingerMinState = true;
                    mCurScale = Mathf.Lerp(mCurScale, 1, Time.deltaTime * 10);
                    if (mCurScale > 0.99)
                    {
                        mCurScale = 1;
                        mIsDoubleFingerMinState = false;
                    }
                    SetScale(mCurScale);
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            mIsDrag = true;
            mHorizontal = 0;
            mIsSilde = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (mIsView)
            {
                mCurTarget.image.rectTransform.anchoredPosition += eventData.delta;
            }
            else
            {
                if (mHorizontal > 0 && eventData.delta.x < 0)
                {
                    mIsSilde = false;
                }
                if (mHorizontal < 0 && eventData.delta.x > 0)
                {
                    mIsSilde = false;
                }
                mHorizontal += eventData.delta.x;
                if (!mIsDoubleFinger)
                {
                    mImageArea.anchoredPosition += Vector2.right * eventData.delta.x;
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            mIsDrag = false;
            if (!mIsView)
            {
                if (mIsSilde && Mathf.Abs(mHorizontal) > Screen.width * 0.1f)
                {
                    if (mHorizontal > 0)
                    {
                        if (mCurImageIndex > 0)
                        {
                            mCurImageIndex--;
                        }
                    }
                    else
                    {
                        if (mCurImageIndex < mImageCount - 1)
                        {
                            mCurImageIndex++;
                        }
                    }
                }
            }
        }
    }
}