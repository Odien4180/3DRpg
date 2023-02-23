using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


[Serializable]
public struct ItemDataArray
{
    public ItemData[] datas;
}

public enum EItemType
{
    None,
    Equip,
    Ingredient
}

[Serializable]
public struct ItemData
{
    public int id;
    public EItemType type;
    public int grade;
    public string icon;
    public string name;
}

[Serializable]
public class UserItemData
{
    public int itemId;
    public int userItemId;
    public int count;
}

public static class ItemInfo
{
    private static Dictionary<int, ItemData> itemDataDictionary = new Dictionary<int, ItemData>();

    public static async UniTask LoadItemDatas()
    {
        var loadAsset = Addressables.LoadAssetAsync<TextAsset>("Assets/AddressableResources/ItemData.json");
        await loadAsset;

        LoadItemDatas(loadAsset.Result);
    }

    private static void LoadItemDatas(TextAsset textAsset)
    {
        lock (itemDataDictionary)
        {
            var itemDataArray = JsonUtility.FromJson<ItemDataArray>(textAsset.ToString());

            for (int i = 0; i < itemDataArray.datas.Length; ++i)
            {
                if (itemDataDictionary.ContainsKey(itemDataArray.datas[i].id) == false)
                    itemDataDictionary.Add(itemDataArray.datas[i].id,
                        itemDataArray.datas[i]);
            }
        }
    }

    public static async UniTask<ItemData> Get(int id)
    {
        if (itemDataDictionary.ContainsKey(id) == false)
        {
            await LoadItemDatas();
        }

        return itemDataDictionary[id];
    }
}
