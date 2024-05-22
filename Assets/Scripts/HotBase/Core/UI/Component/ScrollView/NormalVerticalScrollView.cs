using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using YFramework;

namespace Game
{
    /// <summary>
    /// 自制滑动模块
    /// </summary>
    public class NormalVerticalScrollView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public RectTransform viewPort { get; private set; }//显示窗口
        public RectTransform content { get; private set; }//内容
        private float mVerticalSize;//滑动区域大小
        private float mVerticcalValue;//当前滑倒的地方
        private bool mIsToBottom;//是否滑到底部了
        private bool mIsToTop;//是否滑到顶部了
        private bool mIsFull;//子对象是否填满屏幕了
        private float mBottomValue;//底部值
        private float mDragStartPos;//开始滑动的位置
        private bool mIsSlidering;//是否在滑动中
        private float mDragLen;//滑动的长度
        private float mDragTime;//滑动的时间
        private bool mIsUpDir;//是否是向上滑动的
        private float mDragStartTime;//开始滑动的时间
        private bool mIsDraging;//是否处于拖动中
        private float[] mDragPosList;//记录滑动时手指所在的最后几帧位置
        private int mDragPosIndex;//记录滑动时手指所在的最后几帧位置的当前下标
        private int mDragPosCount = 5;//记录滑动时手指所在的最后几帧位置个数
        private bool mUpFrash = false;//是否可以向上拉动刷新
        private float mUpFrashValue = 100;//向上拉动刷新阈值
        private Action mUpFrashCallBack;//向上拉动刷新回调
        private bool mDownFrash = false;//是否可以向上拉动刷新
        private float mDownFrashValue = 100;//向上拉动刷新阈值
        private Action mDownFrashCallBack;//向上拉动刷新回调
        public void Init(RectTransform viewPort = null, RectTransform content = null)
        {
            mVerticcalValue = 0;
            mDragPosIndex = 0;
            if (viewPort == null) this.viewPort = transform.GetChild(0).GetComponent<RectTransform>();
            else this.viewPort = viewPort;
            if (content == null) this.content = this.viewPort.GetChild(0).GetComponent<RectTransform>();
            else this.content = content;
            mDragPosList = new float[mDragPosCount];
            mVerticalSize = 0;
            mIsFull = this.content.rect.size.y > this.viewPort.rect.size.y;
            mVerticalSize = this.content.sizeDelta.y;
        }

        public void SetSize(float sizeY)
        {
            mVerticalSize = sizeY;
            content.SetSizeDeltaY(sizeY);
            mIsFull = mVerticalSize > this.viewPort.rect.size.y;
        }

        public void SetDownFrashCallBack(Action callBack)
        {
            mDownFrashCallBack = callBack;
        }
        public void SetDownFrashState(bool value)
        {
            mDownFrash = value;
        }
        public void SetDownFrashValue(float value)
        {
            mDownFrashValue = value;
        }
        public void SetUpFrashCallBack(Action callBack)
        {
            mUpFrashCallBack = callBack;
        }
        public void SetUpFrashState(bool value)
        {
            mUpFrash = value;
        }
        public void SetUpFrashValue(float value)
        {
            mUpFrashValue = value;
        }
        /// <summary>
        /// 开始拖动
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            mIsDraging = true;
            mDragStartTime = Time.time;
            mDragStartPos = eventData.position.y;
            mIsToBottom = false;
            mIsToTop = false;
        }
        /// <summary>
        /// 拖动中
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (mIsToBottom) return;
            if (mIsToTop) return;
            float ratio = 1;
            if (mVerticcalValue < 0)//如果拖动的距离小于0说明在向上拉
            {
                ratio = Mathf.Min(1, 100 / -mVerticcalValue);
            }
            else
            {
                if (mIsFull)//如果内容满了
                {
                    if (mVerticcalValue + viewPort.rect.size.y > mVerticalSize)//当前滑动值加上视口大小是否超过内容大小
                    {
                        //计算底部值
                        float value = mVerticcalValue + viewPort.rect.size.y - mVerticalSize;
                        ratio = Mathf.Min(1, 100 / value);
                    }
                }
                else
                {
                    ratio = Mathf.Min(1, 100 / mVerticcalValue);
                }
            }
            mVerticcalValue += eventData.delta.y * ratio;
            MoveDelta(new Vector2(0, eventData.delta.y * ratio));
        }
        /// <summary>
        /// 结束拖动
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            mIsDraging = false;
            if (mVerticcalValue < 0)//如果拖动的距离小于0说明在向上拉
            {
                mIsToTop = true;
                if (mUpFrash)
                {
                    if (-mVerticcalValue > mUpFrashValue)
                    {
                        mUpFrashCallBack?.Invoke();
                    }
                }
                return;
            }
            if (mIsFull)//如果内容满了
            {
                if (mVerticcalValue + viewPort.rect.size.y > mVerticalSize)//当前滑动值加上视口大小是否超过内容大小
                {
                    //计算底部值
                    mBottomValue = mVerticcalValue - ((mVerticcalValue + viewPort.rect.size.y) - mVerticalSize);
                    mIsToBottom = true;
                    if (mDownFrash)
                    {
                        if (mVerticcalValue + viewPort.rect.size.y + mDownFrashValue > mVerticalSize)
                        {
                            mDownFrashCallBack?.Invoke();
                        }
                    }
                    return;
                }
            }
            else
            {
                mBottomValue = 0;
                mIsToBottom = true;
                if (mDownFrash)
                {
                    mDownFrashCallBack?.Invoke();
                }
                return;
            }
            bool isSlider = true;//是否是滑动
            if (mDragPosList[mDragPosCount - 1] != 0)//如果最后一个值不为零的话说明滑动超过五帧
            {
                float count = 0;
                for (int i = 0; i < mDragPosList.Length; i++)
                {
                    count += mDragPosList[i];
                }
                count /= mDragPosCount;
                if (Mathf.Abs(mDragPosList[(mDragPosIndex + 4) % mDragPosCount] - count) < 10)
                {
                    //如果滑动的偏移值比较小的话就判断为是拖动
                    isSlider = false;
                }
            }
            for (int i = 0; i < mDragPosList.Length; i++)
            {
                mDragPosList[i] = 0;
            }
            mDragPosIndex = 0;

            if (isSlider)
            {
                mIsSlidering = true;
                mDragLen = eventData.position.y - mDragStartPos;
                mIsUpDir = mDragLen > 0;
                mDragTime = Time.time - mDragStartTime;
                //计算滑动的速录
                float ratio = Mathf.Clamp(Mathf.Abs(mDragLen / (mDragTime * 1000)), 1, 10);
                mDragLen = mDragLen * ratio;
            }
        }
        /// <summary>
        /// 按位移差来移动
        /// </summary>
        /// <param name="vector"></param>
        private void MoveDelta(Vector2 vector)
        {
            content.anchoredPosition += vector;
        }

        private void Update()
        {

            if (mIsDraging)
            {
                if (
#if UNITY_EDITOR
 Input.GetMouseButton(0)
#else
 Input.touchCount > 0
#endif
                    )
                {
                    mDragPosIndex = mDragPosIndex % mDragPosCount;
                    mDragPosList[mDragPosIndex++] =
#if UNITY_EDITOR
 Input.mousePosition.y;
#else
Input.touches[0].position.y;
#endif
                    mDragPosIndex = mDragPosIndex % mDragPosCount;
                }
            }

            if (mIsToBottom)
            {
                float offect = Mathf.Lerp(mVerticcalValue, mBottomValue, Time.deltaTime * 10);
                float delta = mVerticcalValue - offect;
                mVerticcalValue = offect;
                if (mVerticcalValue < mBottomValue + 5f)
                {
                    content.anchoredPosition = new Vector2(0, mBottomValue);
                    mVerticcalValue = mBottomValue;
                    mIsToBottom = false;
                    return;
                }
                MoveDelta(new Vector2(0, -delta));
                return;
            }
            if (mIsToTop)
            {
                float offect = Mathf.Lerp(0, mVerticcalValue, Time.deltaTime * 10);
                mVerticcalValue -= offect;
                if (mVerticcalValue > -5f)
                {
                    content.anchoredPosition = Vector2.zero;
                    mVerticcalValue = 0;
                    mIsToTop = false;
                    return;
                }
                MoveDelta(new Vector2(0, -offect));
                return;
            }
            if (mIsSlidering)
            {
                bool isToBound = false;//是否到边界
                if (mVerticcalValue < 0)
                {
                    mIsToTop = true;
                    isToBound = true;
                }
                if (mIsFull)
                {
                    if (mVerticcalValue + viewPort.rect.size.y > mVerticalSize)
                    {
                        mBottomValue = mVerticcalValue - ((mVerticcalValue + viewPort.rect.size.y) - mVerticalSize);
                        mIsToBottom = true;
                        isToBound = true;
                    }
                }
                else
                {
                    if (mVerticcalValue > mVerticalSize)
                    {
                        mBottomValue = mVerticalSize - mVerticcalValue;
                        mIsToBottom = true;
                        isToBound = true;
                    }
                }
                if (isToBound)
                {
                    mIsSlidering = false;
                    mDragLen = 0;
                    return;
                }
                float offect = Mathf.Lerp(0, mDragLen, Time.deltaTime * 5);
                mDragLen -= offect;
                if (mIsUpDir)
                {
                    if (mDragLen < 5)
                    {
                        mIsSlidering = false;
                        mVerticcalValue += mDragLen;
                        MoveDelta(new Vector2(0, mDragLen));
                        mDragLen = 0;
                        return;
                    }
                }
                else
                {
                    if (mDragLen > -5)
                    {
                        mVerticcalValue += mDragLen;
                        mIsSlidering = false;
                        MoveDelta(new Vector2(0, mDragLen));
                        mDragLen = 0;
                        return;
                    }
                }
                mVerticcalValue += offect;
                MoveDelta(new Vector2(0, offect));
            }
        }
        /// <summary>
        /// 点击滑动区域时停止滑动
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            mIsSlidering = false;
        }
    }
}