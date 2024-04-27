using UnityEngine;
using System.Collections;
using System;
using YFramework;

namespace Game
{
    public static class AssetBundleTools
    {
        /// <summary>
        /// 加载AB 包场景
        /// </summary>
        /// <param name="sceneLocalPath"></param>
        /// <param name="loadSuccess"></param>
        /// <param name="error"></param>
        public static void LoadScene(string sceneLocalPath, string sceneName,Action loadSuccess, Action<string> error) {
            IEnumeratorModule.StartCoroutine(IELoadScene(sceneLocalPath, sceneName, loadSuccess,error));
        }
        private static IEnumerator IELoadScene(string sceneLocalPath, string sceneName, Action loadSuccess,Action<string> error)
        {
            AssetBundleCreateRequest bundleCreateRequest = AssetBundle.LoadFromFileAsync(sceneLocalPath);
            yield return bundleCreateRequest;
            if (bundleCreateRequest.assetBundle != null)
            {
                // 加载场景
                AssetBundleSceneLoader sceneLoader = new AssetBundleSceneLoader(bundleCreateRequest.assetBundle);
                AsyncOperation asyncLoad = sceneLoader.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                // 等待场景加载完成
                yield return asyncLoad;
                // 加载完成后，可以卸载AssetBundle
                bundleCreateRequest.assetBundle.Unload(false);
                loadSuccess?.Invoke();
            }
            else
            {
         
                error?.Invoke("Failed to load AssetBundle.");
            }
        }
    } 
}

