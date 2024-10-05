using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameMaker
{
    private string[] first = { "김", "박", "이", "강", "최", "윤" };
    private string[] Msecond = { "민", "서", "도", "시", "지", "정", "승", "예", "태" };
    private string[] Wsecond = { "서", "하", "지", "채", "소", "시", "아", "민", "다" };
    private string[] Mthird = { "우", "후", "현", "환", "찬", "준", "원", "규", "결", "혁"};
    private string[] Wthird = { "연", "윤", "현", "은", "원", "율", "온", "영", "빈", "유", "우" };
    public string MaleName()
    {
        return first[Random.Range(0, first.Length)] + Msecond[Random.Range(0, Msecond.Length)] + Mthird[Random.Range(0, Mthird.Length)];
    }
    public string FemaleName()
    {
        return first[Random.Range(0, first.Length)] + Wsecond[Random.Range(0, Wsecond.Length)] + Wthird[Random.Range(0, Wthird.Length)];
    }
}
