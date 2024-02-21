using UnityEngine;

namespace Game
{
    /// <summary>
    /// 与启动工程之间的桥接脚本
    /// </summary>
    public class LauncherBridgeMono : MonoBehaviour
    {
        public void Init(GameObject launcherTarget)
        {
            if (launcherTarget == null) return;
            Debug.Log("启动LauncherBridgeMono");
        }
    }
}
