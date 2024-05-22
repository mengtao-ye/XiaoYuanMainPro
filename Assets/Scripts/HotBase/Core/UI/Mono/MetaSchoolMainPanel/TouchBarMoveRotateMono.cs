using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class TouchBarMoveRotateMono : MonoBehaviour, IPointerDownHandler,IPointerUpHandler,IPointerMoveHandler
    {
        private bool mIsDown;
        private Action<Vector2> mCallBack;
        public void SetCallBack(Action<Vector2> callback)
        {
            mCallBack = callback;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            mIsDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            mIsDown = false;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!mIsDown) return;
            mCallBack?.Invoke(eventData.delta);
        }
      
    }
}
