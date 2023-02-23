using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterViewBox : MonoBehaviour
{
    public bool lockLook = false;
    public GameObject charactersLook;
    public GameObject head;
    public GameObject face;
    private GameObject nearestTarget;
    private Vector3 lookOriginLocalPos;
    private Vector3 currentVelocity;

    public float moveLookTime = 30.0f;
    
    public List<GameObject> objsInSight = new List<GameObject>();

    private void Awake()
    {
        lookOriginLocalPos = charactersLook.transform.localPosition;
    }

    private void Update()
    {
        MoveCharactersLook();
    }

    private void LateUpdate()
    {
        if (lockLook == false)
            head.transform.LookAt(charactersLook.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (nearestTarget == null)
        {
            nearestTarget = other.gameObject;
        }

        if (!objsInSight.Contains(other.gameObject))
        {
            objsInSight.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objsInSight.Contains(other.gameObject))
        {
            objsInSight.Remove(other.gameObject);
        }

        if (other.gameObject == nearestTarget)
        {
            FindNearestObj();
        }
    }

    private void FindNearestObj()
    {
        if (objsInSight.Count == 0)
        {
            nearestTarget = null;
        }
        else if (objsInSight.Count == 1)
        {
            nearestTarget = objsInSight[0];
        }
        else
        {
            nearestTarget = objsInSight[0];

            for (int i = 1; i < objsInSight.Count; ++i)
            {
                if (Vector3.Distance(face.transform.position, objsInSight[i].transform.position)
                    < Vector3.Distance(face.transform.position, nearestTarget.transform.position))
                {
                    nearestTarget = objsInSight[i];
                }
            }
        }
    }

    private void MoveCharactersLook()
    {
        if (lockLook)
        {
            charactersLook.transform.localPosition = lookOriginLocalPos;
        }
        else
        {
            if (nearestTarget == null)
            {
                charactersLook.transform.localPosition = Vector3.SmoothDamp(charactersLook.transform.localPosition,
                    lookOriginLocalPos, ref currentVelocity, moveLookTime * Time.deltaTime);
            }
            if (nearestTarget != null)
            {
                charactersLook.transform.position = Vector3.SmoothDamp(charactersLook.transform.position,
                    nearestTarget.transform.position, ref currentVelocity, moveLookTime * Time.deltaTime);
            }
        }
    }
}
