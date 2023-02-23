using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Player_RemainWeapon : CCHStateBase
{
    private PlayerCharacterController owner;

    private void Awake()
    {
        owner = GetComponent<PlayerCharacterController>();  
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        return CCHStateMachine.EState.RemainWeapon;
    }

    public override void OnStateExit()
    {
        
    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        if (owner.isRemainWeapon)
        {
            if (owner.currentWeaponTime > owner.maxRemainWeaponTime)
            {
                owner.isRemainWeapon = false;
                owner.currentWeaponTime = 0.0f;
            }
            else
                owner.currentWeaponTime += Time.deltaTime;
        }
        else
            owner.weaponModule.DeactiveWeapon(EWeaponSlot.Back, true);

        return CCHStateMachine.EState.RemainWeapon;
    }

    public override void Dispose()
    {
        owner = null;
    }
}
