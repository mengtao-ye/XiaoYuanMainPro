using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public static class BoardCastID
    {
        public const int Click = 1;//屏幕点击
        public const int LostLine = 2;//玩家掉线
        public const int ConnectLine = 3;//网络连接成功
        public const int GetMetaSchoolData = 4;//获取校园数据


        private static int UniqueStartIndex = 100000000;
        public static int GetUniqueID()
        {
            return UniqueStartIndex++;
        }

    }
}
