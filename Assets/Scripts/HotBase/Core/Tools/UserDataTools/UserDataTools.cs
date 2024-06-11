namespace Game
{
    public static class UserDataTools
    {
        public static string GetSex(byte sex)
        {
            if (sex == 1) return "男";
            else if (sex == 2) return "女";
            else return "保密";
        }
        public static int GetBrithdayInt(int year, int month, int day)
        {
            return year * 10000 + month * 100 + day;
        }
        public static (int, int, int) ValueToBirthday(int brithday)
        {
            int year = brithday / 10000;
            int month = (brithday / 100) % 100;
            int day = brithday % 100;
            return (year, month, day);
        }
        public static string ValueToBirthdayStr(int brithday)
        {
            if (brithday == 0)
            {
                return "保密";
            }
            else
            {
                int month = (brithday / 100) % 100;
                int day = brithday % 100;
                return month + "/" + day;
            }
        }
    }
}
