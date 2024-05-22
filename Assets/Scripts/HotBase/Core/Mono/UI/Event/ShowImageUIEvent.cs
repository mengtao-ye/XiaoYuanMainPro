using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    /// <summary>
    /// UI��ѹ�¼�
    /// </summary>
    public class ShowImageUIEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        private Vector2 mDownPos;//����ʱ��λ��
        private Vector2 mUpPos;//̧��ʱ��λ��
        private Vector2 mCurPos;//��ǰ�ֲ�λ��
        private bool mIsPress;//�Ƿ�ѹ��ȥ��
        private float mPressTimer;
        private float mPressTime;
        private Action mClickAction;
        private Action mPressAction;
        private Action mDoubleClickAction;//˫��
        private float mDownTime;//�����ȥ��ʱ��
        private float mUpTime;//̧���ʱ��
        private float mClickTime = 0.4f;//������ʱ��
        private void Awake()
        {
            mPressTime = 0.2f;
        }
        public void SetClickAction(Action clickAction)
        {
            mClickAction = clickAction;
        }
        public void SetDoubleClickAction(Action doubleClickAction)
        {
            mDoubleClickAction = doubleClickAction;
        }
        public void SetPressAction(Action pressAction)
        {
            mPressAction = pressAction;
        }
    
        public void OnPointerDown(PointerEventData eventData)
        {
            mIsPress = true;
            mPressTimer = 0;
            mCurPos = eventData.position;
            mDownPos = eventData.position;
            if (mDownTime == 0)
            {
                mDownTime = Time.time;
            }
            else
            {
                mDownTime = 0;
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            mCurPos = eventData.position;
            if (!mIsPress) return;
            if (Vector2.Distance(mDownPos, eventData.position) > 5)
            {
                mIsPress = false;
            }
        }

        private void Update()
        {
            if (mDownTime != 0)
            {
                //�û������һ����Ļ
                if (Time.time - mDownTime > mClickTime)
                {
                    if (Vector2.Distance(mDownPos, mCurPos) < 10)
                    {
                        //�û��ǵ�������    
                        mClickAction?.Invoke();
                    }
                    mDownTime = 0;
                }
            }
            if (mIsPress)
            {
                mPressTimer += Time.deltaTime;
                if (mPressTimer > mPressTime)
                {
                    mIsPress = false;
                    mPressAction?.Invoke();
                    mPressTimer = 0;
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            mIsPress = false;
            if (mUpTime != 0)
            {
                if (Time.time - mUpTime < mClickTime && eventData.delta == Vector2.zero)
                {
                    //�û���˫������
                    if (Vector2.Distance(mUpPos, eventData.position) <100)
                    {
                        mDoubleClickAction?.Invoke();
                    }
                    mUpTime = 0;
                }
                else 
                {
                    mUpTime = Time.time;
                }
            }
            else
            {
                mUpTime = Time.time;
            }
            mUpPos = eventData.position;
        }
    }
}
