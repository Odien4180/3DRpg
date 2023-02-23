using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Player_Jump : CCHStateBase, IDisposable
{
    private PlayerCharacterController owner;
    private float fallTimeDelta;
    private float jumpTimeDelta;

    private void Awake()
    {
        owner = GetComponent<PlayerCharacterController>();
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        return CCHStateMachine.EState.Jump;
    }

    public override void OnStateExit()
    {
        
    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        if (owner.isWater)
            return CCHStateMachine.EState.Jump;

        var input = CCHGameManager.Instance.Input;
        
        if (owner.onGround)
        {
            fallTimeDelta = owner.fallMaxTime;

            owner.animator.SetBool("jump", false);
            owner.animator.SetBool("fall", false);

            //낙하속도 최대치
            if (owner.verticalVelocity < 0.0f)
            {
                owner.verticalVelocity = -2f;
            }

            //점프 시작
            if (input.jump && jumpTimeDelta <= 0.0f)
            {
                //H * -2 * G = 지정한 jumpHeight에 도달하기 위한 값
                owner.verticalVelocity = Mathf.Sqrt(owner.jumpHeight * -2f * owner.gravity);


                owner.animator.SetBool("jump", true);
            }

            // jump timeout
            if (jumpTimeDelta >= 0.0f)
            {
                jumpTimeDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            jumpTimeDelta = owner.jumpMaxTime;

            // fall timeout
            if (fallTimeDelta >= 0.0f)
            {
                fallTimeDelta -= Time.deltaTime;
            }
            else
            {
                owner.animator.SetBool("fall", true);
            }

            CCHGameManager.Instance.Input.mainAttack = false;
        }
        input.jump = false;

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (owner.verticalVelocity < owner.terminalVelocity)
        {
            owner.verticalVelocity += owner.gravity * Time.deltaTime;
        }

        return CCHStateMachine.EState.Jump;
    }

    public override void Dispose()
    {
        owner = null;
    }
}
