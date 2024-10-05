using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [HideInInspector]public static Transform playerHead;
    private void Awake()
    {
        playerHead = transform;
    }
}
