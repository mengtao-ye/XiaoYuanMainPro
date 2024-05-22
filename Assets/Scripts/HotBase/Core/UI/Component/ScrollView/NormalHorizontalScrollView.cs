using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 自制滑动模块
    /// </summary>
    public class NormalHorizontalScrollView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public RectTransform viewPort { get; private set; }//显示窗口
        public RectTransform content { get; private set; }//内容
        private float mHorizontalSize;//滑动区域大小
        private float mHorizontalValue;//当前滑倒的地方
        private bool mIsToRight;//是否滑到右部了
        private bool mIsToLeft;//是否滑到左部了
        private bool mIsFull;//子对象是否填满屏幕了
        private float mRightValue;//底部值
        private float mDragStartPos;//开始滑动的位置
        private bool mIsSlidering;//是否在滑动中
        private float mDragLen;//滑动的长度
        private float mDragTime;//滑动的时间
        private bool mIsLeftDir;//是否是向左滑动的
        private float mDragStartTime;//开始滑动的时间
        private bool mIsDraging;//是否处于拖动中
        private float[] mDragPosList;//记录滑动时手指所在的最后几帧位置
        private int mDragPosIndex;//记录滑动时手指所在的最后几帧位置的当前下标
        private int mDragPosCount = 5;//记录滑动时手指所在的最后几帧位置个数
        private bool mLeftFrash = false;//是否可以向左拉动刷新
        private float mLeftFrashValue = 100;//向左拉动刷新阈值
        private Action mLeftFrashCallBack;//向左拉动刷新回调
        private bool mRightFrash = false;//是否可以向右拉动刷新
        private float mRightFrashValue = 100;//向右拉动刷新阈值
        private Action mRightFrashCallBack;//向右拉动刷新回调
        public void Init(RectTransform viewPort = null, RectTransform content = null)
        {
            mHorizontalValue = 0;
            mDragPosIndex = 0;
            if (viewPort == null)
            {
                this.viewPort = transform.Find("Viewport").GetComponent<RectTransform>();
            }
            else
            {
                this.viewPort = viewPort;
            }
            if (content == null)
            {
                this.content = this.viewPort.Find("Content").GetComponent<RectTransform>();
            }
            else
            {
                this.content = content;
            }
            mDragPosList = new float[mDragPosCount];
            mHorizontalSize = 0;
            mIsFull = this.content.rect.size.x > this.viewPort.rect.size.x;
            mHorizontalSize = this.content.sizeDelta.x;
        }

        public void SetSize(float sizeX)
        {
            mHorizontalSize = sizeX;
            mIsFull = mHorizontalSize > this.viewPort.rect.size.x;
        }

        public void SetRightFrashCallBack(Action callBack)
        {
            mRightFrashCallBack = callBack;
        }
        public void SetRightFrashState(bool value)
        {
            mRightFrash = value;
        }
        public void SetRightFrashValue(float value)
        {
            mRightFrashValue = value;
        }
        public void SetLeftFrashCallBack(Action callBack)
        {
            mLeftFrashCallBack = callBack;
        }
        public void SetLeftFrashState(bool value)
        {
            mLeftFrash = value;
        }
        public void SetLeftFrashValue(float value)
        {
            mLeftFrashValue = value;
        }
        /// <summary>
        /// 开始拖动
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            mIsDraging = true;
            mDragStartTime = Time.time;
            mDragStartPos = eventData.position.x;
            mIsToRight = false;
            mIsToLeft = false;
        }
        /// <summary>
        /// 拖动中
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (mIsToRight) return;
            if (mIsToLeft) return;
            float ratio = 1;
            if (mHorizontalValue > 0)//如果拖动的距离小于0说明在向右拉
            {
                ratio = Mathf.Min(1, 100 / mHorizontalValue);
            }
            else
            {
                if (mIsFull)//如果内容满了
                {
                    if (-mHorizontalValue + viewPort.rect.size.x > mHorizontalSize)//当前滑动值加上视口大小是否超过内容大小
                    {
                        //计算底部值
                        float value = -mHorizontalValue + viewPort.rect.size.x - mHorizontalSize;
                        ratio = Mathf.Min(1, 100 / value);
                    }
                }
                else
                {
                    ratio = Mathf.Min(1, MathF.Abs(100 / mHorizontalValue));
                }
            }
            mHorizontalValue += eventData.delta.x * ratio;
            MoveDelta(new Vector2(eventData.delta.x * ratio, 0));
        }
        /// <summary>
        /// 结束拖动
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            mIsDraging = false;
            if (mHorizontalValue > 0)//如果拖动的距离大于0说明在向上拉
            {
                mIsToLeft = true;
                if (mLeftFrash)
                {
                    if (mHorizontalValue > mLeftFrashValue)
                    {
                        mLeftFrashCallBack?.Invoke();
                    }
                }
                return;
            }
            if (mIsFull)//如果内容满了
            {
                if (-mHorizontalValue + viewPort.rect.size.x > mHorizontalSize)//当前滑动值加上视口大小是否超过内容大小
                {
                    //计算底部值
                    mRightValue = -(-mHorizontalValue - ((-mHorizontalValue + viewPort.rect.size.x) - mHorizontalSize));
                    mIsToRight = true;
                    if (mRightFrash)
                    {
                        if (-mHorizontalValue + viewPort.rect.size.x + mRightFrashValue > mHorizontalSize)
                        {
                            mRightFrashCallBack?.Invoke();
                        }
                    }
                    return;
                }
            }
            else
            {
                mRightValue = 0;
                mIsToRight = true;
                if (mRightFrash)
                {
                    mRightFrashCallBack?.Invoke();
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
                mDragLen = eventData.position.x - mDragStartPos;
                mIsLeftDir = mDragLen < 0;
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

            if (mIsToRight)
            {
                float offect = Mathf.Lerp(mHorizontalValue, mRightValue, Time.deltaTime * 10);
                float delta = mHorizontalValue - offect;
                mHorizontalValue = offect;
                if (mHorizontalValue > mRightValue + 5f)
                {
                    content.anchoredPosition = new Vector2(mRightValue, 0);
                    mHorizontalValue = mRightValue;
                    mIsToRight = false;
                    return;
                }
                MoveDelta(new Vector2(-delta, 0));
                return;
            }
            if (mIsToLeft)
            {
                float offect = Mathf.Lerp(0, mHorizontalValue, Time.deltaTime * 10);
                mHorizontalValue -= offect;
                if (mHorizontalValue < 5f)
                {
                    content.anchoredPosition = Vector2.zero;
                    mHorizontalValue = 0;
                    mIsToLeft = false;
                    return;
                }
                MoveDelta(new Vector2(-offect, 0));
                return;
            }
            if (mIsSlidering)
            {
                bool isToBound = false;//是否到边界
                if (mHorizontalValue > 0)
                {
                    mIsToLeft = true;
                    isToBound = true;
                }
                if (mIsFull)
                {
                    if (-mHorizontalValue + viewPort.rect.size.x > mHorizontalSize)//当前滑动值加上视口大小是否超过内容大小
                    {
                        mRightValue = -(-mHorizontalValue - ((-mHorizontalValue + viewPort.rect.size.x) - mHorizontalSize));
                        mIsToRight = true;
                        isToBound = true;
                    }
                }
                else
                {
                    if (mHorizontalValue > mHorizontalSize)
                    {
                        mRightValue = mHorizontalSize - mHorizontalValue;
                        mIsToRight = true;
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
                if (!mIsLeftDir)
                {
                    if (mDragLen < 5)
                    {
                        mIsSlidering = false;
                        mHorizontalValue -= mDragLen;
                        MoveDelta(new Vector2(-mDragLen, 0));
                        mDragLen = 0;
                        return;
                    }
                }
                else
                {
                    if (mDragLen > -5)
                    {
                        mHorizontalValue += mDragLen;
                        mIsSlidering = false;
                        MoveDelta(new Vector2(mDragLen, 0));
                        mDragLen = 0;
                        return;
                    }
                }
                mHorizontalValue += offect;
                MoveDelta(new Vector2(offect, 0));
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