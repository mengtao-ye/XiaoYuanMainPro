using UnityEngine;

namespace Game
{
    public static class MetaSchoolGlobalVarData
    {
        /// <summary>
        /// 主相机
        /// </summary>
        public static Camera mainCamera { get; private set; }
        /// <summary>
        /// 我的校园场景数据
        /// </summary>
        public static MyMetaSchoolData myMetaSchoolData { get; private set; }
        /// <summary>
        /// 我的校园场景数据
        /// </summary>
        public static SchoolData schoolData { get; private set; }
        /// <summary>
        /// 设置校园数据
        /// </summary>
        /// <param name="camera"></param>
        public static void SetSchoolData(SchoolData data)
        {
            schoolData?.Recycle();
            schoolData = data;
        }

        /// <summary>
        /// 设置我的校园数据
        /// </summary>
        /// <param name="camera"></param>
        public static void SetMyMetaSchoolData(MyMetaSchoolData data)
        {
            myMetaSchoolData = data;
        }
        /// <summary>
        /// 设置主相机
        /// </summary>
        /// <param name="camera"></param>
        public static void SetMainCamera(Camera camera) 
        {
            mainCamera = camera;    
        }
    }
}
