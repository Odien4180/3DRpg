using CCHExtention;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioHelper : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(string clipName, Vector3 position)
    {
        transform.position = position;
        audioSource.SetClip(clipName, x => { 
            x.Play();
            StartCoroutine("ReturnToPool", x.clip.length);
        }).Forget();
    }

    IEnumerator ReturnToPool(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        ObjectPoolManager.Instance.Push(gameObject);
    }
}
