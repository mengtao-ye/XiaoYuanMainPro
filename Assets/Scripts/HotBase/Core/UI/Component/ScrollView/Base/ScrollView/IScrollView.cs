using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using YFramework;

namespace Game
{
    public interface IScrollView: IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        IList<IScrollViewItem> listData { get; }
        int Count { get; }
        RectTransform viewPort { get; }//显示窗口
        RectTransform content { get; }//内容
        IScrollViewItem topScrollViewItem { get; }
        void Add(IScrollViewItem scrollViewItem);
        void Delete(IScrollViewItem scrollViewItem);
        void Delete(long itemID);
        IScrollViewItem Get(long id);
        bool Contains(long id);
        void SetSpace(float topSpace, float bottomSpace, float space);
        void Init(RectTransform viewPort = null, RectTransform content = null);
        void InsertTo(int insertIndex, int targetIndex);
        void Exchange(int exchangeIndex, int moveTargetIndex);
        void Insert(IScrollViewItem scrollViewItem, int index);
        void ClearItems();
        void UpdateSize(IScrollViewItem scrollViewItem, Vector2 size);
        void SetUpFrashCallBack(Action callBack);
        void SetUpFrashState(bool value);
        void SetDownFrashCallBack(Action callBack);
        void SetDownFrashState(bool value);
        void SetDragCallBack(Action dragCallBack);
    }
}
