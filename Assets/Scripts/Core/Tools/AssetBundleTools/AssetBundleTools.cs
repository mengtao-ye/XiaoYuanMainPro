using UnityEngine;
using System.Collections;
using System;
using YFramework;

namespace Game
{
    public static class AssetBundleTools
    {
        /// <summary>
        /// ����AB ������
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
                // ���س���
                AssetBundleSceneLoader sceneLoader = new AssetBundleSceneLoader(bundleCreateRequest.assetBundle);
                AsyncOperation asyncLoad = sceneLoader.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                // �ȴ������������
                yield return asyncLoad;
                // ������ɺ󣬿���ж��AssetBundle
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

