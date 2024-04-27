using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class AssetBundleSceneLoader
    {
        AssetBundle bundle;
        public AssetBundleSceneLoader(AssetBundle bundle)
        {
            this.bundle = bundle;
        }
        public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            // 从AssetBundle中加载场景
            string[] scenes = bundle.GetAllScenePaths();
            int sceneIndex = System.Array.FindIndex(scenes, s => s.Contains(sceneName));
            if (sceneIndex >= 0)
            {
                string[] scenePaths = new string[] { scenes[sceneIndex] };
                return SceneManager.LoadSceneAsync(scenePaths[0], mode);
            }
            return null;
        }
    }
}
