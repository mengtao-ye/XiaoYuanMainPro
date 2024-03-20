using UnityEngine;
using YFramework;

namespace Game
{
    public interface IScrollViewItem<TScrollViewItem> where TScrollViewItem : IScrollViewItem<TScrollViewItem>
    {
        IScrollView< TScrollViewItem> scrollViewTarget { get; set; }
        long ID { get; set; }//对象唯一ID
        int index { get; set; }
        bool isInstantiate { get; }
        Vector2 size { get; set; }
        Vector2 originalPos { get; set; }
        IGameObjectPoolTarget poolTarget { get; set; }
        RectTransform rectTransform { get; }
        void MoveDelat(Vector2 delta);
        void MoveTo(Vector2 pos);
        bool IsShow(Vector2 posV2, Vector2 size);
        void LoadGameObject();
        bool CheckTopRecycle(Vector2 size);
        bool CheckBottomRecycle(Vector2 size);
        bool CheckTopInstantiate(Vector2 pos, Vector2 size);
        bool CheckBottomInstantiate(Vector2 pos, Vector2 size);
        void SetParent(Transform parent);
        void Recycle();
        void LoadData();
        void Exchange(IScrollViewItem< TScrollViewItem> item);
        void LoadToOriginalPos(Vector2 offect = default);
        void Exchange(int thisindex, Vector2 thisOriginalPos, bool thisIsInstantiate);
        void InsertTo(int targetIndex);
        void Exchange(int index);
    }
}
