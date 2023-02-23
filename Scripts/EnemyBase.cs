using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;

public class EnemyBase : MonoBehaviour, IHittable
{
    public NavMeshAgent agent;

    public CCHStateMachine stateMachine;
    public LayerMask searchLayer;
    public LayerMask obstarcleMask;
    public GameObject eye;
    public Animator animator;
    public DissolveEffect dissolve;
    public HealthGague healthGague;
    public EnemyData enemyData;

    [SerializeField]
    private GameObject target;
    public GameObject Target {
        get { return target; }
        set { 
            target = value;
            if (target == null) return;
            var controller = target.GetComponent<CharacterController>();
            if (controller != null)
                playerCc = controller;
        }
    }
    [HideInInspector] public CharacterController playerCc;

    [HideInInspector]public float sight = 15.0f;
    [HideInInspector]public float fov = 120.0f;
    [HideInInspector]public float speed = 1.0f;
    [HideInInspector]public float range = 5.0f;
    [HideInInspector]public float rangeAngle = 30.0f;
    [HideInInspector]public float attackDelay = 1.0f;
    [HideInInspector]public int health = 100;
    [HideInInspector]public int power = 100;
    [HideInInspector]public int powerRange = 100;

    [HideInInspector] public float currentDelay = 0.0f;
    [HideInInspector] public bool isCombat = false;
    [HideInInspector] public List<FovHitData> rangeHitDatas = new List<FovHitData>();
    [HideInInspector] public List<FovHitData> sightHitDatas = new List<FovHitData>();
    [HideInInspector] public FovHitData nearestFovHitData;

    [HideInInspector] public bool alive = true;

    private Collider enemyCollider;

    private void Awake()
    {
        gameObject.SetActive(false);

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        if (target != null)
            playerCc = target.GetComponent<CharacterController>();
        if (stateMachine == null)
            stateMachine = GetComponent<CCHStateMachine>();
        if (animator == null)
            animator = GetComponent<Animator>();

        Initialize().Forget();
    }

    public async UniTask Initialize()
    {
        sight = enemyData.sight;
        fov = enemyData.fov;
        speed = enemyData.speed;
        range = enemyData.range;
        rangeAngle = enemyData.rangeAngle;
        attackDelay = enemyData.attackDelay;
        health= enemyData.health;
        power = enemyData.power;
        powerRange = enemyData.powerRange;

        animator.Rebind();
        animator.SetFloat("speed", speed);

        if (healthGague == null)
        {
            healthGague = await ObjectPoolManager.Instance.Get<HealthGague>("WorldUI", "HealthGague.prefab");

            healthGague.transform.SetParent(transform);
            var rt = healthGague.GetComponent<RectTransform>();
            rt.localPosition = enemyData.healthGaguePos;
            rt.localScale = enemyData.healthGagueScale;
        }
        healthGague.gameObject.SetActive(true);
        healthGague.SetGague(1.0f);

        gameObject.SetActive(true);

        CCHGameManager.Instance.currentCharacter.Subscribe(x =>
        {
            if (x != null)
                target = x.gameObject;
            else
                target = null;
        }).AddTo(this);
    }

    public void Hit(HitData hitData)
    {
        health -= hitData.damage;

        if (health <= 0 && alive)
        {
            alive = false;
        }

        healthGague?.SetGague(Mathf.Max((float)health / enemyData.health, 0));

        WorldUIGenerator.Instance.PopDamageText(hitData.damage.ToString(), hitData.position, Color.green, 0.1f).Forget();
    }
}