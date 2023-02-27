using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Player_GroundCheck : CCHStateBase
{
    private PlayerCharacterController owner;

    private void Awake()
    {
        owner = GetComponent<PlayerCharacterController>();
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        return CCHStateMachine.EState.CheckGround;
    }

    public override void OnStateExit()
    {
        
    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        owner.isWater = WaterCheck();

        Vector3 checkPointPos = new Vector3(transform.position.x, transform.position.y - owner.groundCheckOffset,
                transform.position.z);
        owner.onGround = Physics.CheckSphere(checkPointPos, owner.groundCheckRadius, owner.groundLayer, QueryTriggerInteraction.Ignore);

        owner.animator.SetBool("grounded", owner.onGround);

        return CCHStateMachine.EState.CheckGround;
    }

    private bool WaterCheck()
    {
#if UNITY_EDITOR
        Debug.DrawRay(transform.position + new Vector3(0, owner.waterRayHeight, 0), Vector3.down * owner.waterRayLength);
#endif

        Ray ray = new Ray(transform.position + new Vector3(0, owner.waterRayHeight, 0), Vector3.down);
        if (Physics.Raycast(ray, out var hit, owner.waterRayLength, owner.waterLayer))
        {
            owner.waterY = hit.point.y - (owner.waterRayHeight - owner.waterRayLength) - 0.05f;
            return true;
        }
        return false;
    }

    public override void Dispose()
    {
        owner = null;
    }
}
