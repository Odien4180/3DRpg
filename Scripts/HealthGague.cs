using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthGague : MonoBehaviour
{
    public Image background;
    public Image fill;
    public Image lerp;

    public void SetGague(float per)
    {
        float backWidth = background.rectTransform.sizeDelta.x;
        var anchoredPos = fill.rectTransform.anchoredPosition;

        float targetPosX = backWidth / 2 - backWidth * (1 - per);
        DOTween.Kill(lerp);
        lerp.rectTransform.DOAnchorPosX(targetPosX, 1.0f).SetEase(Ease.InQuint);
        
        fill.rectTransform.anchoredPosition = new Vector2(targetPosX, anchoredPos.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            ConversationManager.Instance.Initialize("Conversation/testConv.playable").Forget();
    }

}
