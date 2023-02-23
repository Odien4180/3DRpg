using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class CameraInputSystem : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;
    private Cinemachine3rdPersonFollow thirdPersonCam;
    public float maxZoom = 10.0f;
    public float minZoom = 0.0f;

    public float zoomDuration = 0.3f;

    private float defaultZoom = 6.0f;
    private float currentZoom = 6.0f;

    private Tween tween;
    private void Start()
    {
        thirdPersonCam = virtualCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        defaultZoom = thirdPersonCam.CameraDistance;
        currentZoom = thirdPersonCam.CameraDistance;
    }

    public void OnScroll(InputValue value)
    {
        Scroll(value.Get<Vector2>());
    }

    private void Scroll(Vector2 scrollVec)
    {
        if (scrollVec.y > 0)
        {
            currentZoom = Mathf.Max(minZoom, currentZoom - 1);
        }
        else if (scrollVec.y < 0)
        {
            currentZoom = Mathf.Min(maxZoom, currentZoom + 1);
        }

        tween.Kill();
        tween = DOTween.To(() => thirdPersonCam.CameraDistance,
            x => thirdPersonCam.CameraDistance = x,
            currentZoom, zoomDuration).SetEase(Ease.Linear);
    }
}
