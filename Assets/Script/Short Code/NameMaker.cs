using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameMaker
{
    private string[] first = { "��", "��", "��", "��", "��", "��" };
    private string[] Msecond = { "��", "��", "��", "��", "��", "��", "��", "��", "��" };
    private string[] Wsecond = { "��", "��", "��", "ä", "��", "��", "��", "��", "��" };
    private string[] Mthird = { "��", "��", "��", "ȯ", "��", "��", "��", "��", "��", "��"};
    private string[] Wthird = { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };
    public string MaleName()
    {
        return first[Random.Range(0, first.Length)] + Msecond[Random.Range(0, Msecond.Length)] + Mthird[Random.Range(0, Mthird.Length)];
    }
    public string FemaleName()
    {
        return first[Random.Range(0, first.Length)] + Wsecond[Random.Range(0, Wsecond.Length)] + Wthird[Random.Range(0, Wthird.Length)];
    }
}
