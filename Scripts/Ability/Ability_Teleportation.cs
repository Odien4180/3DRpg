using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Ability_Teleportation : AbilityBase
{
    public float range;

    public CharacterController cc;

    public LayerMask checkLayer;

    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    public Material mat;
    public VisualEffect popEffect;

    private void Awake()
    {
        if (cc == null)
            cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Activate();
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            CameraManager.Instance.playerCameraModule.OnAim();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            CameraManager.Instance.playerCameraModule.OnMain();
        }
    }

    public override void Activate()
    {
        Ray ray = new Ray(cc.bounds.center, transform.forward);
        Vector3 tpPos = transform.position;

        if (Physics.Raycast(ray, out var hit, range, checkLayer))
        {
            tpPos = hit.point;
        }
        else
        {
            tpPos += transform.forward * range;
        }

        ray = new Ray(tpPos + new Vector3(0, 1.0f, 0), Vector3.down);

        if (Physics.Raycast(ray, out var downHit, range, checkLayer))
        {
            tpPos = downHit.point;
        }

        ActiveTrail();

        cc.enabled = false;
        transform.position = tpPos;
        cc.enabled = true;
    }

    public void ActiveTrail()
    {
        if (skinnedMeshRenderers == null)
            skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        for (int i = 0; i < skinnedMeshRenderers.Length; ++i)
        {
            GameObject gObj = new GameObject();
            gObj.transform.SetPositionAndRotation(transform.position, transform.rotation);
            MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            MeshFilter mf = gObj.AddComponent<MeshFilter>();
            Mesh mesh = new Mesh();
            skinnedMeshRenderers[i].BakeMesh(mesh);
            mf.mesh = mesh;
            
            Material[] mats = skinnedMeshRenderers[i].materials;
            for (int j = 0; j < mats.Length; ++j)
            {
                mats[j] = Instantiate(mat);
                mats[j].DOFloat(0, "_Alpha", 1.0f);
            }

            mr.materials = mats;
            var pe = Instantiate(popEffect, transform.position, transform.rotation);
            pe.Play();

            Destroy(gObj, 1.0f);
        }
    }
}
