using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Player_Attack : CCHStateBase
{
    private PlayerCharacterController owner;

    private float rotationVelocity;
    private float targetRotation;
    private void Awake()
    {
        owner = GetComponent<PlayerCharacterController>();
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        owner.animator.SetBool("attack", true);
        CCHGameManager.Instance.Input.mainAttack = false;

        owner.customFocusing = false;
        owner.weaponModule.ActiveWeapon(EWeaponSlot.Main);
        owner.weaponModule.DeactiveWeapon(EWeaponSlot.Back);

        owner.isRemainWeapon = false;

        owner.animator.applyRootMotion = true;

        return CCHStateMachine.EState.Attack;
    }

    public override void OnStateExit()
    {
        rotationVelocity = 0.0f;
        targetRotation = 0.0f;

        owner.customFocusing = true;
        owner.weaponModule.ActiveWeapon(EWeaponSlot.Back);
        owner.weaponModule.DeactiveWeapon(EWeaponSlot.Main);

        owner.currentWeaponTime = 0.0f;
        owner.isRemainWeapon = true;

        owner.animator.applyRootMotion = false;
    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        var input = CCHGameManager.Instance.Input;
        owner.animator.SetFloat("attackSpeed", owner.attackSpeed);

        //Rotation();
        if (input.mainAttack)
        {
            input.mainAttack = false;
            owner.animator.SetBool("attack", true);
        }
        var animPer = owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (owner.animator.GetBool("attackCheck") == false)
        {
            if (animPer > 0.5)
                return CCHStateMachine.EState.Move;
            else
                return CCHStateMachine.EState.Attack;
        }
        else
            return CCHStateMachine.EState.Attack;
    }

    public void OnEnterAttack()
    {
        Rotation();
    }

    private void Rotation()
    {
        Camera mainCamera = Camera.main;

        transform.rotation = Quaternion.Euler(0.0f, mainCamera.transform.eulerAngles.y, 0.0f);

        //카메라가 향하고 있는 방향으로 회전
        targetRotation = 
            mainCamera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
            ref rotationVelocity, owner.rotateTime * 3);

        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }

    public override void Dispose()
    {
        owner = null;
    }
}
