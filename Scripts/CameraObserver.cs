using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraObserver : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCam;
    
    private void Start()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        SetMainVirtualCamera().Forget();
    }

    private async UniTask SetMainVirtualCamera()
    {
        while (CCHGameManager.Instance.currentCharacter.HasValue == false ||
            CCHGameManager.Instance.currentCharacter.Value == null)
        {
            await UniTask.NextFrame();
        }

        CCHGameManager.Instance.currentCharacter.Subscribe(
            x => virtualCam.Follow = x.cvCamTarget.transform).AddTo(this);
    }
}
