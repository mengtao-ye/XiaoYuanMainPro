using System;
using System.Text;
using UnityEngine;
using YFramework;

namespace Game
{
    public class UniqueCodeGenerator
    {
        private static System. Random mRandom = new System.Random();
        private static long mMinValue = 1;
        public static string GenerateUniqueCode()
        {
            // 生成一个新的GUID
            Guid guid = Guid.NewGuid();
            // 将GUID转换为字符串，去掉括号
            string uniqueCode = guid.ToString("N");
            return uniqueCode;
        }
        public static long GenerateMyUniqueCodeLong()
        {
            long maxValue = long.MaxValue;
            long minValue = 1;
            byte[] buf = new byte[8];
            mRandom.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);
            // 确保生成的随机数在指定范围内
            long absLongRand = Math.Abs(longRand);
            long randomLong = longRand < 0 ? (longRand % (maxValue - minValue)) - (absLongRand % (maxValue - minValue)) : longRand % (maxValue - minValue) + minValue;
            return randomLong;
        }
        public static string GenerateMyUniqueCodeStr()
        {
            return GenerateMyUniqueCodeLong().ToString();
        }
    }

}