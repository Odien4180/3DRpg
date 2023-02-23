using UnityEngine;

public class FovHitData
{
    public Collider collider;
    public float distance;
    public Vector3 direction;
    public FovHitData(Collider collider, float distance, Vector3 direction)
    {
        this.collider = collider;
        this.distance = distance;
        this.direction = direction;
    }
}

public class State_Enemy_Search : CCHStateBase
{
    private EnemyBase owner;

    private Vector3 lookDir;

    private Vector3 rightSightDir;
    private Vector3 leftSightDir;

    private Vector3 rightRangeDir;
    private Vector3 leftRangeDir;

    private void Awake()
    {
        if (owner == null)
            owner = GetComponent<EnemyBase>();
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        return CCHStateMachine.EState.Search;
    }

    public override void OnStateExit()
    {
        
    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        owner.sightHitDatas.Clear();
        owner.rangeHitDatas.Clear();
        owner.nearestFovHitData = null;

        float lookAngle = owner.eye.transform.eulerAngles.y;
        Vector3 eyePos = owner.eye.transform.position;

        lookDir = AngleToDir(lookAngle);

        rightSightDir = AngleToDir(owner.eye.transform.eulerAngles.y + owner.fov * 0.5f);
        leftSightDir = AngleToDir(owner.eye.transform.eulerAngles.y - owner.fov * 0.5f);

        rightRangeDir = AngleToDir(owner.eye.transform.eulerAngles.y + owner.rangeAngle * 0.5f);
        leftRangeDir = AngleToDir(owner.eye.transform.eulerAngles.y - owner.rangeAngle * 0.5f);

        Collider[] targets = Physics.OverlapSphere(owner.eye.transform.position, owner.sight, owner.searchLayer, QueryTriggerInteraction.Ignore);
        if (targets.Length == 0)
        {
            return CCHStateMachine.EState.Search;
        }

        FovHitData nearestHitData = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < targets.Length; ++i)
        {
            Vector3 targetPos = targets[i].bounds.center;
            Vector3 targetVec = (targetPos - owner.eye.transform.position);
            float distance = targetVec.magnitude;
            targetVec.y = 0;
            Vector3 targetDir = targetVec.normalized;

            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;


            if (distance < owner.range)
            {
                if (targetAngle <= owner.rangeAngle * 0.5f &&
                Physics.Raycast(eyePos, targetDir, distance, owner.obstarcleMask, QueryTriggerInteraction.Ignore) == false)
                {
                    var newData = new FovHitData(targets[i], distance, targetDir);
                    owner.rangeHitDatas.Add(newData);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestHitData = newData;
                    }
                }
#if UNITY_EDITOR
                Debug.DrawLine(eyePos, targetPos, Color.red);
#endif
            }
            else
            {
                if (targetAngle <= owner.fov * 0.5f &&
                Physics.Raycast(eyePos, targetDir, distance, owner.obstarcleMask, QueryTriggerInteraction.Ignore) == false)
                {
                    var newData = new FovHitData(targets[i], distance, targetDir);
                    owner.sightHitDatas.Add(newData);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestHitData = newData;
                    }
                }
#if UNITY_EDITOR
                Debug.DrawLine(eyePos, targetPos, Color.magenta);
#endif
            }
        }

        owner.nearestFovHitData = nearestHitData;

        return CCHStateMachine.EState.Search;
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (owner == null) return;
        if (owner.eye == null) return;

        Vector3 eyePos = owner.eye.transform.position;


        if (owner != null)
        {
            //시야
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(eyePos, owner.sight);

            //사거리
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(eyePos, owner.range);
        }

        if (Application.isPlaying)
        {
            Debug.DrawRay(eyePos, lookDir * owner.sight, Color.cyan);

            Debug.DrawRay(eyePos, rightSightDir * owner.sight, Color.blue);
            Debug.DrawRay(eyePos, leftSightDir * owner.sight, Color.blue);
            Debug.DrawRay(eyePos, rightRangeDir * owner.range, Color.blue);
            Debug.DrawRay(eyePos, leftRangeDir * owner.range, Color.blue);
        }
    }
#endif

    public override void Dispose()
    {
        owner = null;
    }
}
