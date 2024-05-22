namespace Game
{
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
        /// <summary>
        /// 获取角色文件地址
        /// </summary>
        /// <param name="sceneABName"></param>
        /// <returns></returns>
        public static string GetRoleData(string roleID, string version)
        {
            return GetOssBucketDir() + "/AssetBundles/Roles/" + roleID + "/role_" + roleID + "@" + version;
        }
        /// <summary>
        /// 获取场景文件地址
        /// </summary>
        /// <param name="sceneABName"></param>
        /// <returns></returns>
        public static string GetSceneData(string sceneABName,string version)
        {
            return GetOssBucketDir() + "/AssetBundles/Scenes/" + sceneABName + "/"+ sceneABName + "@"+ version;
        }
        /// <summary>
        /// 获取角色配置文件地址
        /// </summary>
        /// <param name="sceneABName"></param>
        /// <returns></returns>
        public static string GetRoleConfigData(string roleID)
        {
            return GetOssBucketDir() + "/AssetBundles/Roles/" + roleID + "/config.txt";
        }
        /// <summary>
        /// 获取场景配置文件地址
        /// </summary>
        /// <param name="sceneABName"></param>
        /// <returns></returns>
        public static string GetSceneConfigData(string sceneABName)
        {
            return GetOssBucketDir() + "/AssetBundles/Scenes/" + sceneABName + "/config.txt";
        }
        public static string GetMiniHeadPath(long account)
        {
            return GetOssBucketDir() + "/Images/Heads/" + account+ ".jpg"+ GetSize(50,50);
        }

        public static string GetSchoolIcon(long schoolCode)
        {
            return GetOssBucketDir() + "/Images/SchoolDataImages/" + schoolCode + "/Icon.jpg" ;
        }
        public static string GetCampusCircleImage(string url)
        {
            return GetOssBucketDir() + "/Images/CampusCircle/" + url + ".jpg" ;
        }

        public static string GetSchoolBG(long schoolCode)
        {
            return GetOssBucketDir() + "/Images/SchoolDataImages/" + schoolCode + "/BG.jpg" ;
        }
        public static string GetLostImage(string imageCode)
        {
            return GetOssBucketDir() + "/Images/LostImages/" + imageCode + ".jpg";
        }
        public static string GetUnuseImage(string name) 
        { 
            return GetOssBucketDir() + "/Images/UnuseImages/" + name + ".jpg";
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
