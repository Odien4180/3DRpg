using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModule : MonoBehaviour
{
    public CinemachineVirtualCamera mainVCam;
    public CinemachineVirtualCamera aimVCam;

    public CinemachineVirtualCamera currentVCam;

    private void OnEnable()
    {
        CameraManager.Instance.playerCameraModule = this;
    }

    public void OnMain()
    {
        SwapCam(mainVCam);
    }

    public void OnAim()
    {
        SwapCam(aimVCam);
    }


    private void SwapCam(CinemachineVirtualCamera vCam)
    {
        currentVCam.enabled = false;

        currentVCam = vCam;
        currentVCam.enabled = true;
    }
}
