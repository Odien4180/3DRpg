using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectPool : MonoBehaviour
{
    private GameObject origin;
    private Queue<GameObject> items = new Queue<GameObject>();
    
    public void Initialize(GameObject origin, int poolSize)
    {
        while(items.Count > 0)
        {
            Destroy(items.Dequeue());
        }

        this.origin = origin;
        name = origin.name;

        for (int i = 0; i < poolSize; ++i)
        {
            Add();
        }
    }

    public GameObject Pop(bool isActive = true)
    {
        if (items.Count == 0)
            Add();
        var popItem = items.Dequeue();
        popItem.SetActive(isActive);

        return popItem;
    }

    public GameObject Pop(float autoPushTimer, bool isActive = true)
    {
        var obj = Pop(isActive);

        Push(obj, autoPushTimer).Forget();

        return obj;
    }


    private void Add()
    {
        var copied = Instantiate(origin);
        copied.name = origin.name;
        copied.gameObject.SetActive(false);
        items.Enqueue(copied);
    }
    public async UniTask Push(GameObject returnItem, float time)
    {
        int delay = (int)(time * 1000);
        await UniTask.Delay(delay);
        Push(returnItem);
    }

    public void Push(GameObject returnItem)
    {
        returnItem.SetActive(false);
        items.Enqueue(returnItem);
    }
}

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private Dictionary<string, ObjectPool> poolDic = new Dictionary<string, ObjectPool>();

    private ObjectPool CreatePool(string poolName, GameObject origin, int poolSize)
    {
        ObjectPool pool = gameObject.AddComponent<ObjectPool>();
        pool.Initialize(origin, poolSize);
        poolDic.Add(poolName, pool);

        return pool;
    }

    public async UniTask<GameObject> Get(string type, string name, Action<GameObject> callback = null, int poolSize = 5)
    {
        string noExtensionName = Path.GetFileNameWithoutExtension(name);

        if (poolDic == null)
            poolDic = new Dictionary<string, ObjectPool>();

        if (!poolDic.TryGetValue(noExtensionName, out ObjectPool pool))
        {
            var resourcePath = await AddressableManager.Instance.GetResourcePath(type);
            var loadedAsset = await AddressableManager.Instance.LoadAssetAsync<GameObject>(resourcePath + name);

            if (poolDic.TryGetValue(noExtensionName, out ObjectPool poolSafe))
                pool = poolSafe;
            else
                pool = CreatePool(noExtensionName, loadedAsset, poolSize);
        }

        var popItem = pool.Pop();
        callback?.Invoke(popItem);

        return popItem;
    }

    public async UniTask<T> Get<T>(string type, string name, Action<T> callback = null, int poolSize = 5)
    {
        GameObject popGameObj = await Get(type, name, null, poolSize);
        var returnComp = popGameObj.GetComponent<T>();
        if (callback != null)
            callback(returnComp);
        return returnComp;
    }

    public async UniTask<GameObject> Get(string type, string name, float autoPushTimer, Action<GameObject> callback = null, int poolSize = 5)
    {
        if (poolDic == null)
            poolDic = new Dictionary<string, ObjectPool>();
        string noExtensionName = Path.GetFileNameWithoutExtension(name);
        if (!poolDic.TryGetValue(noExtensionName, out ObjectPool pool))
        {
            var resourcePath = await AddressableManager.Instance.GetResourcePath(type);
            var loadedAsset = await AddressableManager.Instance.LoadAssetAsync<GameObject>(resourcePath + name);
            pool = CreatePool(noExtensionName, loadedAsset, poolSize);
        }
        var popItem = pool.Pop(autoPushTimer);
        callback?.Invoke(popItem);

        return popItem;
    }

    public async UniTask<T> Get<T>(string type, string name, float autoPushTimer, Action<T> callback = null, int poolSize = 5)
    {
        GameObject popGameObj = await Get(type, name, autoPushTimer, null, poolSize);
        var returnComp = popGameObj.GetComponent<T>();
        if (callback != null)
            callback(returnComp);
        return returnComp;
    }

    public void Push(GameObject obj)
    {
        var quadMapUnit = obj.GetComponent<QuadMapUnit>();
        if (quadMapUnit != null)
        {
            quadMapUnit.RemoveUnit();
        }

        if (!poolDic.TryGetValue(obj.name, out ObjectPool pool))
        {
            //반환할 풀이 없어졌다면 오브젝트 제거
            Destroy(obj);
            return;
        }

        pool.Push(obj);
    }
}
