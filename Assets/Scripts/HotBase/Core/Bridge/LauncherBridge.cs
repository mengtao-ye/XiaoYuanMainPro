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
    public class LauncherBridge : SingletonMono<MonoBehaviour>
    {
        /// <summary>
        /// 启动器的桥梁对象
        /// </summary>
        public static GameObject LauncherTarget;

        private void Awake()
        {
            Debug.Log("LauncherBridge Awake");
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
            Debug.Log(launcherTarget.name);
            LauncherTarget = launcherTarget;
            Debug.Log(GameCenter.Instance == null);
            GameCenter.Instance.Init();
            LogHelper.Log("LauncherBridge LauncherBridge");
        }
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="launcherTarget"></param>
        public void StartGame()
        {
            GameCenter.isRun = true;
            LogHelper.Log("LauncherBridge StartGame");
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="launcherTarget"></param>
        public void StopGame()
        {
            GameCenter.isRun = false;
            LogHelper.Log("LauncherBridge  StopGame");
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

        /// <summary>
        /// 收到解析出来的拼音
        /// </summary>
        /// <param name="target"></param>
        public void ReceivePinYin(char[] target)
        {
            if (target.IsNullOrEmpty() )
            {
                LogHelper.Log("ReceivePinYin对象为空");
                return;
            }
            if (target.Length !=2)
            {
                LogHelper.Log("ReceivePinYin对象格数异常");
                return;
            }
            PinYinTools.ReceivePinYin(target[0], target[1]);
        }

        #endregion
        #region Send
        /// <summary>
        /// 发送异步加载资源请求
        /// </summary>
        public static void SendLoadAsset(ABTagEnum tag,string assetName, LoadAssetType loadAssetType )
        { 
           LauncherTarget?.SendMessage("LoadAsset", tag.ToString() +"&"+ assetName +"&"+ (byte)loadAssetType, UnityEngine.SendMessageOptions.DontRequireReceiver);
        }
        /// <summary>
        /// 发送切换场景请求
        /// </summary>
        /// <param name="sceneName"></param>
        public static void SendLoadScene(string tag, string sceneName) 
        { 
           LauncherTarget?.SendMessage("LoadScene", tag +"&"+sceneName, UnityEngine.SendMessageOptions.DontRequireReceiver);
        }

        public static void SendGetPinYin(string tag, char ch)
        { 
           LauncherTarget?.SendMessage("GetPinYin", tag + "&" + ch, UnityEngine.SendMessageOptions.DontRequireReceiver);
        }
        #endregion
    }
}
