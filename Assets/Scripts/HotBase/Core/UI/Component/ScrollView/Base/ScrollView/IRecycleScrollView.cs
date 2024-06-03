using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using YFramework;

namespace Game
{
    public interface IRecycleScrollView : IScrollView
    {
        IScrollViewItem topScrollViewItem { get; }
    }
}
