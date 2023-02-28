using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemInteraction : InteractionBase
{
    public UserItemData itemData;

    public async UniTask Start()
    {
        if (interactionName == string.Empty)
        {
            var itemInfo = await ItemInfo.Get(itemData.itemId);
            interactionName = itemInfo.name;
        }
    }

    public override void Interaction(QuadMapUnit unit)
    {
        AddItem(unit).Forget();
    }

    private async UniTask AddItem(QuadMapUnit unit)
    {
        await InventoryManager.Instance.Add(itemData);

        ObjectPoolManager.Instance.Push(gameObject);

        unit?.GetAllNeighbor();
    }
}
