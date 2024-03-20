using UnityEngine;

namespace Game
{
    public static class ApplicationData
    {
        /// <summary>
        /// 项目根地址
        /// </summary>
        public static string ProjectPath { get; private set; } = Application.dataPath.Replace("/Assets", "") + "/XiaoYuanData";
    }
}
