namespace Game
{
    public static class PartTimeJobTools
    {
        public static string GetPriceType(byte type)
        {
            switch (type)
            {
                case 0:
                    return "小时";
                case 1:
                    return "天";
                case 2:
                    return "周";
                case 3:
                    return "月";
            }
            return "";
        }
    }
}
