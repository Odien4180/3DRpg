using Cysharp.Threading.Tasks;
using UnityEngine;

public class State_Enemy_Die : CCHStateBase
{
    private EnemyBase owner;
    private float deltaTime;
    private void Awake()
    {
        owner = GetComponent<EnemyBase>();
    }

    public override CCHStateMachine.EState OnStateEnter()
    {
        owner.animator.SetTrigger("die");
        owner.healthGague.gameObject.SetActive(false);
        deltaTime = 0.0f;


        var items = owner.enemyData.dropItems;

        for (int i = 0; i < items.Count; ++i)
        {
            InventoryManager.Instance.Add(items[i]).Forget();
        }

        return CCHStateMachine.EState.Die;
    }

    public override void OnStateExit()
    {
        
    }

    public override CCHStateMachine.EState OnStateUpdate()
    {
        var animPer = owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        var clip = owner.animator.GetCurrentAnimatorClipInfo(0);
        
        if (animPer >= 1.0f && deltaTime > 0.1f)
        {
            if (owner.dissolve == null)
            {
                ObjectPoolManager.Instance.Push(owner.gameObject);
            }
            else
            {
                owner.dissolve.Dissolve(() =>
                ObjectPoolManager.Instance.Push(owner.gameObject));
            }
        }

        deltaTime += Time.deltaTime;
        return CCHStateMachine.EState.Die;
    }
    public override void Dispose()
    {
        owner = null;
    }
}
