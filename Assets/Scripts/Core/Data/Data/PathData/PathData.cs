using UnityEngine;

namespace Game
{
    /// <summary>
    /// 路径数据
    /// </summary>
    public static class PathData
    {
        public static string ProjectDir { get; private set; } = Application.dataPath.Replace("/Assets","");
    }
}
