using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// ���ƻ���ģ��
    /// </summary>
    public class NormalHorizontalScrollView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public RectTransform viewPort { get; private set; }//��ʾ����
        public RectTransform content { get; private set; }//����
        private float mHorizontalSize;//���������С
        private float mHorizontalValue;//��ǰ�����ĵط�
        private bool mIsToRight;//�Ƿ񻬵��Ҳ���
        private bool mIsToLeft;//�Ƿ񻬵�����
        private bool mIsFull;//�Ӷ����Ƿ�������Ļ��
        private float mRightValue;//�ײ�ֵ
        private float mDragStartPos;//��ʼ������λ��
        private bool mIsSlidering;//�Ƿ��ڻ�����
        private float mDragLen;//�����ĳ���
        private float mDragTime;//������ʱ��
        private bool mIsLeftDir;//�Ƿ������󻬶���
        private float mDragStartTime;//��ʼ������ʱ��
        private bool mIsDraging;//�Ƿ����϶���
        private float[] mDragPosList;//��¼����ʱ��ָ���ڵ����֡λ��
        private int mDragPosIndex;//��¼����ʱ��ָ���ڵ����֡λ�õĵ�ǰ�±�
        private int mDragPosCount = 5;//��¼����ʱ��ָ���ڵ����֡λ�ø���
        private bool mLeftFrash = false;//�Ƿ������������ˢ��
        private float mLeftFrashValue = 100;//��������ˢ����ֵ
        private Action mLeftFrashCallBack;//��������ˢ�»ص�
        private bool mRightFrash = false;//�Ƿ������������ˢ��
        private float mRightFrashValue = 100;//��������ˢ����ֵ
        private Action mRightFrashCallBack;//��������ˢ�»ص�
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
        /// ��ʼ�϶�
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
        /// �϶���
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (mIsToRight) return;
            if (mIsToLeft) return;
            float ratio = 1;
            if (mHorizontalValue > 0)//����϶��ľ���С��0˵����������
            {
                ratio = Mathf.Min(1, 100 / mHorizontalValue);
            }
            else
            {
                if (mIsFull)//�����������
                {
                    if (-mHorizontalValue + viewPort.rect.size.x > mHorizontalSize)//��ǰ����ֵ�����ӿڴ�С�Ƿ񳬹����ݴ�С
                    {
                        //����ײ�ֵ
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
        /// �����϶�
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            mIsDraging = false;
            if (mHorizontalValue > 0)//����϶��ľ������0˵����������
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
            if (mIsFull)//�����������
            {
                if (-mHorizontalValue + viewPort.rect.size.x > mHorizontalSize)//��ǰ����ֵ�����ӿڴ�С�Ƿ񳬹����ݴ�С
                {
                    //����ײ�ֵ
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
            bool isSlider = true;//�Ƿ��ǻ���
            if (mDragPosList[mDragPosCount - 1] != 0)//������һ��ֵ��Ϊ��Ļ�˵������������֡
            {
                float count = 0;
                for (int i = 0; i < mDragPosList.Length; i++)
                {
                    count += mDragPosList[i];
                }
                count /= mDragPosCount;
                if (Mathf.Abs(mDragPosList[(mDragPosIndex + 4) % mDragPosCount] - count) < 10)
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
                mDragLen = eventData.position.x - mDragStartPos;
                mIsLeftDir = mDragLen < 0;
                mDragTime = Time.time - mDragStartTime;
                //���㻬������¼
                float ratio = Mathf.Clamp(Mathf.Abs(mDragLen / (mDragTime * 1000)), 1, 10);
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
                bool isToBound = false;//�Ƿ񵽱߽�
                if (mHorizontalValue > 0)
                {
                    mIsToLeft = true;
                    isToBound = true;
                }
                if (mIsFull)
                {
                    if (-mHorizontalValue + viewPort.rect.size.x > mHorizontalSize)//��ǰ����ֵ�����ӿڴ�С�Ƿ񳬹����ݴ�С
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
        /// �����������ʱֹͣ����
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            mIsSlidering = false;
        }
    }
}