using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class State_Enemy_Attack : CCHStateBase
{
    protected EnemyBase owner;

    private void Awake()
    {
        owner = GetComponent<EnemyBase>();
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        owner.animator.SetTrigger("attack");

        return CCHStateMachine.EState.Attack;
    }

    public override void OnStateExit()
    {

    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        if (owner.rangeHitDatas.Count > 0)
            owner.animator.SetBool("move", false);

        if (owner.animator.GetBool("attack"))
            return CCHStateMachine.EState.Attack;
        else
        {
            if (owner.animator.GetBool("move"))
                return CCHStateMachine.EState.Move;
            else
                return CCHStateMachine.EState.Idle;
        }
    }

    public override void Dispose()
    {
        owner = null;
    }
}
