using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniJSON;
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

    public InteractionView rootingItemView;

    [SerializeField] private FieldPlayerInput _input;
    public FieldPlayerInput Input => _input;

    [SerializeField] private PlayerInput playerInput;
    public PlayerInput PlayerInput => playerInput;

    public Inventory inventory;

    private new void Awake()
    {
        base.Awake();

        if(_input == null)
            _input = GetComponent<FieldPlayerInput>();

        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();
    }

    public async UniTask LoadRootingItemView()
    {
        rootingItemView = await AddressableManager.Instance.InstantiateAddressableAsync<InteractionView>("UI", "RootingItemUI.prefab");
    }

    public async UniTask LoadInventory()
    {
        inventory = await AddressableManager.Instance.InstantiateAddressableAsync<Inventory>("UI", "Inventory.prefab");
        inventory.gameObject.SetActive(false);
    }

    public async UniTask LoadCharacter()
    {
        //캐릭터 로드 추후 변경 (현재 임시)
        currentCharacter.Value = await AddressableManager.Instance.
            InstantiateAddressableAsync<PlayerCharacterController>("Character", "Kato/Kato.prefab");
         
        Vector3 startPosition = Vector3.zero;

        Ray ray = new Ray(startPosition + new Vector3(0, 10, 0), Vector3.down);
        if (Physics.Raycast(ray, out var hit))
        {
            startPosition += hit.point;
        }
        currentCharacter.Value.transform.position = startPosition;
        
        //장비 장착 추후 변경 (현재 임시)
        await EquipWeapon("SiFiSword");
    }

    public async UniTask EquipWeapon(string weaponName)
    {
        await currentCharacter.Value.weaponModule.EquipWeapon(weaponName);
    }

    public void SwitchActionMap(string actionMapName)
    {
        PlayerInput.SwitchCurrentActionMap(actionMapName);
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void PopInventory()
    {
        if (inventory == null)
            return;

        inventory.gameObject.SetActive(true);
        inventory.Initialize().Forget();
    }
}
