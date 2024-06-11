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
        /// 设置主相机
        /// </summary>
        /// <param name="camera"></param>
        public static void SetMainCamera(Camera camera) 
        {
            mainCamera = camera;    
        }
    }
}
