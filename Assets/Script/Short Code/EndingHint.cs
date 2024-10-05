using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class EndingHint : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI butText;
    public RectTransform window;

    [TextArea]
    [SerializeField] private string[] firstHint;
    [SerializeField] private string[] buttonHint;
    int stack = 0;
    bool smalling = false;
    Tweener tween;
    private void Awake()
    {
        text.text = firstHint[stack];
        butText.text = buttonHint[stack];
    }
    public void OnClickOpen()
    {
        if(tween == null || !tween.IsPlaying())
            tween = window.DOScale(Vector2.one, 0.6f).SetEase(Ease.InQuart);
    }
    public void OnClickNext()
    {
        stack++;
        text.text = firstHint[stack];
        if(stack >= buttonHint.Length) butText.gameObject.SetActive(false);
        else butText.text = buttonHint[stack];
    }
    public void OnClickClose(float time)
    {
        if (smalling == false)
        {
            smalling = true;
            window.DOScale(Vector3.zero, 0.6f).SetEase(Ease.InQuart).OnComplete(() =>
            {
                stack = 0;
                text.text = firstHint[stack];
                butText.text = buttonHint[stack];
                butText.gameObject.SetActive(true);
                smalling = false;
            });
        }
    }
}
