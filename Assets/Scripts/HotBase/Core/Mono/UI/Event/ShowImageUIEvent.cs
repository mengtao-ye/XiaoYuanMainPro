using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    /// <summary>
    /// UI按压事件
    /// </summary>
    public class ShowImageUIEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        private Vector2 mDownPos;//按下时的位置
        private Vector2 mUpPos;//抬起时的位置
        private Vector2 mCurPos;//当前手部位置
        private bool mIsPress;//是否按压下去了
        private float mPressTimer;
        private float mPressTime;
        private Action mClickAction;
        private Action mPressAction;
        private Action mDoubleClickAction;//双击
        private float mDownTime;//点击下去的时间
        private float mUpTime;//抬起的时间
        private float mClickTime = 0.4f;//单击的时间
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
                //用户点击了一下屏幕
                if (Time.time - mDownTime > mClickTime)
                {
                    if (Vector2.Distance(mDownPos, mCurPos) < 10)
                    {
                        //用户是单击操作    
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
                    //用户是双击操作
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
