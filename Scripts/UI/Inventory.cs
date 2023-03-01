using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : UIBase
{
    public ItemProfilePresenter originItemProfile;

    public GameObject content;
    private List<ItemProfilePresenter> itemList = new List<ItemProfilePresenter>();

    public override async UniTask Initialize()
    {
        var inventory = InventoryManager.Instance.GetInventory();

        if (itemList.Count < inventory.Count)
            itemList.Capacity = inventory.Count;

        int tempMax = Mathf.Max(itemList.Count, inventory.Count);

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
            await itemList[i].Initialize(inventory[i].Value);
            itemList[i].gameObject.SetActive(true);
        }
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }
}
