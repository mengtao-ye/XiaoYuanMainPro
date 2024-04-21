using System;

namespace Game
{
    public static class MetaSchoolTools
    {
        /// <summary>
        /// 是否是白天
        /// </summary>
        public static bool IsLight 
        {
            get {
                if (DateTime.Now.Hour > 6 && DateTime.Now.Hour < 18)
                    return true;
                return false;
            }
        }
    }
}
