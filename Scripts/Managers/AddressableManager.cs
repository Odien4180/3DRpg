using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;


[Serializable]
public struct ResourcePathArray
{
    public ResourcePath[] paths;
}

[Serializable]
public struct ResourcePath
{
    public string type;
    public string path;
}

public class AddressableManager : Singleton<AddressableManager>
{
    private Dictionary<string, string> resourcePathDic = new Dictionary<string, string>();

    private async UniTask LoadResourceMap()
    {
        var loadAsset = Addressables.LoadAssetAsync<TextAsset>("Assets/AddressableResources/ResourceMap.json");
        await loadAsset;
        
        LoadResourceMap(loadAsset.Result);
    }

    private void LoadResourceMap(TextAsset textAsset)
    {
        lock (resourcePathDic)
        {
            var resourcePathArray = JsonUtility.FromJson<ResourcePathArray>(textAsset.ToString());

            for (int i = 0; i < resourcePathArray.paths.Length; ++i)
            {
                if (resourcePathDic.ContainsKey(resourcePathArray.paths[i].type) == false)
                    resourcePathDic.Add(resourcePathArray.paths[i].type,
                        resourcePathArray.paths[i].path);
            }
        }
    }

    public async UniTask<string> GetResourcePath(string type)
    {
        if (resourcePathDic.TryGetValue(type, out string path))
        {
            return path;
        }
        else
        {
            //찾는 리소스 경로 정보 없으면 경로 관련 파일 다시 로드 후 한번만 재시도
            await LoadResourceMap();
            if (resourcePathDic.TryGetValue(type, out string rePath))
            {
                return rePath;
            }
        }
        
        return string.Empty;
    }

    public async UniTask<T> LoadAddressableAsync<T>(
        string type, string name, Action<T> callback = null)
    {
        var resourcePath = await GetResourcePath(type);
        var loadAsset = await Addressables.LoadAssetAsync<T>(resourcePath + name);
        return loadAsset;
    }

    public async UniTask<T> InstantiateAddressableAsync<T>(
        string type, string name, Action<T> callback = null)
    {
        GameObject popGameObj = await InstantiateAddressableAsync(type, name, null);

        var returnComp = popGameObj.GetComponent<T>();
        if (callback != null)
            callback(returnComp);

        return returnComp;
    }

    public async UniTask<GameObject> InstantiateAddressableAsync(
        string type, string name, Action<GameObject> callback = null)
    {
        var resourcePath = await GetResourcePath(type);
        var loadAsset = await Addressables.LoadAssetAsync<GameObject>(resourcePath + name);
        var copiedObj = Instantiate(loadAsset);

        callback?.Invoke(copiedObj);

        return copiedObj;
    }

    public async UniTask DownloadByTag(string tag, Action<float> onWait = null, 
        Action onComplete = null, float minDeltaTime = 0.0f)
    {
        var downloadOperation = Addressables.DownloadDependenciesAsync(tag);
        float spendTime = 0.0f;

        while (downloadOperation.IsDone == false)
        {
            onWait?.Invoke(downloadOperation.GetDownloadStatus().Percent);
            spendTime += Time.deltaTime;

            await UniTask.NextFrame();
        }

        onComplete?.Invoke();

        while (spendTime < minDeltaTime)
        {
            spendTime += Time.deltaTime;
            await UniTask.NextFrame();
        }
    }

    public async UniTask LoadByTag(string tag, Action<float> onWait = null, 
        Action onComplete = null, float minDeltaTime = 0.0f)
    {
        var locations = await Addressables.LoadResourceLocationsAsync(tag);
        var loadOperation = Addressables.LoadAssetsAsync<UnityEngine.Object>(locations, null);
        float spendTime = 0.0f;

        while (loadOperation.IsDone == false)
        {
            onWait?.Invoke(loadOperation.GetDownloadStatus().Percent);
            spendTime += Time.deltaTime;

            await UniTask.NextFrame();
        }

        onComplete?.Invoke();


        while (spendTime < minDeltaTime)
        {
            spendTime += Time.deltaTime;
            await UniTask.NextFrame();
        }
    }

    public async UniTask LoadAll(string tag)
    {
        var downloadOperation = Addressables.DownloadDependenciesAsync("Must");
        while (downloadOperation.IsDone == false)
        {
            Debug.Log("Download Must");
            await UniTask.NextFrame();
        }

        downloadOperation = Addressables.DownloadDependenciesAsync("Conversation");
        while (downloadOperation.IsDone == false)
        {
            Debug.Log("Download Conversation");
            await UniTask.NextFrame();
        }

        var locations = await Addressables.LoadResourceLocationsAsync("Must");
        await Addressables.LoadAssetsAsync<UnityEngine.Object>(locations, null);
        
        locations = await Addressables.LoadResourceLocationsAsync("Conversation");
        var convAssets = await Addressables.LoadAssetsAsync<TextAsset>(locations, null);
        
        for (int i = 0; i < convAssets.Count; ++i)
        {
            Conversations.Clear();
            Conversations.Add(convAssets[i].name, convAssets[i]);
        }

        SceneManager.LoadScene("TestScene");
    }

    public async UniTask LoadConversations()
    {
        var locations = await Addressables.LoadResourceLocationsAsync("Conversation");
        var convAssets = await Addressables.LoadAssetsAsync<TextAsset>(locations, null);
        
        for (int i = 0; i < convAssets.Count; ++i)
        {
            Conversations.Clear();
            Conversations.Add(convAssets[i].name, convAssets[i]);
        }
        //Conversation 정보는 따로 저장 해두기 때문에 릴리즈 시켜도 상관없음
        Addressables.Release(convAssets);
    }
}
