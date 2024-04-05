namespace Game
{
    public static class StringTools
    {
        public static string ConverterCount(int count)
        {
            if (count < 1000) 
            {
                return count.ToString();
            }
            else 
            {
                int k = count / 1000;
                int h = (count / 100) % 10 ;
                return $"{k}.{h}k";
            }
        }
    }
}