using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : UIBase
{
    public ItemProfilePresenter originItemProfile;

    public GameObject content;
    private List<ItemProfilePresenter> itemList = new List<ItemProfilePresenter>();

    [Header("아이템 하나 표시될 때 걸리는 시간")]
    //millisecond
    public int showItemTimeMs = 50;

    public override async UniTask Initialize()
    {
        var inventory = InventoryManager.Instance.GetInventory();

        if (itemList.Count < inventory.Count)
            itemList.Capacity = inventory.Count;

        int tempMax = Mathf.Max(itemList.Count, inventory.Count);

        List<UniTask> tasks = new List<UniTask>();
        for (int i = 0; i < tempMax; ++i)
        {
            if (i >= inventory.Count)
            {
                itemList[i].gameObject.SetActive(false);
                continue;
            }

            if (i >= itemList.Count)
            {
                itemList.Add(Instantiate(originItemProfile, content.transform));
            }
            //itemList[i].gameObject.SetActive(false);
            //await itemList[i].Initialize(inventory[i].Value);
            //itemList[i].gameObject.SetActive(true);
            tasks.Add(itemList[i].Initialize(inventory[i].Value, false));
        }

        await UniTask.WhenAll(tasks);

        for (int i = 0; i < inventory.Count; ++i)
        {
            itemList[i].gameObject.SetActive(true);
            await UniTask.Delay(showItemTimeMs, true);
        }

    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }
}
