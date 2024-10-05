using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BookManager : MonoBehaviour
{
    public Transform[] pages;
    int page = 0;
    Tweener tween;
    public EndingHint[] explain;
    bool playing = false;
    private RandomAudio pageAud;
    private void Awake()
    {
        pageAud = GetComponent<RandomAudio>();
    }
    public void OnClickRight()
    {
        if (tween == null || !tween.IsPlaying())
        {
            page++;
            if (page > 2)
            {
                page = 2;
            }
            else
            {
                pageAud.Play();
            }
            if (page == 2)
            {
                pages[1].SetAsLastSibling();
            }
            tween = pages[page - 1].DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(0, 180, 0)), 0.7f);
        }
    }
    public void OnClickLeft()
    {
        if (tween == null || !tween.IsPlaying())
        {
            page--;
            if(page < 0)
            {
                page = 0;
            }
            else
            {
                pageAud.Play();
            }
            if (page == 0)
            {
                pages[0].SetAsLastSibling();
            }
            tween = pages[page].DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(0, 0, 0)), 0.7f);
        }
    }
    public void OnClickDown()
    {
        if (tween == null || !tween.IsPlaying())
        {
            UIManager.Instance.InMainUI();
            tween = GetComponent<RectTransform>().DOAnchorPosY(-1108f, 1f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                page = 0;
                pages[0].localRotation = Quaternion.Euler(Vector3.zero);
                pages[1].localRotation = Quaternion.Euler(Vector3.zero);
                pages[0].SetAsLastSibling();
                for(int i = 0; i < explain.Length; i++)
                {
                    explain[i].OnClickClose(0);
                }
            });
        }
    }
    public void OnClickBook()
    {
        UIManager.Instance.OutMainUI();
        GetComponent<RectTransform>().DOAnchorPosY(0f, 1f).SetEase(Ease.OutCubic);
    }
}
