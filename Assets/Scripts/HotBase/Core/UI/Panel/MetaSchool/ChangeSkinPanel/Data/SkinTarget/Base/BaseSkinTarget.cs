using System.Collections.Generic;
using UnityEngine;
using YFramework;

namespace Game
{
    public abstract class BaseSkinTarget : ISkinTarget
    {
        public IDictionary<byte, IDictionary<byte,Color>> skinTargetDict { get; private set; }
        protected byte mType;
        public BaseSkinTarget(byte type)
        {
            mType = type;
            skinTargetDict = new Dictionary<byte, IDictionary<byte, Color>>();
            ConfigSkinTargetDic();
        }
        protected abstract void ConfigSkinTargetDic();
        protected void AddSkinTarget(byte key, Dictionary<byte, Color> color) 
        {
            if (skinTargetDict.ContainsKey(key))
            {
                Debug.LogError("skinTargetDict is contains");
                return;
            }
            skinTargetDict.Add(key, color);
        }
    }
}
