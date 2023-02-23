using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class RootingItemPresenter : MonoBehaviour
{
    public RootingItemView view;
    public QuadMapUnit unit;

    private void Start()
    {
        view = CCHGameManager.Instance.rootingItemView;
        CCHGameManager.Instance.currentCharacter.Subscribe(x =>
        { unit = x.GetComponent<QuadMapUnit>(); }).AddTo(this);

        unit.nearUnit.AsObservable().Subscribe(async x =>
        {
            view.Remove();
            if (x != null)
            {
                var nearUnit = unit.nearUnit.Value;
                if (nearUnit.interactionModule is ItemInteraction == false)
                    return;
                var item = nearUnit.interactionModule as ItemInteraction;
                var itemInfo = await ItemInfo.Get(item.itemData.itemId);
                view.Pop(itemInfo.name);
            }
        }).AddTo(this);
    }
}
