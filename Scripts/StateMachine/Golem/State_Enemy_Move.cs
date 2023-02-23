using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class State_Enemy_Move : CCHStateBase
{
    protected EnemyBase owner;

    private void Awake()
    {
        owner = GetComponent<EnemyBase>();
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        owner.animator.SetBool("move", true);

        return CCHStateMachine.EState.Move;
    }

    public override void OnStateExit()
    {
        owner.agent.ResetPath();
    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        var targetPos = owner.Target.transform.position;
        var eyePos = owner.eye.transform.position;
        
        float sqrDistance = (targetPos - eyePos).sqrMagnitude;

        if (sqrDistance > owner.sight * owner.sight)
        {
            owner.animator.SetBool("move", false);
            owner.isCombat = false;
            return CCHStateMachine.EState.Idle;
        }

        owner.currentDelay += Time.deltaTime;

        if (owner.rangeHitDatas.Count > 0)
        {
            if (owner.currentDelay >= owner.attackDelay)
            {
                owner.currentDelay = 0.0f;
                return CCHStateMachine.EState.Attack;
            }
            else
                return CCHStateMachine.EState.Idle;
        }
        else
        {
            owner.agent.SetDestination(owner.Target.transform.position);

            return CCHStateMachine.EState.Move;
        }
    }

    public override void Dispose()
    {
        owner = null;
    }
}
