using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System;

public class InteractionPresenter : MonoBehaviour
{
    public InteractionView view;
    public QuadMapUnit unit;

    private IDisposable currentRx;

    private void Start()
    {
        view = CCHGameManager.Instance.rootingItemView;
        CCHGameManager.Instance.currentCharacter.Subscribe(x =>
        { unit = x.GetComponent<QuadMapUnit>(); SetRx(); }).AddTo(this);
    }

    private void SetRx()
    {
        currentRx?.Dispose();
        currentRx = unit.nearUnit.AsObservable().Subscribe(x =>
        {
            view.Remove();
            if (x != null)
            {
                var nearUnit = unit.nearUnit.Value;
                view.Pop(nearUnit.interactionModule.interactionName);
            }
        }).AddTo(this);
    }
}
