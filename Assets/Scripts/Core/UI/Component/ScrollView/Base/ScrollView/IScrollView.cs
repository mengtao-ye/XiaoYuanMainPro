using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using YFramework;

namespace Game
{
    public interface IScrollView< TScrollViewItem> : IBeginDragHandler, IDragHandler, IEndDragHandler ,IPointerDownHandler where TScrollViewItem : IScrollViewItem<TScrollViewItem>
    {
        IList<TScrollViewItem> listData { get; }
         RectTransform viewPort { get; }//显示窗口
         RectTransform content { get; }//内容
        void Add(TScrollViewItem scrollViewItem);
        void Delete(TScrollViewItem scrollViewItem);
        TScrollViewItem Get(long id);
        bool Contains(long id);
        void SetSpace(float topSpace, float bottomSpace, float space);
        void Init(RectTransform viewPort = null, RectTransform content = null);
        void InsertTo(int insertIndex, int targetIndex);
        void Exchange(int exchangeIndex, int moveTargetIndex);
    }
}
