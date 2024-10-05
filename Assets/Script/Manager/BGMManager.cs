using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;
    public AudioClip[] bgms;
    [HideInInspector] public AudioSource bgmSource;

    public AudioMixer mixer;
    public Slider sfxslider;
    public Slider bgmslider;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        bgmSource = GetComponent<AudioSource>();
        FadeIn(0, 1);
    }
    public void FadeInAndOut(int nextAud, float outTime, float inTime)
    {
        bgmSource.volume = 1;
        bgmSource.DOFade(0, outTime).OnComplete(() =>
        {
            bgmSource.clip = bgms[nextAud];
            bgmSource.Play();
            bgmSource.DOFade(1, inTime);
        });
    }
    public void FadeIn(int aud, float time)
    {
        bgmSource.volume = 0;
        bgmSource.clip = bgms[aud];
        bgmSource.Play();
        bgmSource.DOFade(1, time);
    }
    public void FadeOut(float time)
    {
        bgmSource.volume = 1;
        bgmSource.DOFade(0, time);
    }
    public void OnBGMValueChange()
    {
        try
        {
            mixer.SetFloat("BGM", Mathf.Log10(bgmslider.value) * 20);
        }
        catch(UnityException e)
        {
            print(e);
        }
    }
    public void OnSFXValueChange()
    {
        try
        {
            mixer.SetFloat("SFX", Mathf.Log10(sfxslider.value) * 20);
        }

        catch (UnityException e)
        {
            print(e);
        }
    }
}
