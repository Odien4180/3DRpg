using UnityEngine;

public class State_Player_Look : CCHStateBase
{
    private PlayerCharacterController owner;
    private Vector3 lookDir;

    private Vector3 rightSightDir;
    private Vector3 leftSightDir;

    private Vector3 currentVelocity;

    public float moveLookTime = 30.0f;

    private void Awake()
    {
        owner = GetComponent<PlayerCharacterController>();
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        return CCHStateMachine.EState.Look;
    }

    public override void OnStateExit()
    {

    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        float lookAngle = owner.head.eulerAngles.y;
        Vector3 headPos = owner.head.position;
        lookDir = AngleToDir(lookAngle);

        rightSightDir = AngleToDir(owner.head.eulerAngles.y + owner.fov * 0.5f);
        leftSightDir = AngleToDir(owner.head.eulerAngles.y - owner.fov * 0.5f);

        Debug.DrawRay(headPos, lookDir * owner.sight, Color.cyan);
        Debug.DrawRay(headPos, rightSightDir * owner.sight, Color.blue);
        Debug.DrawRay(headPos, leftSightDir * owner.sight, Color.blue);


        Collider[] targets = Physics.OverlapSphere(headPos, 
            owner.sight, owner.searchLayer, QueryTriggerInteraction.Ignore);

        if (targets.Length == 0)
        {
            return CCHStateMachine.EState.Look;
        }

        Collider nearCollider = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < targets.Length; ++i)
        {
            Vector3 targetPos = targets[i].bounds.center;
            Vector3 targetVec = (targetPos - headPos);
            float distance = targetVec.magnitude;
            targetVec.y = 0;
            Vector3 targetDir = targetVec.normalized;

            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;

            if (targetAngle <= owner.fov * 0.5f &&
                Physics.Raycast(headPos, targetDir, distance, owner.obstarcleLayer, QueryTriggerInteraction.Ignore) == false)
            {
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearCollider = targets[i];
                }
            }
#if UNITY_EDITOR
            Debug.DrawLine(headPos, targetPos, Color.red);
#endif
        }

        if (owner.customFocusing)
        {
            if (nearCollider != null)
            {
                owner.look.transform.position = Vector3.SmoothDamp(owner.look.transform.position,
                nearCollider.bounds.center, ref currentVelocity, moveLookTime * Time.deltaTime);
            }
        }

        return CCHStateMachine.EState.Look;
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

    public override void Dispose()
    {
        owner = null;
    }
}
