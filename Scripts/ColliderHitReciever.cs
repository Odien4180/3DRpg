using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Scripting;

public class ColliderHitReciever : MonoBehaviour, IDisposable
{
    public Subject<Collision> colliderHitSubject = new Subject<Collision>();
    public Subject<Collision> colliderExitSubject = new Subject<Collision>();

    private void OnCollisionEnter(Collision collision)
    {
        colliderHitSubject?.OnNext(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        colliderExitSubject?.OnNext(collision);
    }

    public void Dispose()
    {
        colliderHitSubject?.Dispose();
        colliderExitSubject?.Dispose();
    }
}
