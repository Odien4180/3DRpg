using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEditor;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    private Dictionary<int, UserItemData> equipInventory = new Dictionary<int, UserItemData>();
    private Dictionary<int, UserItemData> inventory = new Dictionary<int, UserItemData>();

    private Subject<UserItemData> addSubject = new Subject<UserItemData>();
    public Subject<UserItemData> AddSubject => addSubject;

    private void Awake()
    {
        base.Awake();

        LoadTempUserItemData();
    }

    public async UniTask Add(UserItemData itemData)
    {
        int itemId = itemData.itemId;

        ItemData iData = await ItemInfo.Get(itemId);
        switch (iData.type)
        {
            case EItemType.None:
                break;
            case EItemType.Equip:
                var userItemData = new UserItemData()
                {
                    //추후 수정 필요
                    userItemId = 0,
                    count = 1
                };
                equipInventory.Add(userItemData.userItemId, userItemData);
                break;
            case EItemType.Ingredient:
                if (inventory.ContainsKey(itemId) == false)
                {
                    inventory.Add(itemId, itemData);
                }
                else
                {
                    inventory[itemId].count += itemData.count;
                }
                break;
        }

        addSubject.OnNext(itemData);
    }

    public List<KeyValuePair<int, UserItemData>> GetEquipInventory()
    {
        var inventoryList = equipInventory.ToList();

        return inventoryList;
    }

    public List<KeyValuePair<int, UserItemData>> GetInventory()
    {
        var inventoryList = inventory.ToList();

        return inventoryList;
    }

    [Obsolete("인벤토리 기능 테스트를 위해 임시로 사용할 로컬 데이터 로드 함수")]
    public void LoadTempUserItemData()
    {
        var inventoryData = Resources.Load<TempInventoryData>("Temp Inventory Data");
        foreach (var data in inventoryData.dataList)
        {
            Add(data).Forget();
        }

    }
}
