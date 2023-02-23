using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemInteraction : InteractionBase
{
    public UserItemData itemData;

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
