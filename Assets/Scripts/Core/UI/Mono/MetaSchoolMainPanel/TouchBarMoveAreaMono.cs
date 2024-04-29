using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class TouchBarMoveAreaMono : MonoBehaviour, IPointerDownHandler,IPointerUpHandler,IPointerMoveHandler
    {
        private RectTransform Bar;
        private RectTransform Center;
        private RectTransform Anchor;
        private bool mIsDown;
        private float mRadius;
        private Vector2 mDownPos;
        private Vector2 mDir;
        private float mDis;
        private Action<Vector2> mCallBack;
        private Action<float> mSpeedBack;
        private void Awake()
        {
            mRadius = 65;
            Bar = transform.GetChild(0).GetComponent<RectTransform>();
            Center = Bar.GetChild(0).GetComponent<RectTransform>();
            Anchor = Center.GetChild(0).GetComponent<RectTransform>();
            Bar.gameObject.SetActive(false);
        }

        public void SetCallBack(Action<Vector2> callback)
        {
            mCallBack = callback;
        }
        public void SetMoveSpeedBack(Action<float> callback)
        {
            mSpeedBack = callback;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            mDownPos = eventData.position;
            mIsDown = true;
            Bar.gameObject.SetActive(true);
            Bar.position = eventData.position;
            Anchor.anchoredPosition = Vector2.zero;
            Center.localRotation = Quaternion.identity;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            mIsDown = false;
            Bar.gameObject.SetActive(false);
            mSpeedBack?.Invoke(0);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!mIsDown) return;
            mDir = eventData.position- mDownPos;
            Center.up = mDir;
            mDis = Mathf.Clamp(Vector2.Distance(mDownPos, eventData.position),0,mRadius);
            mSpeedBack?.Invoke(mDis);
            Anchor.anchoredPosition = new Vector2(0, mDis);
        }
        private void FixedUpdate()
        {
            if (!mIsDown) return;
            mCallBack?.Invoke((Anchor.position - new Vector3(mDownPos.x, mDownPos.y, 0))/mRadius);
        }
    }
}
