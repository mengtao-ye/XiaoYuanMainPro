using UnityEngine;
using YFramework;

namespace Game
{
    public abstract class BaseScrollViewItem<TPool,TScrollViewItem> : IScrollViewItem<TScrollViewItem> where TPool : class, IGameObjectPoolTarget, new() where TScrollViewItem : IScrollViewItem<TScrollViewItem>
    {
        public abstract Vector2 size { get; set; }
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
        public long ID { get; set ; }
        public IScrollView<TScrollViewItem> scrollViewTarget { get ; set; }

        public BaseScrollViewItem()
        {

        }

        public void LoadGameObject()
        {
            isInstantiate = true;
            poolTarget = GameObjectPoolModule.Pop<TPool>(mParent);
            LoadToOriginalPos();
            LoadData();
        }

        public void LoadToOriginalPos(Vector2 offect = default)
        {
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(0, -originalPos.y) + offect;
            }
        }

        public abstract void LoadData();

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

        public bool IsShow(Vector2 posV2, Vector2 size)
        {
            if (originalPos.y + this.size.y > posV2.y && originalPos.y - posV2.y < size.y)
            {
                return true;
            }
            return false;
        }

        public bool CheckTopRecycle(Vector2 size)
        {
            if (!isInstantiate) return false;
            if (-rectTransform.anchoredPosition.y + this.size.y < 0)
            {
                Recycle();
                return true;
            }
            return false;
        }

        public bool CheckBottomRecycle(Vector2 size)
        {
            if (!isInstantiate) return false;
            if (-rectTransform.anchoredPosition.y > size.y)
            {
                Recycle();
                return true;
            }
            return false;
        }

        public void Recycle()
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
            if (originalPos.y - pos.y < size.y)
            {

                LoadGameObject();
                return true;
            }
            return false;
        }

        public void Exchange(IScrollViewItem<TScrollViewItem> item)
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
            scrollViewTarget.InsertTo(index,targetIndex);
        }

        public void Exchange(int index)
        {
            scrollViewTarget.Exchange(this.index, index);
        }
    }
}
