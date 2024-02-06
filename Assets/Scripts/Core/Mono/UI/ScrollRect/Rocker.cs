using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Game
{
    /// <summary>
    /// ҡ��
    /// </summary>
    public class Rocker : ScrollRect
    {
        protected float mRadius = 0f;
        private Action<Vector2> mRockerEvent;
        private Vector2 mDir;
        private bool mIsDrag;
        protected override void Start()
        {
            base.Start();
            //����ҡ�˿�İ뾶
            mRadius = (transform as RectTransform).sizeDelta.x * 0.5f;
        }
        private void Update()
        {
            if (mIsDrag) 
            {
                mRockerEvent?.Invoke(mDir);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            var contentPostion = this.content.anchoredPosition;
            if (contentPostion.magnitude > mRadius)
            {
                contentPostion = contentPostion.normalized * mRadius;
                SetContentAnchoredPosition(contentPostion);
            }
            mDir = contentPostion * 0.01f;
            mIsDrag = true;
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            mRockerEvent?.Invoke(Vector2.zero);
            mIsDrag = false;
        }
        /// <summary>
        /// ��ӻ��˼����¼�
        /// </summary>
        /// <param name="action"></param>
        public void AddRockerEvent(Action<Vector2> action)
        {
            mRockerEvent += action;
        }
    } 
}