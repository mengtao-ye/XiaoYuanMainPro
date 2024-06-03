using System.IO;
using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public static class PlayerPrefsModule
    {
        private static string LocalPath = PathData.ProjectDataPath + "/PlayerPrefsData/";

        public static void Set(string key, byte[] value)
        {
            if (key.IsNullOrEmpty())
            {
                Debug.LogError("PlayerPrefsModule.Set(string key,string value) key is null");
                return;
            }
            if (value.IsNullOrEmpty())
            {
                Debug.LogError("PlayerPrefsModule.Set(string key,string value) value is null");
                return;
            }
            string path = LocalPath + key;
            FileTools.Write(path, value);
        }
        public static void Set(string key,string value)
        {
            Set(key,value.ToBytes());
        }

        public static byte[] GetBytes(string key)
        {
            if (key.IsNullOrEmpty())
            {
                Debug.LogError("PlayerPrefsModule.Get(string key) key is null");
                return null;
            }
            string path = LocalPath + key;
            if (Contains(key))
            {
                return File.ReadAllBytes(path);
            }
            else
            {
                return null;
            }
        }

        public static string GetString(string key)
        {
            if (key.IsNullOrEmpty())
            {
                Debug.LogError("PlayerPrefsModule.Get(string key) key is null");
                return null;
            }
            string path = LocalPath + key;
            if (Contains(key))
            {
                return File.ReadAllText(path);
            }
            else 
            {
                return null;
            }
        }

        public static bool Contains(string key) 
        {
            if (key.IsNullOrEmpty())
            {
                Debug.LogError("PlayerPrefsModule.Contains(string key)  key is null");
                return false;
            }
            string path = LocalPath + key;
            return File.Exists(path);
        }
    }
}
