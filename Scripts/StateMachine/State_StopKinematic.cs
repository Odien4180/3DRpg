using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class State_StopKinematic : CCHStateBase
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        if (rb != null)
            rb.constraints = RigidbodyConstraints.FreezeAll;

        return CCHStateMachine.EState.StopKinematic;
    }

    public override void OnStateExit()
    {
        
    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        if (rb != null)
            rb.velocity = Vector3.zero;

        return CCHStateMachine.EState.StopKinematic;
    }

    public override void Dispose()
    {
        rb = null;
    }
}
