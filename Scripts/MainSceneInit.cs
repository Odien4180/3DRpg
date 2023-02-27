using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MainSceneInit : MonoBehaviour
{
    private AddItemView addItemView;

    void Start()
    {
        SetObjects().Forget();
    }

    private async UniTask SetObjects()
    {
        addItemView = await AddressableManager.Instance.InstantiateAddressableAsync<AddItemView>("UI", "AddItemUI.prefab");

        InventoryManager.Instance.AddSubject.Subscribe(async x => await PopAddItem(x)).AddTo(this);

        await CCHGameManager.Instance.LoadCharacter();
        await CCHGameManager.Instance.LoadRootingItemView();
        await CCHGameManager.Instance.LoadInventory();
    }

    public async UniTask PopAddItem(UserItemData userItemData)
    {
        ItemData itemData = await ItemInfo.Get(userItemData.itemId);

        addItemView.Remove();
        addItemView.Pop(itemData.name);
    }
}
