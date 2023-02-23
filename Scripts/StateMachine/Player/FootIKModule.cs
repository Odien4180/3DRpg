using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIKModule : MonoBehaviour
{
    [SerializeField] private PlayerCharacterController owner;
    [SerializeField] private Animator animator;
    [Range(0, 1)]
    public float ikOffset = 0.05f;
    public float rayLength = 1.0f;

    public GameObject left;
    public GameObject right;


    private void Awake()
    {
        if (owner == null)
            owner = GetComponent<PlayerCharacterController>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }


    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);

        //지면 감지 수월하게 하기위해 +up
        Ray leftRay = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
#if UNITY_EDITOR
        Debug.DrawRay(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down * (ikOffset + rayLength), Color.red);
#endif
        if (Physics.Raycast(leftRay, out var leftHit, ikOffset + rayLength, owner.groundLayer))
        {
            left.transform.position = leftHit.point;

            Vector3 footPos = leftHit.point;
            //footPos.y += ikOffset;

            animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPos);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot,
                Quaternion.LookRotation(transform.forward, leftHit.normal));
        }

        Ray RightRay = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
#if UNITY_EDITOR
        Debug.DrawRay(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down * (ikOffset + rayLength), Color.red);
#endif
        if (Physics.Raycast(RightRay, out var rightHit, ikOffset + rayLength, owner.groundLayer))
        {
            right.transform.position = rightHit.point;

            Vector3 footPos = rightHit.point;
            footPos.y += ikOffset;
        
            animator.SetIKPosition(AvatarIKGoal.RightFoot, footPos);
            animator.SetIKRotation(AvatarIKGoal.RightFoot,
                Quaternion.LookRotation(transform.forward, rightHit.normal));
        }
    }
}
