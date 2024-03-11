using UnityEngine;
using YFramework;

namespace Game
{

    public enum LoadAssetType : byte
    { 
        GameObject = 1,
    }

    /// <summary>
    /// 与启动工程之间的桥接脚本
    /// </summary>
    public class LauncherBridgeMono : MonoBehaviour
    {
        /// <summary>
        /// 启动器的桥梁对象
        /// </summary>
        public static GameObject LauncherTarget;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        #region Receive
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="launcherTarget"></param>
        public void Init(GameObject launcherTarget)
        {
            if (launcherTarget == null) return;
            LauncherTarget = launcherTarget;
            GameCenter.Instance.Init();
            LogHelper.Log("初始化应用");
        }
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="launcherTarget"></param>
        public void StartGame()
        {
            GameCenter.isRun = true;
            LogHelper.Log("启动应用");
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="launcherTarget"></param>
        public void StopGame()
        {
            GameCenter.isRun = false;
            LogHelper.Log("暂停应用");
        }

        /// <summary>
        /// 开始切换场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void StartChangeScene(string sceneName)
        {
            LogHelper.Log("开始切换场景:" + sceneName);
        }
        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void ChangeScene(string sceneName)
        {
            GameCenter.Instance.ChangeScene(sceneName);
            LogHelper.Log("切换场景:" + sceneName);
        }
        /// <summary>
        /// 加载GameObject资源为空
        /// </summary>
        /// <param name="gameObject"></param>
        public void LoadGameObject(GameObject gameObject)
        {
            if (gameObject == null)
            {
                LogHelper.Log("LoadGameObject资源为空");
                return;
            }
            LogHelper.Log("加载资源:" + gameObject.name);
            ABResModule<GameObject>.BoardCast(gameObject.name, gameObject);
        }
        #endregion
        #region Send
        /// <summary>
        /// 发送异步加载资源请求
        /// </summary>
        public static void SendLoadAsset(string tag,string assetName, LoadAssetType loadAssetType )
        { 
           LauncherTarget?.SendMessage("LoadAsset", tag +"&"+ assetName +"&"+ (byte)loadAssetType, UnityEngine.SendMessageOptions.DontRequireReceiver);
        }
        /// <summary>
        /// 发送切换场景请求
        /// </summary>
        /// <param name="sceneName"></param>
        public static void SendLoadScene(string tag, string sceneName) 
        { 
           LauncherTarget?.SendMessage("LoadScene", tag +"&"+sceneName, UnityEngine.SendMessageOptions.DontRequireReceiver);
        }
        #endregion
    }
}
