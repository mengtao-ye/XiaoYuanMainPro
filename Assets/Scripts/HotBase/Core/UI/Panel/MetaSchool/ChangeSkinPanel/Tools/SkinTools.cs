namespace Game
{
    public static class SkinTools
    {
        private const string ICON_PARENT_PATH = "Images/Skin";
        private const string TEXTURE_PARENT_PATH = "Textures/Skin";
        private const string MATERIAL_PARENT_PATH = "Materials/Skin";
        private const string MODEL_PARENT_PATH = "Prefabs/Skin";
        public static string GetSkinMatPath() 
        {
            return MATERIAL_PARENT_PATH + "/11/BodySkin@Mat";
        }
        /// <summary>
        /// 获取皮肤贴图地址
        /// </summary>
        /// <param name="type"></param>
        /// <param name="type2"></param>
        /// <param name="type3"></param>
        /// <returns></returns>
        public static string GetSkinTexturePath(byte type, byte type2, byte type3)
        {
            return $"{TEXTURE_PARENT_PATH}/{type}/{type2}/Skin_{type}.{type2}.{type3}@Texture";
        }
        /// <summary>
        /// 获取皮肤图标地址
        /// </summary>
        /// <param name="type"></param>
        /// <param name="type2"></param>
        /// <param name="type3"></param>
        /// <returns></returns>
        public static string GetSkinIconPath(byte type, byte type2, byte type3)
        {
            return $"{ICON_PARENT_PATH}/{type}/{type2}/Skin_{type}.{type2}.{type3}@Icon";
        }
        /// <summary>
        /// 获取皮肤材质地址
        /// </summary>
        /// <param name="type"></param>
        /// <param name="type2"></param>
        /// <param name="type3"></param>
        /// <returns></returns>
        public static string GetSkinMaterialPath(byte type, byte type2, byte type3)
        {
            return $"{MATERIAL_PARENT_PATH}/{type}/{type2}/Skin_{type}.{type2}.{type3}@Mat";
        }

        /// <summary>
        /// 获取发型模型地址
        /// </summary>
        /// <param name="type"></param>
        /// <param name="type2"></param>
        /// <param name="type3"></param>
        /// <param name="hasHat">是否有帽子</param>
        /// <returns></returns>
        public static string GetSkinHairModelPath(byte type, byte type2, byte type3,bool hasHat)
        {
            string hasHatStr = hasHat ? "Headless":"Normal";
            return $"{MODEL_PARENT_PATH}/{type}/{type2}/Skin_{type}.{type2}.1_{hasHatStr}@Model";
        }

        /// <summary>
        /// 获取模型地址
        /// </summary>
        /// <param name="type"></param>
        /// <param name="type2"></param>
        /// <param name="type3"></param>
        /// <returns></returns>
        public static string GetSkinModelPath(byte type, byte type2, byte type3)
        {
            return $"{MODEL_PARENT_PATH}/{type}/{type2}/Skin_{type}.{type2}.{type3}@Model";
        }
    }
}
