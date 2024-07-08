using System;
using System.Collections.Generic;
using YFramework;

namespace Game
{
    /// <summary>
    /// ab包资源管理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ABResModule<T>
    {
        private static Dictionary<string, Action<T>> mGameObjectActionDict = new Dictionary<string, Action<T>>();
        private static Dictionary<string, T> mGameObjectDict = new Dictionary<string, T>();
        public static void BoardCast(string abName,T obj)
        {
            if (abName == null) {
                LogHelper.LogError("ab包名称为空");
                return;
            }
            if (obj == null)
            {
                LogHelper.LogError("obj资源为空");
                return;
            }
            if (!mGameObjectDict.ContainsKey(abName))
            {
                mGameObjectDict.Add(abName, obj);
            }
            if (mGameObjectActionDict.ContainsKey(abName)) 
            {
                mGameObjectActionDict[abName]?.Invoke(mGameObjectDict[abName]);
                Remove(abName);
            }
        }
        public static void Register(string abName,Action<T> action)
        {
            if (abName == null)
            {
                LogHelper.LogError("ab包名称为空");
                return;
            }
            if (action == null)
            {
                LogHelper.LogError("action资源为空");
                return;
            }
            if (!mGameObjectActionDict.ContainsKey(abName))
            {
                mGameObjectActionDict.Add(abName, action);
            }
            else
            {
                mGameObjectActionDict[abName] = action;
            }
        }

        public static void Remove(string abName)
        {
            if (abName == null)
            {
                LogHelper.LogError("ab包名称为空");
                return;
            }
            if (mGameObjectActionDict.ContainsKey(abName))
            {
                mGameObjectActionDict[abName] = null;
                mGameObjectActionDict.Remove(abName);
            }
        }
    }
}
