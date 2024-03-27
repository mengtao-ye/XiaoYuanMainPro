using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    /// <summary>
    /// ���ƻ���ģ��
    /// </summary>
    public class CustomScrollView<TScrollView> : MonoBehaviour, IScrollView<TScrollView> where TScrollView : IScrollViewItem<TScrollView>
    {
        public RectTransform viewPort { get; private set; }//��ʾ����
        public RectTransform content { get; private set; }//����
        public IList<TScrollView> listData { get; private set; }//���ݽӿڶ���
        private float mTopSpace;//�������ϱߵľ���
        private float mBottomSpace;//�������±ߵľ���
        private float mSpace;//ÿ������֮��ľ���
        private float mVerticalSize;//���������С
        private float mVerticcalValue;//��ǰ�����ĵط�
        private bool mIsToBottom;//�Ƿ񻬵��ײ���
        private bool mIsToTop;//�Ƿ񻬵�������
        private int mTopIndex;//��ʾ����������±�
        private int mBottomIndex;//��ʾ����������±�
        private bool mIsFull;//�Ӷ����Ƿ�������Ļ��
        private float mBottomValue;//�ײ�ֵ
        private float mDragStartPos;//��ʼ������λ��
        private bool mIsSlidering;//�Ƿ��ڻ�����
        private float mDragLen;//�����ĳ���
        private float mDragTime;//������ʱ��
        private bool mIsUpDir;//�Ƿ������ϻ�����
        private float mDragStartTime;//��ʼ������ʱ��
        private bool mIsDraging;//�Ƿ����϶���
        private float[] mDragPosList;//��¼����ʱ��ָ���ڵ����֡λ��
        private int mDragPosIndex;//��¼����ʱ��ָ���ڵ����֡λ�õĵ�ǰ�±�
        private int mDragPosCount = 5;//��¼����ʱ��ָ���ڵ����֡λ�ø���
        private bool mUpFrash = false;//�Ƿ������������ˢ��
        private float mUpFrashValue = 20;//��������ˢ����ֵ
        private Action mUpFrashCallBack;//��������ˢ�»ص�
        private bool mDownFrash = false;//�Ƿ������������ˢ��
        private float mDownFrashValue = 20;//��������ˢ����ֵ
        private Action mDownFrashCallBack;//��������ˢ�»ص�
        public void Init(RectTransform viewPort = null, RectTransform content = null)
        {
            mVerticcalValue = 0;
            mDragPosIndex = 0;
            if (viewPort == null) this.viewPort = transform.Find("Viewport").GetComponent<RectTransform>();
            if (content == null) this.content = this.viewPort.Find("Content").GetComponent<RectTransform>();
            listData = new List<TScrollView>();
            mTopIndex = mBottomIndex = 0;
            mDragPosList = new float[mDragPosCount];
            mVerticalSize = 0;
        }
        /// <summary>
        ///���ü��
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
        /// ���㻬������
        /// </summary>
        private void CalcScrollView()
        {
            mBottomIndex = 0;
            mTopIndex = 0;
            for (int i = 0; i < listData.Count; i++)
            {
                if (listData[i].IsShow(new Vector2(0, mVerticcalValue), viewPort.rect.size))
                {
                    mBottomIndex++;
                }
                else
                {
                    if (mBottomIndex > mTopIndex)
                    {
                        break;
                    }
                    mTopIndex++;
                    mBottomIndex++;
                }
            }
            for (int i = 0; i < listData.Count; i++)
            {
                if (i < mTopIndex || i >= mBottomIndex)
                {
                    if (listData[i].isInstantiate)
                    {
                        listData[i].RecycleItem();
                    }
                }
                else
                {
                    if (!listData[i].isInstantiate)
                    {
                        listData[i].LoadGameObject();
                    }
                    else
                    {
                        listData[i].LoadToOriginalPos();
                    }
                }
            }
        }
        /// <summary>
        /// �����Ƿ�������Ļ��
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
        /// ��Ӷ���
        /// </summary>
        /// <param name="scrollViewItem"></param>
        public void Add(TScrollView scrollViewItem)
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
            CalcScrollView();
            CheckIsFull();
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="scrollViewItem"></param>
        /// <param name="index"></param>
        public void Insert(TScrollView scrollViewItem, int index)
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
            CalcScrollView();
            CheckIsFull();
        }
        /// <summary>
        /// ɾ���Ӷ���
        /// </summary>
        /// <param name="scrollViewItem"></param>
        public void Delete(TScrollView scrollViewItem)
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
            CalcScrollView();
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
        /// ���ݶ���ID��ɾ��
        /// </summary>
        /// <param name="itemID"></param>
        public void Delete(long itemID)
        {
            TScrollView scrollView = Get(itemID);
            if (scrollView == null) return;
            Delete(scrollView);
        }
        /// <summary>
        /// ���´�С
        /// </summary>
        /// <param name="scrollViewItem"></param>
        /// <param name="size"></param>
        public void UpdateSize(TScrollView scrollViewItem, Vector2 size)
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
            CalcScrollView();
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
        /// ��ָ�������ƶ���Ŀ��λ��
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
        /// ����
        /// </summary>
        /// <param name="exchangeIndex"></param>
        /// <param name="moveTargetIndex"></param>
        public void Exchange(int exchangeIndex, int moveTargetIndex)
        {
            if (!CheckIndex(moveTargetIndex)) return;
            if (!CheckIndex(exchangeIndex)) return;
            if (exchangeIndex == moveTargetIndex) return;
            TScrollView tempMoveTarget = listData[moveTargetIndex];
            listData[moveTargetIndex] = listData[exchangeIndex];
            listData[exchangeIndex] = tempMoveTarget;

            int index = listData[moveTargetIndex].index;
            Vector2 originalPos = listData[moveTargetIndex].originalPos;
            bool isInstantiate = listData[moveTargetIndex].isInstantiate;

            listData[moveTargetIndex].Exchange(listData[exchangeIndex]);
            listData[exchangeIndex].Exchange(index, originalPos, isInstantiate);

            listData[moveTargetIndex].RecycleItem();
            listData[exchangeIndex].RecycleItem();

            CalcScrollView();
        }
        /// <summary>
        /// ��ʼ�϶�
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
        /// �϶���
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (mIsToBottom) return;
            if (mIsToTop) return;
            mVerticcalValue += eventData.delta.y;
            MoveDelta(new Vector2(0, eventData.delta.y));
        }
        /// <summary>
        /// �����϶�
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            mIsDraging = false;
            if (mVerticcalValue < 0)//����϶��ľ���С��0˵����������
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
            if (mIsFull)//�����������
            {
                if (mVerticcalValue + viewPort.rect.size.y > mVerticalSize)//��ǰ����ֵ�����ӿڴ�С�Ƿ񳬹����ݴ�С
                {
                    //����ײ�ֵ
                    mBottomValue = mVerticcalValue - ((mVerticcalValue + viewPort.rect.size.y) - mVerticalSize);
                    mIsToBottom = true;
                    if (mVerticcalValue + viewPort.rect.size.y + mDownFrashValue > mVerticalSize)
                    {
                        mDownFrashCallBack?.Invoke();
                    }
                    return;
                }
            }
            else
            {
                mBottomValue = 0;
                mIsToBottom = true;
                return;
            }
            bool isSlider = true;//�Ƿ��ǻ���
            if (mDragPosList[mDragPosCount - 1] != 0)//������һ��ֵ��Ϊ��Ļ�˵������������֡
            {
                float count = 0;
                for (int i = 0; i < mDragPosList.Length; i++)
                {
                    count += mDragPosList[i];
                }
                count /= mDragPosCount;
                if (Mathf.Abs(mDragPosList[(mDragPosIndex + 4) % mDragPosCount] - count) <10)
                {
                    //���������ƫ��ֵ�Ƚ�С�Ļ����ж�Ϊ���϶�
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
                //���㻬������¼
                float ratio = Mathf.Clamp(Mathf.Abs(mDragLen / (mDragTime * 1000)), 1,10);
                mDragLen = mDragLen * ratio;
            }
        }
        /// <summary>
        /// ��λ�Ʋ����ƶ�
        /// </summary>
        /// <param name="vector"></param>
        private void MoveDelta(Vector2 vector)
        {
            content.anchoredPosition += vector;
            //�Ƿ������»���
            bool isToBottom = vector.y < 0;
            if (isToBottom)
            {
                //�²����Ƿ���Ҫ����
                CheckBottomRecycle();
                //�ϲ����Ƿ���Ҫ����
                CheckTopInstantiate();
            }
            else
            {
                //�²����Ƿ���Ҫ����
                CheckBottomInstantiate();
                //�ϲ����Ƿ���Ҫ����
                CheckTopRecycle();
            }
            CalcScrollView();
        }
        /// <summary>
        /// ��鶥���Ƿ����
        /// </summary>
        public void CheckTopRecycle()
        {
            if (CheckIndex(mTopIndex) && listData[mTopIndex].isInstantiate)
            {
                bool res = listData[mTopIndex].CheckTopRecycle(new Vector2(0, mVerticcalValue));
                if (res)
                {
                    //����һ�������
                    mTopIndex++;
                    CheckTopRecycle();
                }
            }
        }
        public void CheckBottomInstantiate()
        {
            if (CheckIndex(mBottomIndex) && !listData[mBottomIndex].isInstantiate)
            {
                bool res = listData[mBottomIndex].CheckBottomInstantiate(new Vector2(0, mVerticcalValue), viewPort.rect.size);
                
                if (res)
                {
                    //����һ����Ҫ����
                    mBottomIndex++;
                    CheckBottomInstantiate();
                }
            }
        }
        private void CheckBottomRecycle()
        {
            if (CheckIndex(mBottomIndex - 1) && listData[mBottomIndex - 1].isInstantiate)
            {
                bool res = listData[mBottomIndex - 1].CheckBottomRecycle(new Vector2(0, mVerticcalValue), viewPort.rect.size);
                if (res)
                {
                    //����һ�������
                    mBottomIndex--;
                    CheckBottomRecycle();
                }
            }
        }
        public void CheckTopInstantiate()
        {
            if (CheckIndex(mTopIndex - 1))//��һ��ʱ����Ҫ���� 
            {
                bool res = listData[mTopIndex - 1].CheckTopInstantiate(new Vector2(0, mVerticcalValue), viewPort.rect.size);
                if (res)
                {
                    //����һ����Ҫ����
                    mTopIndex--;
                    if (mTopIndex < 0) 
                    {
                        mTopIndex = 0;
                        return;

                    }
                    CheckTopInstantiate();
                }
            }
        }
        /// <summary>
        /// ����±��Ƿ�Խ��
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
                bool isToBound = false;//�Ƿ񵽱߽�
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
        /// �����������ʱֹͣ����
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            mIsSlidering = false;
        }
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TScrollView Get(long id)
        {
            for (int i = 0; i < listData.Count; i++)
            {
                if (listData[i].ViewItemID == id) return listData[i];
            }
            return default(TScrollView);
        }
        /// <summary>
        /// �Ƿ��������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Contains(long id)
        {
            return Get(id) != null;
        }
        /// <summary>
        /// ���յ�ǰ���ɵĶ���
        /// </summary>
        public void ClearItems()
        {
            for (int i = 0; i < listData.Count; i++)
            {
                listData[i].RecycleItem();
            }
            listData.Clear();
            mVerticcalValue = 0;
            mVerticalSize = mTopSpace + mBottomSpace;
            mTopIndex = mBottomIndex = 0;
        }
    }

}