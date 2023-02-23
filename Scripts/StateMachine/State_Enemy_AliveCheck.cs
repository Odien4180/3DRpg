using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Enemy_AliveCheck : CCHStateBase
{
    private EnemyBase owner;
    private CCHStateMachine stateMachine;

    private bool lastCheck;

    private void Awake()
    {
        owner = GetComponent<EnemyBase>();
        stateMachine = GetComponent<CCHStateMachine>();

        lastCheck = owner.alive;
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        return CCHStateMachine.EState.AliveCheck;
    }

    public override void OnStateExit()
    {
        
    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        if (owner.alive == false && lastCheck != owner.alive)
        {
            stateMachine.State = CCHStateMachine.EState.Die;
        }

        lastCheck = owner.alive;

        return CCHStateMachine.EState.AliveCheck;
    }

    public override void Dispose()
    {
        owner = null;
        stateMachine = null;
    }
}
