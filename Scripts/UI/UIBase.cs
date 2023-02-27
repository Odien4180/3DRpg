using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    [SerializeField] private bool managing = true;
    public virtual void Initialize() { }

    private void OnEnable()
    {
        if (managing)
            UIManager.Instance.Add(this);
    }

    private void OnDisable()
    {
        if (managing)
            UIManager.Instance.Remove(this);
    }

    public virtual void OnExit()
    {
        gameObject.SetActive(false);
        ObjectPoolManager.Instance.Push(gameObject);
    }
}
