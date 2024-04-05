using System;

namespace Game
{
    public class UniqueCodeGenerator
    {
        public static string GenerateUniqueCode()
        {
            // 生成一个新的GUID
            Guid guid = Guid.NewGuid();

            // 将GUID转换为字符串，去掉括号
            string uniqueCode = guid.ToString("N");

            return uniqueCode;
        }
    }

}