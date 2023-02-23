using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class CCHGameManager : Singleton<CCHGameManager>
{
    private PlayerCharacterController[] characterControllers;

    public ReactiveProperty<PlayerCharacterController> currentCharacter
        = new ReactiveProperty<PlayerCharacterController>();

    public CinemachineVirtualCamera virtualCam;
    public CinemachineBrain cinemachineBrain;

    public RootingItemView rootingItemView;

    [SerializeField] private FieldPlayerInput _input;
    public FieldPlayerInput Input => _input;

    [SerializeField] private PlayerInput playerInput;
    public PlayerInput PlayerInput => playerInput;

    private void Start()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();
    }

    public async UniTask LoadRootingItemView()
    {
        rootingItemView = await AddressableManager.Instance.InstantiateAddressableAsync<RootingItemView>("UI", "RootingItemUI.prefab");
    }

    public async UniTask LoadCharacter()
    {
        currentCharacter.Value = await AddressableManager.Instance.
            InstantiateAddressableAsync<PlayerCharacterController>("Character", "Kato/Kato.prefab");
         
        Vector3 startPosition = Vector3.zero;

        Ray ray = new Ray(startPosition + new Vector3(0, 10, 0), Vector3.down);
        if (Physics.Raycast(ray, out var hit))
        {
            startPosition += hit.point;
        }
        currentCharacter.Value.transform.position = startPosition;
        
        await EquipWeapon("SiFiSword");
    }

    public async UniTask EquipWeapon(string weaponName)
    {
        await currentCharacter.Value.weaponModule.EquipWeapon(weaponName);
    }

    public void SwitchActionMap(string actionMapName)
    {
        playerInput.SwitchCurrentActionMap(actionMapName);
    }
}
