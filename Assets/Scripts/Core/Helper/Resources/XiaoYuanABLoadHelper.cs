using System;
using UnityEngine;
using YFramework;

namespace Game
{
    public class XiaoYuanABLoadHelper : ResourceHelper
    {
        protected override void AsyncLoad<T>(string assetPath, Action<T> callBack) 
        {
            ABResModule<T>.Register(assetPath, callBack);
            LoadAssetType loadAssetType = LoadAssetType.GameObject;
            if (typeof(T).Name == typeof(GameObject).Name) 
            {
                loadAssetType = LoadAssetType.GameObject;
            }
            LauncherBridge.SendLoadAsset(ABTag.Main, assetPath, loadAssetType);
        }

        protected override T Load<T>(string assetPath) 
        {
            LogHelper.LogError("校元app不支持同步加载资源");
            return null;
        }
    }
}
