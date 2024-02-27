using YFramework;

namespace Game
{
    /// <summary>
    /// Unity 内置打印消息
    /// </summary>
    public class UnityLogHelper : LogHelper
    {
        protected override void LogErrorMsg<T>(T msg)
        {
            UnityEngine.Debug.LogError( ABTag.Main+":"+ msg.ToString());
        }

        protected override void LogMsg<T>(T msg)
        {
            UnityEngine.Debug.Log(ABTag.Main + ":" + msg.ToString());
        }

        protected override void LogWarningMsg<T>(T msg)
        {
            UnityEngine.Debug.LogWarning(ABTag.Main + ":" + msg.ToString());
        }
    }
}
