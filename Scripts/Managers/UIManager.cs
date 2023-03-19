using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private ReactiveCollection<UIBase> uiCollection = new ReactiveCollection<UIBase>();
    private GameObject uiBlur;

    private new void Awake()
    {
        base.Awake();
        uiCollection.ObserveAdd().Subscribe(x => {
            ChangeInputSetting();
        });
        uiCollection.ObserveRemove().Subscribe(x => {
            ChangeInputSetting();
        });
    }

    private async UniTask AddBlur()
    {
        if (uiBlur == null)
        {
            await AddressableManager.Instance.InstantiateAddressableAsync("UI", "Blur.prefab", x =>
            {
                if (uiBlur != null)
                {
                    Destroy(x);
                }
                else
                {
                    uiBlur = x;
                }
            });
        }

        if (uiCollection.Count > 0)
            uiBlur.SetActive(true);
    }

    private void RemoveBlur()
    {
        if (uiBlur != null)
            uiBlur.gameObject.SetActive(false);
    }

    public void Add(UIBase item)
    {
        if (uiCollection.Contains(item))
            return;

        uiCollection.Add(item);
        
        AddBlur().Forget();
    }

    public void Remove(UIBase item)
    {
        if (uiCollection.Contains(item) == false)
            return;

        uiCollection.Remove(item);

        if (uiCollection.Count == 0)
            RemoveBlur();
    }

    public void RemoveLast()
    {
        if (uiCollection.Count == 0)
            return;

        uiCollection.Last().OnExit();
    }

    private void ChangeInputSetting()
    {
        bool isEnableExist = false;
        for (int i = 0; i < uiCollection.Count; i++)
        {
            if (uiCollection[i].enabled)
            {
                isEnableExist = true;
                break;
            }
        }

        if (isEnableExist)
        {
            CCHGameManager.Instance.SwitchActionMap("UI");
            CCHGameManager.Instance.SetTimeScale(0.0f);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            CCHGameManager.Instance.SwitchActionMap("Player");
            CCHGameManager.Instance.SetTimeScale(1.0f);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
