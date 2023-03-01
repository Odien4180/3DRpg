using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemProfileView))]
public class ItemProfilePresenter : MonoBehaviour
{
    public ItemProfileView itemView;

    private void Start()
    {
        if (itemView == null)
            itemView= GetComponent<ItemProfileView>();
    }

    public async UniTask Initialize(UserItemData data)
    {
        itemView.countText.text = data.count.ToString();
        var itemInfo = await ItemInfo.Get(data.itemId);
        var iconLoad = await AddressableManager.Instance.LoadAddressableAsync<Sprite>(Const.type_icon, itemInfo.icon);
        itemView.iconImage.sprite = iconLoad;

        var frameLoad = await AddressableManager.Instance.LoadAddressableAsync<Sprite>(Const.type_frame, $"{itemInfo.grade}.png");
        itemView.frameImage.sprite = frameLoad;
    }
}
