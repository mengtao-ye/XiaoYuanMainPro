using System;
using UnityEngine;
using YFramework;

namespace Game
{
    public abstract class BaseScrollViewItem: IScrollViewItem
    {
        public abstract Vector2 size { get; set; }
        public abstract float anchoredPositionX { get; }
        private GameObject mGo;
        public GameObject gameoObject
        {
            get
            {
                if (mGo == null)
                {
                    if (poolTarget != null)
                    {
                        mGo = poolTarget.Target;
                    }
                }
                return mGo;
            }
        }
        private RectTransform mRectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (mRectTransform == null)
                {
                    if (poolTarget != null)
                    {
                        mRectTransform = poolTarget.Target.transform.GetComponent<RectTransform>();
                    }
                }
                return mRectTransform;
            }
        }
        public Vector2 originalPos { get; set; }
        public Transform mParent;
        public bool isInstantiate { get; private set; }
        public int index { get; set; }
        public IGameObjectPoolTarget poolTarget { get; set; }
        public long ViewItemID { get; set; }
        public IScrollView scrollViewTarget { get; set; }
        public bool isPop { get ; set ; }
        public BaseScrollViewItem()
        {

        }

        public void LoadGameObject()
        {
            isInstantiate = true;
            StartPopTarget();
        }
        protected abstract void StartPopTarget();
        protected virtual void PopTarget(IGameObjectPoolTarget target) 
        {
            poolTarget = target;
            LoadToOriginalPos();
            LoadData(poolTarget);
        }

        public void LoadToOriginalPos(Vector2 offect = default)
        {
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(anchoredPositionX, -originalPos.y) + offect;
            }
        }

        public abstract void LoadData(IGameObjectPoolTarget gameObjectPoolTarget);

        public void MoveDelat(Vector2 delta)
        {
            if (!isInstantiate) return;
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition += delta;
            }
        }

        public void SetParent(Transform parent)
        {
            mParent = parent;
        }

        public void SetOriginalPos(Vector2 pos)
        {
            originalPos = pos;
        }

        public void MoveTo(Vector2 pos)
        {
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = pos;
            }
        }
        public bool IsShow(Vector2 delta, Vector2 rectSize)
        {
            if (originalPos.y + this.size.y > delta.y && originalPos.y - delta.y < rectSize.y)
            {
                return true;
            }
            return false;
        }
        public bool CheckTopRecycle(Vector2 delta)
        {
            if (!isInstantiate) return false;
            if (originalPos.y + size.y - delta.y < 0)
            {
                RecycleItem();
                return true;
            }
            return false;
        }

        public bool CheckBottomRecycle(Vector2 delta, Vector2 size)
        {
            if (!isInstantiate) return false;
            if (originalPos.y > delta.y + size.y)
            {
                RecycleItem();
                return true;
            }
            return false;
        }

        public void RecycleItem()
        {
            isInstantiate = false;
            if (poolTarget != null)
            {
                mGo = null;
                mRectTransform = null;
                poolTarget.Recycle();
                poolTarget = null;
            }
        }

        public bool CheckTopInstantiate(Vector2 pos, Vector2 size)
        {
            if (isInstantiate) return false;
            if (originalPos.y + this.size.y > pos.y)
            {
                LoadGameObject();
                return true;
            }
            return false;
        }
        public bool CheckBottomInstantiate(Vector2 pos, Vector2 size)
        {
            if (isInstantiate) return false;
            if (originalPos.y < pos.y + size.y)
            {
                LoadGameObject();
                return true;
            }
            return false;
        }

        public void Exchange(IScrollViewItem item)
        {
            if (item == null) return;
            Exchange(item.index, item.originalPos, item.isInstantiate);
        }

        public void Exchange(int thisindex, Vector2 thisOriginalPos, bool thisIsInstantiate)
        {
            index = thisindex;
            originalPos = thisOriginalPos;
            isInstantiate = thisIsInstantiate;
        }

        public void InsertTo(int targetIndex)
        {
            scrollViewTarget.InsertTo(index, targetIndex);
        }

        public void Exchange(int index)
        {
            scrollViewTarget.Exchange(this.index, index);
        }

        public void UpdateSize(Vector2 size)
        {
            scrollViewTarget.UpdateSize(this,size);
        }

        public virtual void PopPool()   { }
        public virtual void PushPool()  { }
        public abstract void Recycle();
    }
}
