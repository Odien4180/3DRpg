using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class State_Player_Idle : CCHStateBase
{
    private PlayerCharacterController owner;
    private float idleTimer = 0.0f;
    private void Awake()
    {
        owner = GetComponent<PlayerCharacterController>();
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        return CCHStateMachine.EState.Idle;
    }

    public override void OnStateExit()
    {
        
    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        if (owner.isWater)
            return CCHStateMachine.EState.Swim;

        var input = CCHGameManager.Instance.Input;

        if (input.mainAttack && owner.onGround)
            return CCHStateMachine.EState.Attack;

        if (input.move != Vector2.zero) 
            return CCHStateMachine.EState.Move;

        owner.characterController.Move(
            new Vector3(0, owner.verticalVelocity, 0) * Time.deltaTime);

        if (owner.onGround == false)
            idleTimer = 0.0f;

        idleTimer += Time.deltaTime;

        if (idleTimer > owner.idleMotionTimer)
        {
            ////대기 모션 랜덤 재생 추가
            idleTimer = 0.0f;
        }

        return CCHStateMachine.EState.Idle;
    }

    public override void Dispose()
    {
        owner = null;
    }
}
