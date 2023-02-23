using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AddItemView : MonoBehaviour
{
    public RootItemDataView origin;

    public GameObject addItemRoot;
    public float viewTime = 1.0f;

    private RootItemDataView current;

    private IDisposable timer;

    public void Remove()
    {
        if (current == null)
            return;

        current.FadeOut();
        current = null;
    }

    public void Pop(string name)
    {
        if (current != null)
            return;

        current = Instantiate(origin, addItemRoot.transform);
        current.Initialize(name);
        current.FadeIn();
        current.gameObject.SetActive(true);

        timer?.Dispose();
        timer = Observable.Timer(TimeSpan.FromSeconds(viewTime))
            .Subscribe(x => Remove()).AddTo(this);
    }
}
