using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EStateMachine
{
    ENTER,
    EXIT,
    STATE_MACHINE_ENTER,
    STATE_MACHINE_EXIT
}

[Serializable]
public struct StateMachineData
{
    public string key;
    public int id;
    public bool value;
    public EStateMachine state;
}

[Serializable]
public struct SendMessageData
{
    public string message;
    public EStateMachine state;
    public bool isParent;
}

public class ComboSystem : StateMachineBehaviour
{
    public StateMachineData[] datas;
    public SendMessageData[] sendMessages;

    private void Awake()
    {
        if (datas.Length == 0)
        {
            Destroy(this);
            return;
        }

        for (int i = 0; i < datas.Length; ++i)
        {
            datas[i].id = Animator.StringToHash(datas[i].key);
        }

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < datas.Length; ++i)
        {
            if (datas[i].state == EStateMachine.ENTER)
            {
                animator.SetBool(datas[i].id, datas[i].value);
            }
        }

        for (int i = 0; i < sendMessages.Length; ++i)
        {
            if (sendMessages[i].state == EStateMachine.ENTER)
            {
                if (sendMessages[i].isParent)
                    animator.transform.parent.gameObject.SendMessage(sendMessages[i].message);
                else
                    animator.gameObject.SendMessage(sendMessages[i].message);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for(int i = 0; i < datas.Length; ++i)
        {
            if (datas[i].state == EStateMachine.EXIT)
            {
                animator.SetBool(datas[i].id, datas[i].value);
            }
        }

        for (int i = 0; i < sendMessages.Length; ++i)
        {
            if (sendMessages[i].state == EStateMachine.EXIT)
            {
                if (sendMessages[i].isParent)
                    animator.transform.parent.gameObject.SendMessage(sendMessages[i].message);
                else
                    animator.gameObject.SendMessage(sendMessages[i].message);
            }
        }
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        for (int i = 0; i < datas.Length; ++i)
        {
            if (datas[i].state == EStateMachine.STATE_MACHINE_ENTER)
            {
                animator.SetBool(datas[i].id, datas[i].value);
            }
        }

        for (int i = 0; i < sendMessages.Length; ++i)
        {
            if (sendMessages[i].state == EStateMachine.STATE_MACHINE_ENTER)
            {
                if (sendMessages[i].isParent)
                    animator.transform.parent.gameObject.SendMessage(sendMessages[i].message);
                else
                    animator.gameObject.SendMessage(sendMessages[i].message);
            }
        }
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        for (int i = 0; i < datas.Length; ++i)
        {
            if (datas[i].state == EStateMachine.STATE_MACHINE_EXIT)
            {
                animator.SetBool(datas[i].id, datas[i].value);
            }
        }

        for (int i = 0; i < sendMessages.Length; ++i)
        {
            if (sendMessages[i].state == EStateMachine.STATE_MACHINE_EXIT)
            {
                if (sendMessages[i].isParent)
                    animator.transform.parent.gameObject.SendMessage(sendMessages[i].message);
                else
                    animator.gameObject.SendMessage(sendMessages[i].message);
            }
        }
    }
}
