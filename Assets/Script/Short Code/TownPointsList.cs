using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownPointsList : MonoBehaviour
{
    public static TownPointsList Instance;
    public Transform[] points;
    public Transform exitPosition;
    public Transform homePosition;
    [HideInInspector] public int length;
    void Awake()
    {
        if (Instance == null) Instance = this;
        length = points.Length;
    }
}
