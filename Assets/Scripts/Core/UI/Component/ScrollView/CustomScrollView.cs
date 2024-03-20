using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    /// <summary>
    /// ���ƻ���ģ��
    /// </summary>
    public class CustomScrollView<T> : MonoBehaviour, IScrollView<T> where T : IScrollViewItem<T>
    {
        public RectTransform viewPort { get; private set; }//��ʾ����
        public RectTransform content { get; private set; }//����
        public IList<T> listData { get; private set; }//���ݽӿڶ���
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
        /// ����Ӷ���
        /// </summary>
        /// <param name="scrollViewItem"></param>
        public void Add(T scrollViewItem)
        {
            if (scrollViewItem == null) return;
            if (listData.Count != 0)
            {
                mVerticalSize += mSpace;//�����ǰ����Ϊ�յĻ���������С����������
            }
            float pos = mVerticalSize - mBottomSpace;//�������ɶ���ĳ�ʼλ��
            scrollViewItem.originalPos = new Vector2(0, pos);
            scrollViewItem.SetParent(content);
            bool needShow = scrollViewItem.IsShow(new Vector2(0, mVerticcalValue), viewPort.rect.size);
            if (needShow)
            {
                mBottomIndex++;
                scrollViewItem.LoadGameObject();
            }
            mVerticalSize += scrollViewItem.size.y;
            if (mVerticalSize > viewPort.rect.size.y)//�����ǰ��С�����ӿڴ�С�Ļ�
            {
                mIsFull = true;
            }
            else
            {
                mIsFull = false;
            }
            content.sizeDelta = new Vector2(content.sizeDelta.x, mVerticalSize);//�������ݴ�С
            scrollViewItem.index = listData.Count;//���������±�
            listData.Add(scrollViewItem);
            scrollViewItem.scrollViewTarget = this;
        }
        /// <summary>
        /// ɾ���Ӷ���
        /// </summary>
        /// <param name="scrollViewItem"></param>
        public void Delete(T scrollViewItem)
        {
            if (scrollViewItem == null) return;
            float offectLen = 0;//ƫ�Ƴ���
            offectLen = scrollViewItem.size.y;
            bool targetIsInstantiate = scrollViewItem.isInstantiate;
            scrollViewItem.Recycle();
            if (targetIsInstantiate)//���ɾ���Ķ����Ǵ�����ʾ״̬
            {
                mBottomIndex--;
            }
            else
            {
                //��ǰ�±궼�����ƶ�һ��
                mBottomIndex--;
                mTopIndex--;
            }
            if (listData.Count == 1)  //ֻ��һ������ʱ
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
                            bool isInstantiate = listData[i].isInstantiate;//��¼��ǰ�����Ƿ�����ʾ״̬
                            listData[i].LoadGameObject();
                            if (isInstantiate)
                            {
                                //���������ʾ״̬���޸�ƫ��
                                listData[i].MoveDelat(new Vector2(0, offectLen));
                            }
                            else
                            {
                                //����˵�����֮ǰûʵ�����Ļ���Ϊǰ���Ѿ��ı�������ԭʼ����
                                //���ɵ�ʱ����Զ����õ�ǰλ��Ϊԭʼ����
                                //�����������ʾ״̬��LoadGameObject()������ԭʼλ�ã�
                                //Ȼ���ټ��ϵ�ǰ����ֵ��ƫ��
                                listData[i].MoveDelat(new Vector2(0, mVerticcalValue));
                                mBottomIndex++;
                            }
                        }
                    }
                }
            }
            if (!targetIsInstantiate)//�����Ҫɾ���Ķ��󲻴�����ʾ״̬�Ļ�
            {
                mVerticcalValue -= offectLen;
            }
            mVerticalSize -= offectLen;//�ܳ��ȼ�ȥɾ���Ķ��󳤶�
            if (mVerticalSize > viewPort.rect.size.y)//�ж�������Ļ�Ƿ�������
            {
                mIsFull = true;
            }
            else
            {
                mIsFull = false;
            }
            content.sizeDelta = new Vector2(content.sizeDelta.x, mVerticalSize);//�������ݴ�С
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
        /// ����
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
            if (mVerticcalValue < 0)//����϶��ľ���С��0˵����������
            {
                mIsToTop = true;
                return;
            }
            if (mIsFull)//�����������
            {
                if (mVerticcalValue + viewPort.rect.size.y > mVerticalSize)//��ǰ����ֵ�����ӿڴ�С�Ƿ񳬹����ݴ�С
                {
                    //����ײ�ֵ
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
            bool isSlider = true;//�Ƿ��ǻ���
            if (mDragPosList[mDragPosCount - 1] != 0)//������һ��ֵ��Ϊ��Ļ�˵������������֡
            {
                float count = 0;
                for (int i = 0; i < mDragPosList.Length; i++)
                {
                    count += mDragPosList[i];
                }
                count /= mDragPosCount;
                if (Mathf.Abs(mDragPosList[(mDragPosIndex + 4) % mDragPosCount] - count) < 5)
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
            //�Ƿ������»���
            bool isToBottom = vector.y < 0;
            if (isToBottom)
            {
                //�²����Ƿ���Ҫ����
                if (CheckIndex(mBottomIndex - 1) && listData[mBottomIndex - 1].isInstantiate)
                {
                    res = listData[mBottomIndex - 1].CheckBottomRecycle(viewPort.rect.size);
                    if (res)
                    {
                        //����һ�������
                        mBottomIndex--;
                    }
                }
                //�ϲ����Ƿ���Ҫ����
                if (CheckIndex(mTopIndex - 1))//��һ��ʱ����Ҫ���� 
                {
                    res = listData[mTopIndex - 1].CheckTopInstantiate(new Vector2(0, mVerticcalValue), viewPort.rect.size);
                    if (res)
                    {
                        //����һ����Ҫ����
                        mTopIndex--;
                        listData[mTopIndex].MoveDelat(new Vector2(vector.x, mVerticcalValue));
                    }
                }
            }
            else
            {
                //�²����Ƿ���Ҫ����
                if (CheckIndex(mBottomIndex) && !listData[mBottomIndex].isInstantiate)
                {
                    res = listData[mBottomIndex].CheckBottomInstantiate(new Vector2(0, mVerticcalValue), viewPort.rect.size);
                    if (res)
                    {
                        //����һ����Ҫ����
                        listData[mBottomIndex].MoveDelat(new Vector2(vector.x, mVerticcalValue));
                        mBottomIndex++;
                    }
                }
                //�ϲ����Ƿ���Ҫ����
                if (CheckIndex(mTopIndex) && listData[mTopIndex].isInstantiate)
                {
                    res = listData[mTopIndex].CheckTopRecycle(viewPort.rect.size);
                    if (res)
                    {
                        //����һ�������
                        mTopIndex++;
                    }
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

            if (mIsToBottom)//��������ײ���
            {
                //���㵱ǰ֡ƫ��ֵ
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
            if (mIsToTop)//�������������
            {
                //���㵱ǰ֡ƫ��ֵ
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
            if (mIsSlidering)//���ڻ���״̬
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
                if (isToBound)//�Ƿ񻬵��߽���
                {
                    mIsSlidering = false;
                    mDragLen = 0;
                    return;
                }
                //���㻬����ƫ��ֵ
                float offect = Mathf.Lerp(0, mDragLen, Time.deltaTime * 5);
                mDragLen -= offect;
                if (mIsUpDir)//�Ƿ������ϻ���
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