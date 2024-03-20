using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    /// <summary>
    /// 自制滑动模块
    /// </summary>
    public class CustomScrollView<T> : MonoBehaviour, IScrollView<T> where T : IScrollViewItem<T>
    {
        public RectTransform viewPort { get; private set; }//显示窗口
        public RectTransform content { get; private set; }//内容
        public IList<T> listData { get; private set; }//内容接口对象
        private float mTopSpace;//距离最上边的距离
        private float mBottomSpace;//距离最下边的距离
        private float mSpace;//每个对象之间的距离
        private float mVerticalSize;//滑动区域大小
        private float mVerticcalValue;//当前滑倒的地方
        private bool mIsToBottom;//是否滑到底部了
        private bool mIsToTop;//是否滑到顶部了
        private int mTopIndex;//显示层最上面的下标
        private int mBottomIndex;//显示层最下面的下标
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
        public void Init(RectTransform viewPort = null, RectTransform content = null)
        {
            mVerticcalValue = 0;
            mDragPosIndex = 0;
            mVerticalSize = mTopSpace + mBottomSpace;
            if (viewPort == null) this.viewPort = transform.Find("Viewport").GetComponent<RectTransform>();
            if (content == null) this.content = this.viewPort.Find("Content").GetComponent<RectTransform>();
            listData = new List<T>();
            mTopIndex = mBottomIndex = 0;
            mDragPosList = new float[mDragPosCount];
        }

        public void SetSpace(float topSpace, float bottomSpace, float space)
        {
            mTopSpace = topSpace;
            mBottomSpace = bottomSpace;
            mSpace = space;
        }

        /// <summary>
        /// 添加子对象
        /// </summary>
        /// <param name="scrollViewItem"></param>
        public void Add(T scrollViewItem)
        {
            if (scrollViewItem == null) return;
            if (listData.Count != 0)
            {
                mVerticalSize += mSpace;//如果当前内容为空的话滑动条大小加上物体间隔
            }
            float pos = mVerticalSize - mBottomSpace;//计算生成对象的初始位置
            scrollViewItem.originalPos = new Vector2(0, pos);
            scrollViewItem.SetParent(content);
            bool needShow = scrollViewItem.IsShow(new Vector2(0, mVerticcalValue), viewPort.rect.size);
            if (needShow)
            {
                mBottomIndex++;
                scrollViewItem.LoadGameObject();
            }
            mVerticalSize += scrollViewItem.size.y;
            if (mVerticalSize > viewPort.rect.size.y)//如果当前大小超过视口大小的话
            {
                mIsFull = true;
            }
            else
            {
                mIsFull = false;
            }
            content.sizeDelta = new Vector2(content.sizeDelta.x, mVerticalSize);//更新内容大小
            scrollViewItem.index = listData.Count;//设置物体下标
            listData.Add(scrollViewItem);
            scrollViewItem.scrollViewTarget = this;
        }
        /// <summary>
        /// 删除子对象
        /// </summary>
        /// <param name="scrollViewItem"></param>
        public void Delete(T scrollViewItem)
        {
            if (scrollViewItem == null) return;
            float offectLen = 0;//偏移长度
            offectLen = scrollViewItem.size.y;
            bool targetIsInstantiate = scrollViewItem.isInstantiate;
            scrollViewItem.Recycle();
            if (targetIsInstantiate)//如果删除的对象是处于显示状态
            {
                mBottomIndex--;
            }
            else
            {
                //当前下标都往下移动一格
                mBottomIndex--;
                mTopIndex--;
            }
            if (listData.Count == 1)  //只有一个数据时
            {
                mTopIndex = mBottomIndex = 0;
            }
            else
            {
                offectLen += mSpace;
                for (int i = scrollViewItem.index + 1; i < listData.Count; i++)
                {
                    listData[i].index--;
                    listData[i].originalPos -= new Vector2(0, offectLen);
                    if (targetIsInstantiate)
                    {
                        bool needShow = listData[i].IsShow(new Vector2(0, mVerticcalValue), viewPort.rect.size);
                        if (needShow)
                        {
                            bool isInstantiate = listData[i].isInstantiate;//记录当前对象是否处于显示状态
                            listData[i].LoadGameObject();
                            if (isInstantiate)
                            {
                                //如果处于显示状态就修改偏移
                                listData[i].MoveDelat(new Vector2(0, offectLen));
                            }
                            else
                            {
                                //这里说明如果之前没实例化的话因为前面已经改变了它的原始坐标
                                //生成的时候会自动设置当前位置为原始坐标
                                //如果不处于显示状态，LoadGameObject()会设置原始位置，
                                //然后再加上当前滑动值的偏移
                                listData[i].MoveDelat(new Vector2(0, mVerticcalValue));
                                mBottomIndex++;
                            }
                        }
                    }
                }
            }
            if (!targetIsInstantiate)//如果需要删除的对象不处于显示状态的话
            {
                mVerticcalValue -= offectLen;
            }
            mVerticalSize -= offectLen;//总长度减去删除的对象长度
            if (mVerticalSize > viewPort.rect.size.y)//判断现在屏幕是否还是满的
            {
                mIsFull = true;
            }
            else
            {
                mIsFull = false;
            }
            content.sizeDelta = new Vector2(content.sizeDelta.x, mVerticalSize);//更新内容大小
            listData.Remove(scrollViewItem);
        }

        public void InsertTo(int insertIndex, int targetIndex)
        {
            if (insertIndex == targetIndex) return;
            if (!CheckIndex(insertIndex)) return;
            if (!CheckIndex(targetIndex)) return;
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
            T tempMoveTarget = listData[moveTargetIndex];
            listData[moveTargetIndex] = listData[exchangeIndex];
            listData[exchangeIndex] = tempMoveTarget;
            int index = listData[moveTargetIndex].index;
            Vector2 originalPos = listData[moveTargetIndex].originalPos;
            bool isInstantiate = listData[moveTargetIndex].isInstantiate;
            if (listData[moveTargetIndex].poolTarget != null)
            {
                listData[moveTargetIndex].Recycle();
            }
            if (listData[exchangeIndex].poolTarget != null)
            {
                listData[exchangeIndex].Recycle();
            }
            listData[moveTargetIndex].Exchange(listData[exchangeIndex]);
            listData[exchangeIndex].Exchange(index, originalPos, isInstantiate);

            bool isShow = listData[exchangeIndex].IsShow(new Vector2(0, mVerticcalValue), viewPort.rect.size);
            if (listData[exchangeIndex].isInstantiate)
            {
                if (isShow)
                {
                    listData[exchangeIndex].LoadGameObject();
                    listData[exchangeIndex].LoadToOriginalPos(new Vector2(0, mVerticcalValue));
                }
                else
                {
                    listData[exchangeIndex].Recycle();
                }
            }
            else
            {
                if (isShow)
                {
                    listData[exchangeIndex].LoadGameObject();
                    listData[exchangeIndex].LoadToOriginalPos(new Vector2(0, mVerticcalValue));
                }
            }

            isShow = listData[moveTargetIndex].IsShow(new Vector2(0, mVerticcalValue), viewPort.rect.size);
            if (listData[moveTargetIndex].isInstantiate)
            {
                if (isShow)
                {
                    listData[moveTargetIndex].LoadGameObject();
                    listData[moveTargetIndex].LoadToOriginalPos(new Vector2(0, mVerticcalValue));
                }
                else
                {
                    listData[moveTargetIndex].Recycle();
                }
            }
            else
            {
                if (isShow)
                {
                    listData[moveTargetIndex].LoadGameObject();
                    listData[moveTargetIndex].LoadToOriginalPos(new Vector2(0, mVerticcalValue));
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            mIsDraging = true;
            mDragStartTime = Time.time;
            mDragStartPos = eventData.position.y;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (mIsToBottom) return;
            if (mIsToTop) return;
            mVerticcalValue += eventData.delta.y;
            MoveDelta(new Vector2(0, eventData.delta.y));
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            mIsDraging = false;
            if (mVerticcalValue < 0)//如果拖动的距离小于0说明在向上拉
            {
                mIsToTop = true;
                return;
            }
            if (mIsFull)//如果内容满了
            {
                if (mVerticcalValue + viewPort.rect.size.y > mVerticalSize)//当前滑动值加上视口大小是否超过内容大小
                {
                    //计算底部值
                    mBottomValue = mVerticcalValue - ((mVerticcalValue + viewPort.rect.size.y) - mVerticalSize);
                    mIsToBottom = true;
                    return;
                }
            }
            else
            {
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
                if (Mathf.Abs(mDragPosList[(mDragPosIndex + 4) % mDragPosCount] - count) < 5)
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

        private void MoveDelta(Vector2 vector)
        {
            for (int i = mTopIndex; i < mBottomIndex; i++)
            {
                listData[i].MoveDelat(vector);
            }
            bool res = false;
            //是否是向下滑动
            bool isToBottom = vector.y < 0;
            if (isToBottom)
            {
                //下层检查是否需要回收
                if (CheckIndex(mBottomIndex - 1) && listData[mBottomIndex - 1].isInstantiate)
                {
                    res = listData[mBottomIndex - 1].CheckBottomRecycle(viewPort.rect.size);
                    if (res)
                    {
                        //最下一层回收了
                        mBottomIndex--;
                    }
                }
                //上层检查是否需要生成
                if (CheckIndex(mTopIndex - 1))//第一层时不需要生成 
                {
                    res = listData[mTopIndex - 1].CheckTopInstantiate(new Vector2(0, mVerticcalValue), viewPort.rect.size);
                    if (res)
                    {
                        //最上一层需要生成
                        mTopIndex--;
                        listData[mTopIndex].MoveDelat(new Vector2(vector.x, mVerticcalValue));
                    }
                }
            }
            else
            {
                //下层检查是否需要生成
                if (CheckIndex(mBottomIndex) && !listData[mBottomIndex].isInstantiate)
                {
                    res = listData[mBottomIndex].CheckBottomInstantiate(new Vector2(0, mVerticcalValue), viewPort.rect.size);
                    if (res)
                    {
                        //最下一层需要生成
                        listData[mBottomIndex].MoveDelat(new Vector2(vector.x, mVerticcalValue));
                        mBottomIndex++;
                    }
                }
                //上层检查是否需要回收
                if (CheckIndex(mTopIndex) && listData[mTopIndex].isInstantiate)
                {
                    res = listData[mTopIndex].CheckTopRecycle(viewPort.rect.size);
                    if (res)
                    {
                        //最上一层回收了
                        mTopIndex++;
                    }
                }
            }
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

            if (mIsToBottom)//如果滑到底部了
            {
                //计算当前帧偏移值
                float offect = Mathf.Lerp(mVerticcalValue, mBottomValue, Time.deltaTime * 10);
                float delta = mVerticcalValue - offect;
                mVerticcalValue = offect;
                if (mVerticcalValue < mBottomValue + 5f)
                {
                    MoveDelta(new Vector2(0, -(mVerticcalValue - mBottomValue)));
                    mVerticcalValue = mBottomValue;
                    mIsToBottom = false;
                    return;
                }
                MoveDelta(new Vector2(0, -delta));
                return;
            }
            if (mIsToTop)//如果滑到顶部了
            {
                //计算当前帧偏移值
                float offect = Mathf.Lerp(0, mVerticcalValue, Time.deltaTime * 10);
                mVerticcalValue -= offect;
                if (mVerticcalValue > -5f)
                {
                    MoveDelta(new Vector2(0, -mVerticcalValue));
                    mVerticcalValue = 0;
                    mIsToTop = false;
                    return;
                }
                MoveDelta(new Vector2(0, -offect));
                return;
            }
            if (mIsSlidering)//处于滑动状态
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
                if (isToBound)//是否滑到边界了
                {
                    mIsSlidering = false;
                    mDragLen = 0;
                    return;
                }
                //计算滑动的偏移值
                float offect = Mathf.Lerp(0, mDragLen, Time.deltaTime * 5);
                mDragLen -= offect;
                if (mIsUpDir)//是否是向上滑动
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

        public T Get(long id)
        {
            for (int i = 0; i < listData.Count; i++)
            {
                if (listData[i].ID == id) return listData[i];
            }
            return default(T);
        }

        public bool Contains(long id)
        {
            return Get(id) != null;
        }
    }

}