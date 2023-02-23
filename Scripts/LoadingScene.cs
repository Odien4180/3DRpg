using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Image loadingImage;
    public string nextSceneName;
    public float minDeltaTime = 10.0f;
    
    private AsyncOperation loadSceneOperation;
    private float currentTime = 0.0f;
    public float smoothVelocity = 1.0f;
    public float smoothTime = 60.0f;
    private async UniTask Start()
    {
        nextSceneName = CCHSceneManager.Instance.nextSceneName;
        loadSceneOperation = SceneManager.LoadSceneAsync(nextSceneName);
        loadSceneOperation.allowSceneActivation = false;

        while (currentTime < minDeltaTime)
        {
            Debug.Log(loadSceneOperation.progress);

            await UniTask.NextFrame();
        }
        loadSceneOperation.allowSceneActivation = true;

        while (loadSceneOperation.isDone == false)
        {
            Debug.Log(loadSceneOperation.progress);
            
            await UniTask.NextFrame();
        }
    }
    private void Update()
    {
        loadingImage.fillAmount = Mathf.SmoothDamp(loadingImage.fillAmount, loadSceneOperation.progress, ref smoothVelocity, smoothTime * Time.deltaTime);

        currentTime += Time.deltaTime;
    }
}
