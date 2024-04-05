namespace Game
{
    //https://xiaoyuan-test-data.oss-cn-hangzhou.aliyuncs.com/Images/Heads/18379366314.jpg?x-oss-process=image/resize,w_100,h_100
    public static class OssPathData
    {
        public static string GetOssBucketDir() 
        {
            switch (AppData.platformType)
            {
                case PlatformType.Test:
                    return @"https://xiaoyuan-test-data.oss-cn-hangzhou.aliyuncs.com";
                case PlatformType.Pre:
                    return @"https://xiaoyuan-pre-data.oss-cn-hangzhou.aliyuncs.com";
                case PlatformType.Pro:
                    return @"https://xiaoyuan-pro-data.oss-cn-hangzhou.aliyuncs.com";
            }
            return null;
        }
        public static string GetMiniHeadPath(long account)
        {
            return GetOssBucketDir() + "/Images/Heads/" + account+ ".jpg"+ GetSize(50,50);
        }

        public static string GetMiniSchoolIcon(long schoolCode)
        {
            return GetOssBucketDir() + "/Images/SchoolDataImages/" + schoolCode + "/Icon.jpg" + GetSize(50, 50);
        }
        public static string GetMiniCampusCircleImage(string url,float width, float height)
        {
            return GetOssBucketDir() + "/Images/CampusCircle/" + url + ".jpg" + GetSize(width, height);
        }

        public static string GetSchoolBG(long schoolCode)
        {
            return GetOssBucketDir() + "/Images/SchoolDataImages/" + schoolCode + "/BG.jpg" ;
        }
        /// <summary>
        /// 获取图片尺寸
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string GetSize(float width,float height) 
        {
            return  $"?x-oss-process=image/resize,w_{(int)width},h_{(int)height}";
        }

    }
}
