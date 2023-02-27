using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInput : MonoBehaviour
{
    public void OnPlay(InputValue value)
    {
        ConversationManager.Instance.Play();
    }

    public void OnUIRemove(InputValue value)
    {
        UIManager.Instance.RemoveLast();
    }
}
