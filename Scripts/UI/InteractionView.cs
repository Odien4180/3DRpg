using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionView : MonoBehaviour
{
    public InteractionPopUI origin;

    public GameObject searchingRoot;

    private InteractionPopUI current;

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

