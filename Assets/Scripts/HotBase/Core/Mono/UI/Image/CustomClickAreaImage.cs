using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CustomClickAreaImage :  Image
    {
        private PolygonCollider2D mCollider;
        protected override void Awake()
        {
            mCollider = GetComponent<PolygonCollider2D>();
            if (mCollider == null) {
                Debug.LogError("CustomClickAreaImage�����PolygonCollider2D���");
            }
        }
        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (mCollider != null)
            {
                return mCollider.OverlapPoint(screenPoint);
            }
            else 
            {
                return false;
            }
        }
    } 
}
