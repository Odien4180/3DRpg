using Cinemachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UniRx;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public int damage;

    [Header("공격 애니메이션 이벤트로 On/Off")]
    public Collider weaponCollider;

    [Header("공격시 시네머신 임펄스")]
    public CinemachineImpulseSource impulseSource;
    public float impulseForce = 0.3f;

    [Header("무기 충돌 감지용 컴포넌트")]
    public ColliderHitReciever hitReciever;

    [Header("무기 연출용 이펙트")]
    public DissolveEffect dissolve;
    public ParticleSystem particle;

    [Header("무기 히트 이펙트 정보")]
    public string attackParticleName;
    public string hitParticleName;

    private void Awake()
    {
        weaponCollider.enabled = false;

        hitReciever?.colliderHitSubject.Subscribe(ColliderHit).AddTo(this);
        particle.gameObject.SetActive(false);
    }

    public void DoDissolve(bool makeParticle = false)
    {
        if (dissolve != null)
        {
            if (dissolve.Dissolve() && makeParticle)
                MakeParticle();
        }
    }

    public void DoPhase(bool makeParticle = false)
    {
        if (dissolve != null)
        {
            if (dissolve.Phase() && makeParticle)
                MakeParticle();
        }
    }

    private void MakeParticle()
    {
        if (particle == null)
            return;

        particle.Clear();
        particle.gameObject.SetActive(true);
        particle.Play();
    }

    public void MakeAttackParticle()
    {
        ObjectPoolManager.Instance.Get<ParticleHelper>(Const.type_effects, attackParticleName, 1.0f,
                x => x.Initialize(transform.position, transform.rotation)).Forget();
    }

    public void MakeHitParticle(Vector3 position, Quaternion rotation)
    {
        ObjectPoolManager.Instance.Get<ParticleHelper>(Const.type_effects, hitParticleName, 1.0f,
                x => x.Initialize(position, rotation)).Forget();
    }

    private void ColliderHit(Collision other)
    {
        impulseSource.GenerateImpulseWithVelocity(Random.insideUnitSphere * impulseForce);

        if (other.gameObject.TryGetComponent<IHittable>(out var hittable))
        {
            HitData hd = new HitData();

            hd.position = other.contacts[0].point;
            hd.damage = damage;

            MakeHitParticle(hd.position, transform.rotation);
            
            hittable.Hit(hd);
        }
    }
}
