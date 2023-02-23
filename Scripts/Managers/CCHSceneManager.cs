using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CCHSceneManager : Singleton<CCHSceneManager>
{
    public string nextSceneName = "startScene";
    public float minLoadTime = 0.0f;

    private AsyncOperation loadOperation;
    private bool currentLoading = false;
    private bool preLoad = false;
    public async UniTask LoadScene(string sceneName, float minLoadTime = 0.0f)
    {
        if (currentLoading)
            return;

        SoundManager.Instance.FadeOutBGM(1.0f);

        currentLoading = true;
        nextSceneName = sceneName;
        minLoadTime = 0.0f;

        if (loadOperation == null)
        {
            await PreloadLoadingScene(true);
        }
        else
            loadOperation.allowSceneActivation = true;

        currentLoading = false;
        preLoad = false;
    }

    public async UniTask PreloadLoadingScene(bool immediately = false)
    {
        if (preLoad)
            return;

        preLoad = true;

        loadOperation = SceneManager.LoadSceneAsync("LoadingScene");
        if (immediately == false)
            loadOperation.allowSceneActivation = false;
        await loadOperation;
    }
}
