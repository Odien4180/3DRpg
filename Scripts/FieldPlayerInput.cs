using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FieldPlayerInput : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool dash;

    [Header("Attack")]
    public bool mainAttack;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    [Header("Debugging")]
    public bool debugging = false;

    //Input 시스템 사용 가능할 경우 호출되는 콜백 함수들
#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnDash(InputValue value)
    {
        DashInput(value.isPressed);
    }

    public void OnMainAttack(InputValue value)
    {
        MainAttackInput(value.isPressed);
    }

    public void OnInteraction(InputValue value)
    {
        Interaction(value.isPressed);
    }

    public void OnInventory(InputValue value)
    {
        Inventory(value.isPressed);
    }
#endif

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
#if UNITY_EDITOR
        if (debugging) Debug.Log($"OnMove : {move}");
#endif
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
#if UNITY_EDITOR
        if (debugging) Debug.Log($"OnLook : {look}");
#endif
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
#if UNITY_EDITOR
        if (debugging) Debug.Log($"OnJump : {jump}");
#endif
    }

    public void DashInput(bool newSprintState)
    {
        dash = newSprintState;
#if UNITY_EDITOR
        if (debugging) Debug.Log($"OnDash : {dash}");
#endif
    }

    private void MainAttackInput(bool newMainAttackState)
    {
        mainAttack = newMainAttackState;
#if UNITY_EDITOR
        if (debugging) Debug.Log($"OnMainAttack : {mainAttack}");
#endif
    }

    private void Interaction(bool newInteraction)
    {
        CCHGameManager.Instance.currentCharacter.Value?.Interaction();
#if UNITY_EDITOR
        if (debugging) Debug.Log("OnInteraction");
#endif
    }

    private void Inventory(bool newInventory)
    {
        CCHGameManager.Instance.PopInventory();
#if UNITY_EDITOR
        if (debugging) Debug.Log("Inventory");
#endif
    }

    //private void OnApplicationFocus(bool hasFocus)
    //{
    //    SetCursorState(cursorLocked);
    //}

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
