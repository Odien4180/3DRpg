using CCHExtention;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace CCHExtention
{
    public static class AudioExtention
    {
        public static async UniTask SetClip(this AudioSource audioSource, string clipName, Action<AudioSource> onComplete = null)
        {
            var loadOperation = AddressableManager.Instance.LoadAddressableAsync<AudioClip>("Audio", clipName);
            
            audioSource.clip = await loadOperation;
            onComplete?.Invoke(audioSource);
        }
    }
}

public class SoundManager : Singleton<SoundManager>
{
    public AudioMixer mixer;
    public AudioSource audioSource;

    private Tween tweening;

    public void PlayBGM(string bgmName)
    {
        audioSource.SetClip(bgmName, x => x.Play()).Forget();
    }

    public void FadeInBGM(float fadeTime, Action OnComplete = null)
    {
        tweening?.Kill();

        float realFadeTime = (1 - audioSource.volume) * fadeTime;

        tweening = DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 1, realFadeTime)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                OnComplete?.Invoke();
            });   
    }

    public void FadeOutBGM(float fadeTime, Action OnComplete = null)
    {
        tweening?.Kill();

        float realFadeTime = audioSource.volume * fadeTime;

        tweening = DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0, realFadeTime)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                OnComplete?.Invoke();
            });
    }

    public async UniTask PlaySFX(string sfxName, Vector3 position)
    {
        var audioHelper = await ObjectPoolManager.Instance.Get<AudioHelper>("Audio", "AudioHelper.prefab", null, 10);
        audioHelper.Initialize(sfxName, position);
    }
}
