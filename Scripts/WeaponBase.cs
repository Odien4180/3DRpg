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

    [Header("���� �ִϸ��̼� �̺�Ʈ�� On/Off")]
    public Collider weaponCollider;

    [Header("���ݽ� �ó׸ӽ� ���޽�")]
    public CinemachineImpulseSource impulseSource;
    public float impulseForce = 0.3f;

    [Header("���� �浹 ������ ������Ʈ")]
    public ColliderHitReciever hitReciever;

    [Header("���� ����� ����Ʈ")]
    public DissolveEffect dissolve;
    public ParticleSystem particle;

    [Header("���� ��Ʈ ����Ʈ ����")]
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
