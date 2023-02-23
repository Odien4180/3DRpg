using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

public class State_Player_Move : CCHStateBase
{
    private PlayerCharacterController owner;
    private Camera mainCamera;
    private float currentSpeed;
    private float animBlendSpeed;
    private float targetRotation;
    private float rotationVelocity;
    private bool dash;

    private void Awake()
    {
        owner = GetComponent<PlayerCharacterController>();
        mainCamera = Camera.main;
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        return CCHStateMachine.EState.Move;
    }

    public override void OnStateExit()
    {
        targetRotation = 0.0f;
        rotationVelocity = 0.0f;
        currentSpeed = 0.0f;
        animBlendSpeed = 0.0f;
        owner.animator.SetFloat("speed", 0.0f);
    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        if (owner.isWater)
            return CCHStateMachine.EState.Swim;

        var input = CCHGameManager.Instance.Input;

        if (input.mainAttack && owner.onGround)
            return CCHStateMachine.EState.Attack;

        if (input.dash)
        {
            dash = !dash;
            input.dash = false;
        }

        float targetSpeed = dash ? owner.dashSpeed : owner.walkSpeed;

        if (input.move == Vector2.zero)
            targetSpeed = 0.0f;

        float addSpeed = new Vector3(owner.characterController.velocity.x, 0.0f,
            owner.characterController.velocity.z).magnitude;

        float speedOffset = 0.1f;

        if (addSpeed < targetSpeed - speedOffset ||
            addSpeed > targetSpeed + speedOffset)
        {
            currentSpeed = Mathf.Lerp(addSpeed, targetSpeed, Time.deltaTime * owner.accelRate);
            currentSpeed = Mathf.Round(currentSpeed * 1000) / 1000f;
        }
        else
            currentSpeed = targetSpeed;

        //애님 블렌드 부분
        animBlendSpeed = Mathf.Lerp(animBlendSpeed, targetSpeed, Time.deltaTime * owner.accelRate);
        if (animBlendSpeed < 0.01f)
            animBlendSpeed = 0f;

        owner.animator.SetFloat("speed", animBlendSpeed);

        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;
        
        if (input.move != Vector2.zero)
        {
            //카메라가 향하고 있는 방향으로 회전
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
                ref rotationVelocity, owner.rotateTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        if (currentSpeed == 0)
            return CCHStateMachine.EState.Idle;

        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        owner.characterController.Move(targetDirection.normalized * (currentSpeed * Time.deltaTime) +
            new Vector3(0, owner.verticalVelocity, 0) * Time.deltaTime);

        return CCHStateMachine.EState.Move;
    }

    public override void Dispose()
    {
        owner = null;
        mainCamera = null;
    }
}
