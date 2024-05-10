using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class DefaultSpriteValue
    {
        public const string DEFAULT_HEAD = "Images/Default/DefaultHead";
        public const string DEFAULT_ANONYMOUS_HEAD = "Images/Default/AnonymousHead";
        public const string DEFAULT_NEWFRIEND_HEAD = "Images/Default/NewFriend";
        private static Dictionary<string, Sprite> mDefaultSpriteDict;
        public static void SetValue(string assetPath, Action<Sprite> action)
        {
            if (assetPath == null || action == null)
            {
                LogHelper.LogError("DefaultSpriteValue .SetValue(string assetPath, Action<Sprite> action) 数据为空");
                return;
            }
            if (mDefaultSpriteDict == null)
            {
                mDefaultSpriteDict = new Dictionary<string, Sprite>();
            }
            if (mDefaultSpriteDict.ContainsKey(assetPath))
            {
                action?.Invoke(mDefaultSpriteDict[assetPath]);
            }
            else
            {
                ResourceHelper.AsyncLoadAsset<Sprite>(assetPath, (sp) =>
                {
                    mDefaultSpriteDict.Add(assetPath, sp);
                    action?.Invoke(sp);
                });
            }
        }
        public static void SetValue(string assetPath, Image image)
        {
            if (assetPath == null || image == null)
            {
                LogHelper.LogError("DefaultSpriteValue .SetValue(string assetPath,Image image) 数据为空");
                return;
            }
            if (mDefaultSpriteDict == null)
            {
                mDefaultSpriteDict = new Dictionary<string, Sprite>();
            }
            if (mDefaultSpriteDict.ContainsKey(assetPath))
            {
                image.sprite = mDefaultSpriteDict[assetPath];
            }
            else
            {
                ResourceHelper.AsyncLoadAsset<Sprite>(assetPath, (sp) =>
                {
                    mDefaultSpriteDict.Add(assetPath, sp);
                    image.sprite = sp;
                });
            }
        }
    }
}
