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
    public class PoolScrollView : MonoBehaviour ,IScrollView
    {
        public RectTransform viewPort { get; private set; }//显示窗口
        public RectTransform content { get; private set; }//内容
        public IList<IScrollViewItem> listData { get; private set; }//内容接口对象
        public int Count => listData.Count;
        private float mTopSpace;//距离最上边的距离
        private float mBottomSpace;//距离最下边的距离
        private float mSpace;//每个对象之间的距离
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
        private float mUpFrashValue = 20;//向上拉动刷新阈值
        private Action mUpFrashCallBack;//向上拉动刷新回调
        private bool mDownFrash = false;//是否可以向上拉动刷新
        private float mDownFrashValue = 20;//向上拉动刷新阈值
        private Action mDownFrashCallBack;//向上拉动刷新回调
        private Action mDragCallBack;//滑动时的回调
        private GameObject Mask;
        public void Init(RectTransform viewPort = null, RectTransform content = null)
        {
            mVerticcalValue = 0;
            mDragPosIndex = 0;
            if (viewPort == null) this.viewPort = transform.Find("Viewport").GetComponent<RectTransform>();
            if (content == null) this.content = this.viewPort.Find("Content").GetComponent<RectTransform>();
            listData = new List<IScrollViewItem>();
            mDragPosList = new float[mDragPosCount];
            mVerticalSize = 0;
            Mask = transform.Find("Mask").gameObject;
            Mask.SetAvtiveExtend(false);
        }
        /// <summary>
        ///设置间隔
        /// </summary>
        /// <param name="topSpace"></param>
        /// <param name="bottomSpace"></param>
        /// <param name="space"></param>
        public void SetSpace(float topSpace, float bottomSpace, float space)
        {
            mTopSpace = topSpace;
            mBottomSpace = bottomSpace;
            mSpace = space;
            mVerticalSize = mTopSpace + mBottomSpace;
        }

        public void SetDownFrashCallBack(Action callBack)
        {
            mDownFrashCallBack = callBack;
        }
        public void SetDownFrashState(bool value)
        {
            mDownFrash = value;
        }
        public void SetUpFrashCallBack(Action callBack)
        {
            mUpFrashCallBack = callBack;
        }
        public void SetUpFrashState(bool value)
        {
            mUpFrash = value;
        }
        /// <summary>
        ///设置滑动时的回调
        /// </summary>
        /// <param name="dragCallBack"></param>
        public void SetDragCallBack(Action dragCallBack) 
        {
            mDragCallBack = dragCallBack;
        }

        /// <summary>
        /// 计算是否填满屏幕了
        /// </summary>
        private void CheckIsFull()
        {
            if (mVerticalSize > viewPort.rect.size.y)
            {
                mIsFull = true;
            }
            else
            {
                mIsFull = false;
            }
        }
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="scrollViewItem"></param>
        public void Add(IScrollViewItem scrollViewItem)
        {
            if (scrollViewItem == null) return;
            if (listData.Count != 0)
            {
                mVerticalSize += mSpace;
            }
            float pos = mVerticalSize - mBottomSpace;
            scrollViewItem.originalPos = new Vector2(0, pos);
            scrollViewItem.SetParent(content);
            mVerticalSize += scrollViewItem.size.y;
            content.sizeDelta = new Vector2(content.sizeDelta.x, mVerticalSize);
            scrollViewItem.index = listData.Count;
            scrollViewItem.scrollViewTarget = this;
            listData.Add(scrollViewItem);
            scrollViewItem.LoadGameObject();
            CheckIsFull();
        }
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="scrollViewItem"></param>
        /// <param name="index"></param>
        public void Insert(IScrollViewItem scrollViewItem, int index)
        {
            if (index < 0 && index > listData.Count) return;
            if (scrollViewItem == null) return;
            if (listData.Count != 0)
            {
                mVerticalSize += mSpace;
            }
            float pos = mTopSpace;
            for (int i = 0; i < index; i++)
            {
                pos += listData[i].size.y;
                pos += mSpace;
            }
            scrollViewItem.originalPos = new Vector2(0, pos);
            scrollViewItem.SetParent(content);
            mVerticalSize += scrollViewItem.size.y;
            content.sizeDelta = new Vector2(content.sizeDelta.x, mVerticalSize);
            scrollViewItem.index = index;
            for (int i = index; i < listData.Count; i++)
            {
                listData[i].index++;
                listData[i].originalPos += new Vector2(0, scrollViewItem.size.y + mSpace);
            }
            scrollViewItem.scrollViewTarget = this;
            listData.Insert(index, scrollViewItem);
            CheckIsFull();
        }
        /// <summary>
        /// 删除子对象
        /// </summary>
        /// <param name="scrollViewItem"></param>
        public void Delete(IScrollViewItem scrollViewItem)
        {
            if (scrollViewItem == null) return;
            if (listData.Count == 0) return;
            float offectLen = 0;
            offectLen = scrollViewItem.size.y;
            scrollViewItem.RecycleItem();
            if (listData.Count != 1)
            {
                offectLen += mSpace;
            }
            for (int i = scrollViewItem.index + 1; i < listData.Count; i++)
            {
                listData[i].index--;
                listData[i].originalPos -= new Vector2(0, offectLen);
            }
            //mVerticcalValue -= offectLen;
            mVerticalSize -= offectLen;
            content.sizeDelta = new Vector2(content.sizeDelta.x, mVerticalSize);
            listData.Remove(scrollViewItem);
            CheckIsFull();
            if (mIsFull)
            {
                if (mVerticcalValue + viewPort.rect.size.y > mVerticalSize)
                {
                    mBottomValue = mVerticcalValue - ((mVerticcalValue + viewPort.rect.size.y) - mVerticalSize);
                    mIsToBottom = true;
                }
            }
            else
            {
                mIsToTop = true;
            }
        }
        /// <summary>
        /// 根据对象ID来删除
        /// </summary>
        /// <param name="itemID"></param>
        public void Delete(long itemID)
        {
            IScrollViewItem scrollView = Get(itemID);
            if (scrollView == null) return;
            Delete(scrollView);
        }
        /// <summary>
        /// 更新大小
        /// </summary>
        /// <param name="scrollViewItem"></param>
        /// <param name="size"></param>
        public void UpdateSize(IScrollViewItem scrollViewItem, Vector2 size)
        {
            Vector2 offect = new Vector2(0, size.y - scrollViewItem.size.y);
            scrollViewItem.size = new Vector2(scrollViewItem.size.x, size.y);
            for (int i = scrollViewItem.index + 1; i < listData.Count; i++)
            {
                listData[i].originalPos += offect;
                listData[i].MoveDelat(offect);
            }
            mVerticalSize += offect.y;
            content.sizeDelta = new Vector2(content.sizeDelta.x, mVerticalSize);
            CheckIsFull();
            if (mIsFull)
            {
                if (mVerticcalValue + viewPort.rect.size.y > mVerticalSize)
                {
                    mBottomValue = mVerticcalValue - ((mVerticcalValue + viewPort.rect.size.y) - mVerticalSize);
                    mIsToBottom = true;
                }
            }
            else
            {
                mIsToTop = true;
            }
        }
        /// <summary>
        /// 将指定对象移动到目标位置
        /// </summary>
        /// <param name="insertIndex"></param>
        /// <param name="targetIndex"></param>
        public void InsertTo(int insertIndex, int targetIndex)
        {
            if (!CheckIndex(insertIndex)) return;
            if (!CheckIndex(targetIndex)) return;
            if (insertIndex == targetIndex) return;
            int startIndex = Mathf.Min(insertIndex, targetIndex);
            int endIndex = Mathf.Max(insertIndex, targetIndex);
            for (int i = endIndex; i > startIndex; i--)
            {
                Exchange(i, i - 1);
            }
        }
        /// <summary>
        /// 交换
        /// </summary>
        /// <param name="exchangeIndex"></param>
        /// <param name="moveTargetIndex"></param>
        public void Exchange(int exchangeIndex, int moveTargetIndex)
        {
            if (!CheckIndex(moveTargetIndex)) return;
            if (!CheckIndex(exchangeIndex)) return;
            if (exchangeIndex == moveTargetIndex) return;
            IScrollViewItem tempMoveTarget = listData[moveTargetIndex];
            listData[moveTargetIndex] = listData[exchangeIndex];
            listData[exchangeIndex] = tempMoveTarget;

            int index = listData[moveTargetIndex].index;
            Vector2 originalPos = listData[moveTargetIndex].originalPos;
            bool isInstantiate = listData[moveTargetIndex].isInstantiate;

            listData[moveTargetIndex].Exchange(listData[exchangeIndex]);
            listData[exchangeIndex].Exchange(index, originalPos, isInstantiate);

            listData[moveTargetIndex].RecycleItem();
            listData[exchangeIndex].RecycleItem();

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
            mVerticcalValue += eventData.delta.y  * ratio;
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
                    if (mDownFrash) {
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
                if (mDownFrash)
                {
                    mDownFrashCallBack?.Invoke();
                }
                mBottomValue = 0;
                mIsToBottom = true;
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
                if (Mathf.Abs(mDragPosList[(mDragPosIndex + 4) % mDragPosCount] - count) <10)
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
                float ratio = Mathf.Clamp(Mathf.Abs(mDragLen / (mDragTime * 1000)), 1,10);
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
            mDragCallBack?.Invoke();
        }
     
        /// <summary>
        /// 检查下标是否越界
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckIndex(int index)
        {
            if (index >= 0 && index < listData.Count) return true;
            return false;
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
                Mask.SetAvtiveExtend(true);
                float offect = Mathf.Lerp(mVerticcalValue, mBottomValue, Time.deltaTime * 10);
                float delta = mVerticcalValue - offect;
                mVerticcalValue = offect;
                if (mVerticcalValue < mBottomValue + 5f)
                {
                    content.anchoredPosition = new Vector2(0, mBottomValue);
                    mVerticcalValue = mBottomValue;
                    mIsToBottom = false;
                    Mask.SetAvtiveExtend(false);
                    return;
                }
                MoveDelta(new Vector2(0, -delta));
                return;
            }
            if (mIsToTop)
            {
                Mask.SetAvtiveExtend(true);

                float offect = Mathf.Lerp(0, mVerticcalValue, Time.deltaTime * 10);
                mVerticcalValue -= offect;
                if (mVerticcalValue > -5f)
                {
                    content.anchoredPosition = Vector2.zero;
                    mVerticcalValue = 0;
                    mIsToTop = false;
                    Mask.SetAvtiveExtend(false);
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
                Mask.SetAvtiveExtend(true);
                if (mIsUpDir)
                {
                    if (mDragLen < 5)
                    {
                        Mask.SetAvtiveExtend(false);

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
                        Mask.SetAvtiveExtend(false);
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
            Mask.SetAvtiveExtend(false);
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IScrollViewItem Get(long id)
        {
            for (int i = 0; i < listData.Count; i++)
            {
                if (listData[i].ViewItemID == id) return listData[i];
            }
            return null;
        }
        /// <summary>
        /// 是否包含对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Contains(long id)
        {
            return Get(id) != null;
        }
        /// <summary>
        /// 回收当前生成的对象
        /// </summary>
        public void ClearItems()
        {
            for (int i = 0; i < listData.Count; i++)
            {
                listData[i].RecycleItem();
                listData[i].Recycle();
            }
            listData.Clear();
            mVerticcalValue = 0;
            mVerticalSize = mTopSpace + mBottomSpace;
            content.sizeDelta = new Vector2(content.sizeDelta.x,0);
            content.anchoredPosition = new Vector2(content.anchoredPosition.x,0);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            
        }
    }

}