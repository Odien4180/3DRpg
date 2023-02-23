using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum EWeaponSlot
{
    Main,
    Back,
}

public class WeaponModule : MonoBehaviour
{
    public WeaponBase mainWeapon;
    public WeaponBase backWeapon;
    public GameObject mainWeaponSlot;
    public GameObject backWeaponSlot;

    public async UniTask EquipWeapon(string weaponName)
    {
        if (mainWeapon != null)
            ObjectPoolManager.Instance.Push(mainWeapon.gameObject);

        if (backWeapon != null)
            ObjectPoolManager.Instance.Push(backWeapon.gameObject);

        string weaponPath = $"{weaponName}/{weaponName}.prefab";

        mainWeapon = await ObjectPoolManager.Instance.Get<WeaponBase>("Weapon", weaponPath, null, 2);
        mainWeapon.transform.SetParent(mainWeaponSlot.transform, false);
        backWeapon = await ObjectPoolManager.Instance.Get<WeaponBase>("Weapon", weaponPath, null, 2);
        backWeapon.transform.SetParent(backWeaponSlot.transform, false);

    }

    public void ActiveWeapon(EWeaponSlot weaponSlot, bool playParticle = false)
    {
        WeaponBase weapon = null;

        switch (weaponSlot)
        {
            case EWeaponSlot.Main:
                weapon = mainWeapon;
                break;
            case EWeaponSlot.Back:
                weapon= backWeapon;
                break;
        }

        weapon?.DoPhase(playParticle);
    }

    public void DeactiveWeapon(EWeaponSlot weaponSlot, bool playParticle = false)
    {
        WeaponBase weapon = null;

        switch (weaponSlot)
        {
            case EWeaponSlot.Main:
                weapon = mainWeapon;
                break;
            case EWeaponSlot.Back:
                weapon = backWeapon;
                break;
        }

        weapon?.DoDissolve(playParticle);
    }

    /// <summary>
    /// 공격 애니메이션에서 호출
    /// </summary>
    public void ActivateCollider(int bActive)
    {
        if (mainWeapon?.weaponCollider)
        {
            if (bActive == 1)
            {
                mainWeapon.MakeAttackParticle();
            }

            mainWeapon.weaponCollider.enabled = bActive == 0 ? false : true;
        }
    }
}
