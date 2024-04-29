using UnityEngine;
using System.Collections;
using System;
using YFramework;

namespace Game
{
    public static class AssetBundleTools
    {
        /// <summary>
        /// ����AB ����ɫ
        /// </summary>
        /// <param name="roleLocalPath"></param>
        /// <param name="loadSuccess"></param>
        /// <param name="error"></param>
        public static void LoadRole(string roleLocalPath, string roleName, Action<GameObject> loadSuccess, Action<string> error)
        {
            IEnumeratorModule.StartCoroutine(IELoadRole(roleLocalPath, roleName, loadSuccess, error));
        }
        private static IEnumerator IELoadRole(string sceneLocalPath, string sceneName, Action<GameObject> loadSuccess, Action<string> error)
        {
            AssetBundleCreateRequest bundleCreateRequest = AssetBundle.LoadFromFileAsync(sceneLocalPath);
            yield return bundleCreateRequest;
            if (bundleCreateRequest.assetBundle != null)
            {
               AssetBundleRequest assetBundleReq = bundleCreateRequest.assetBundle.LoadAssetAsync<GameObject>("role_"+sceneName);
                // �ȴ���ɫ�������
                yield return assetBundleReq;
                // ������ɺ󣬿���ж��AssetBundle
                bundleCreateRequest.assetBundle.Unload(false);
                loadSuccess?.Invoke(assetBundleReq.asset as GameObject);
            }
            else
            {
                error?.Invoke("Failed to load AssetBundle.");
            }
        }


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

