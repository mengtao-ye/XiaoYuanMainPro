using YFramework;

namespace Game
{
    public class SceneMapper: Map<string, IScene>
    {
        protected override void Config()
        {
           
        }
        /// <summary>
        /// 注册场景
        /// </summary>
        /// <param name="scene"></param>
        private void AddScene(IScene scene) {
            Add(scene.sceneName,scene);
        }
    }
}
