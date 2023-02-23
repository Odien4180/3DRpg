using Cinemachine;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

[Serializable]
public class CharacterData
{
    
}

public class PlayerCharacterController : MonoBehaviour, IHittable
{    
    public CharacterController characterController;
    public QuadMapUnit quadMapUnit;

    [Header("이동&회전")]
    public float walkSpeed = 2.0f;
    public float dashSpeed = 5.5f;
    public float accelRate = 10.0f;
    public float rotateTime = 0.12f;

    public float swimSpeed = 2.0f;

    [Header("무기")]
    public WeaponModule weaponModule;

    [Header("공격")]
    public float attackSpeed = 1.0f;

    [Header("무기 지속")]
    public float maxRemainWeaponTime = 10.0f;
    public bool isRemainWeapon = false;
    public float currentWeaponTime = 0.0f;

    [Header("애니메이션")]
    public Animator animator;

    [Header("카메라")]
    public GameObject cvCamTarget;
    public float topClampAngle = 70.0f;
    public float bottomClampAngle = -30.0f;

    [Header("시선 조절")]
    public bool customFocusing = true;
    public Transform head;
    public GameObject look;
    public float sight;
    public float fov;
    public LayerMask searchLayer;
    public LayerMask obstarcleLayer;

    [Header("지면 감지")]
    public LayerMask groundLayer;
    public float groundCheckOffset = -0.14f;
    public float groundCheckRadius = 0.28f;

    [Header("수면 감지")]
    public LayerMask waterLayer;
    public float waterRayHeight;
    public float waterRayLength;
    public bool isWater = false;

    //수영 포지션 Y
    public float waterY;

    [Header("점프&낙하")]
    public float jumpMaxTime;
    public float fallMaxTime;
    public float jumpHeight = 1.2f;

    [Tooltip("캐릭터에 적용할 중력. 엔진 기본값은 -9.81f")]
    public float gravity = -15.0f;

    //점프 최대 속도
    public float terminalVelocity = 53.0f;

    public bool onGround = true;

    //낙하속도
    public float verticalVelocity;

    public float idleMotionTimer = 20.0f;

    private void Awake()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (quadMapUnit == null)
            quadMapUnit = GetComponent<QuadMapUnit>();
    }

    private void Update()
    {
        if (CCHGameManager.Instance.Input.interaction == true)
        {
            quadMapUnit.Interaction();

            CCHGameManager.Instance.Input.interaction = false;
        }
    }

    public void OnFootStep()
    {
        SoundManager.Instance.PlaySFX($"FootStep/Grass1 {UnityEngine.Random.Range(17, 31)}-Audio.wav", transform.position).Forget();
    }

    public void Hit(HitData hitData)
    {
        Debug.Log(gameObject.name);
    }
}