using System;
using System.Collections.Generic;

namespace Game
{
    public static class PinYinTools
    {
        private static Dictionary<char, char> mPinYinDict = new Dictionary<char, char>();
        private static Dictionary<char, Action<char>> mActionDict = new Dictionary<char, Action<char>>();
        /// <summary>
        /// 获取拼音首字符
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="action"></param>
        public static void GetPinYin(char ch,Action<char> action)
        {
            if (mPinYinDict.ContainsKey(ch))
            {
                action?.Invoke(mPinYinDict[ch]);
                return;
            }
            mActionDict[ch] = action;
#if UNITY_EDITOR
            char[] chs = new char[] { 'a', 'b', 'c', 'd', 'e', 'f',PinYinConstData.DEFAULT };
            ReceivePinYin(ch, chs[UnityEngine.Random.Range(0, chs.Length)]);
#else
             LauncherBridge.SendGetPinYin(ABTagEnum.Main.ToString(), ch);
#endif
        }
        /// <summary>
        /// 接收到汉字首字符
        /// </summary>
        /// <param name="target"></param>
        /// <param name="pinYin"></param>
        public static void ReceivePinYin(char target,char pinYin) 
        {
            if (!mPinYinDict.ContainsKey(target)) 
            {
                mPinYinDict.Add(target,pinYin);
            }
            if (mActionDict.ContainsKey(target))
            {
                mActionDict[target]?.Invoke(pinYin);
            }
        }
        public static int YinPinCodeToIndex(char ch)
        {
            return (byte)ch - 97;
        }
        public static char YinPinCodeToIndex(int charCode)
        {
            return (char)charCode;
        }
    }
}
