using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootingItemView : MonoBehaviour
{
    public RootItemDataView origin;

    public GameObject searchingRoot;

    private RootItemDataView current;

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

        current = Instantiate(origin, searchingRoot.transform);
        current.Initialize(name);
        current.FadeIn();
        current.gameObject.SetActive(true);
    }
}

