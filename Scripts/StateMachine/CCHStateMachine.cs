using System;
using System.Collections.Generic;
using UnityEngine;

public class CCHStateMachine : MonoBehaviour
{
    [Serializable]
    public class StateDictionary : SerializableDictionary<EState, CCHStateBase> { }

    public enum EState
    {
        None,
        Idle,
        Move,
        Attack,
        Jump,
        CheckGround,
        StopKinematic,
        CameraRotation,
        Search,
        RemainWeapon,
        Look,
        AliveCheck,
        Die,
        IK,
        Swim
    }

    [SerializeField] private StateDictionary stateDictionary;

    [SerializeField]
    private List<CCHStateBase> preStateList = new List<CCHStateBase>();

    [SerializeField]
    private List<CCHStateBase> postStateList = new List<CCHStateBase>();

    public EState startState;
    [SerializeField] private CCHStateBase currentState;
    private EState state = EState.None;
    public EState State
    {
        get { return state; }
        set 
        {
            if (state != value)
            {
                state = value;
                currentState?.OnStateExit();
                currentState = stateDictionary[state];
                if (currentState != null)
                {
                    State = currentState.OnStateEnter();
                }
            }
        }
    }

    private void Start()
    {
        State = startState;
    }

    private void Update()
    {
        if (preStateList.Count > 0)
        {
            for (int i = 0;i < preStateList.Count; ++i)
            {
                preStateList[i].OnStateUpdate();
            }
        }

        if (currentState != null)
            State = currentState.OnStateUpdate();

        if (postStateList.Count > 0)
        {
            for (int i = 0; i < postStateList.Count; ++i)
            {
                postStateList[i].OnStateUpdate();
            }
        }
    }

    private void LateUpdate()
    {
        if (preStateList.Count > 0)
        {
            for (int i = 0; i < preStateList.Count; ++i)
            {
                preStateList[i].OnStateLateUpdate();
            }
        }

        if (currentState != null)
        {
            var updatedState = currentState.OnStateLateUpdate();
            if (updatedState != EState.None)
                State = updatedState;
        }

        if (postStateList.Count > 0)
        {
            for (int i = 0; i < postStateList.Count; ++i)
            {
                postStateList[i].OnStateLateUpdate();
            }
        }
    }
}