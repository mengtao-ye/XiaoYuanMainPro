using System;
using UnityEditor;
using YFramework;

namespace Game
{
    public class ResourcesLoadHelper : ResourceHelper
    {
        protected override void AsyncLoad<T>(string assetPath, Action<T> callBack)
        {
            T value = UnityEngine.Resources.Load<T>(assetPath);
            callBack?.Invoke(value);
            callBack = null;
        }

        protected override T Load<T>(string assetPath)
        {
            LogHelper.LogError("校元工程请不要使用Load<T>,请使用AsyncLoad<T>");
            return null;
        }
    }
}
