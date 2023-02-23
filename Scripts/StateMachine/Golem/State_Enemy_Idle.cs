using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Enemy_Idle : CCHStateBase
{
    private EnemyBase owner;

    private void Awake()
    {
        owner = GetComponent<EnemyBase>();
    }

    private void Start()
    {
        owner.animator.SetFloat("speed", owner.speed);
        owner.agent.speed = owner.speed;
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        if (owner.animator.GetBool("move"))
            return CCHStateMachine.EState.Move;
        else
        {
            owner.animator.SetBool("move", false);
            return CCHStateMachine.EState.Idle;
        }
    }

    public override void OnStateExit()
    {

    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        if (owner.Target == null) 
            return CCHStateMachine.EState.Idle;

        owner.currentDelay += Time.deltaTime;

        if (owner.isCombat)
        {
            if (owner.rangeHitDatas.Count > 0)
            {
                if (owner.currentDelay > owner.attackDelay)
                {
                    owner.currentDelay = 0.0f;
                    return CCHStateMachine.EState.Attack;
                }
                else
                    return CCHStateMachine.EState.Idle;
            }
            else
                return CCHStateMachine.EState.Move;
        }
        else
        {
            if (owner.nearestFovHitData == null)
            {
                return CCHStateMachine.EState.Idle;
            }
            else
            {
                owner.isCombat = true;
                if (owner.rangeHitDatas.Count > 0)
                    return CCHStateMachine.EState.Attack;
                else
                    return CCHStateMachine.EState.Move;
            }
        }
    }

    public override void Dispose()
    {
        owner = null;
    }
}
