using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConversationInput : MonoBehaviour
{
    public void OnPlay(InputValue value)
    {
        ConversationManager.Instance.Play();
    }
}
