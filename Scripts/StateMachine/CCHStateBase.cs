using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CCHStateBase : MonoBehaviour, IDisposable
{
    public abstract CCHStateMachine.EState OnStateEnter();
    public abstract CCHStateMachine.EState OnStateUpdate();
    public virtual CCHStateMachine.EState OnStateLateUpdate() { return CCHStateMachine.EState.None; }
    public abstract void OnStateExit();
    public abstract void Dispose();
}