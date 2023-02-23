using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Player_Swim : CCHStateBase
{
    private PlayerCharacterController owner;

    private Camera mainCamera;
    private float currentSpeed;
    private float animBlendSpeed;
    private float targetRotation;
    private float rotationVelocity;

    private void Awake()
    {
        owner = GetComponent<PlayerCharacterController>();
        mainCamera = Camera.main;
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        owner.animator.SetBool("swim", true);

        return CCHStateMachine.EState.Swim;
    }

    public override void OnStateExit()
    {
        owner.animator.SetBool("swim", false);
    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        if (owner.isWater == false)
            return CCHStateMachine.EState.Move;

        float targetSpeed = owner.swimSpeed;

        var input = CCHGameManager.Instance.Input;

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

        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        owner.characterController.Move(targetDirection.normalized * (currentSpeed * Time.deltaTime));

        
        if (owner.animator.GetBool("grounded") == false)
        {
            var tempPos = owner.transform.position;
            owner.transform.position = new Vector3(tempPos.x, owner.waterY, tempPos.z);

        }
        return CCHStateMachine.EState.Swim;
    }

    public override void Dispose()
    {
        owner = null;
        mainCamera = null;
    }
}
