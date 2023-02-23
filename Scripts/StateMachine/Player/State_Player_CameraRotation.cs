using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Player_CameraRotation : CCHStateBase
{
    private PlayerCharacterController owner;

    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    private void Awake()
    {
        owner = GetComponent<PlayerCharacterController>();
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        return CCHStateMachine.EState.CameraRotation;
    }

    public override void OnStateExit()
    {
        
    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        return CCHStateMachine.EState.CameraRotation;
    }

    public override CCHStateMachine.EState OnStateLateUpdate()
    {
        var input = CCHGameManager.Instance.Input;
        if (input.look.sqrMagnitude >= 0.1)
        {
            cinemachineTargetYaw += input.look.x;
            cinemachineTargetPitch += input.look.y;
        }

        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, owner.bottomClampAngle, owner.topClampAngle);

        owner.cvCamTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch, cinemachineTargetYaw, 0.0f);

        return CCHStateMachine.EState.CameraRotation;
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public override void Dispose()
    {
        owner = null;
    }
}
