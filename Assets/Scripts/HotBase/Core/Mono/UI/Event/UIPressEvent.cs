using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    /// <summary>
    /// UI按压事件
    /// </summary>
    public class UIPressEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerMoveHandler
    {
        private Vector2 mDownPos;
        private bool mIsPress;//是否按压下去了
        private float mTimer;
        private float mTime;
        private bool mIsPressState;
        private Action mClickAction;
        private Action mPressAction;
        public void SetClickAction(Action clickAction)
        {
            mClickAction = clickAction;
        }
        public void SetPressAction(Action pressAction)
        {
            mPressAction = pressAction;
        }
        private void Awake()
        {
            mTime = 0.2f;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (mIsPressState) return;
            mClickAction?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            mDownPos = eventData.position;
            mIsPress = true;
            mTimer = 0;
            mIsPressState = false;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!mIsPress) return;
            if (Vector2.Distance(mDownPos, eventData.position) > 5)
            {
                mIsPress = false;
            }
        }

        private void Update()
        {
            if (mIsPress)
            {
                mTimer += Time.deltaTime;
                if (mTimer > mTime)
                {
                    mIsPress = false;
                    mPressAction?.Invoke();
                    mTimer = 0;
                    mIsPressState = true;
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            mIsPress = false;
        }
    } 
}
