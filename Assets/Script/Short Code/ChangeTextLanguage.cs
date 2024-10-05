using AutoLocalization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextLanguage : MonoBehaviour
{
    public static ChangeTextLanguage instance;
    public TMP_Dropdown dropdown;
    public TextMeshProUGUI[] texts;
    private readonly int[] index = new int[] { 1, 3, 12, 13, 11, 6, 5, 14, 9};
    private void Awake()
    {
        if (instance == null) instance = this;   
    }
    public void Change()
    {
        for(int i = 0; i < texts.Length; i++)
        {
            print((Languages)index[dropdown.value]);
            texts[i].text = LanguageManager.instance.GetMeaning(texts[i].text.ToLower(), (Languages)index[dropdown.value]);
        }
    }
}
