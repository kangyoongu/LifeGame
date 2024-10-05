using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisPosToTargetPosition : MonoBehaviour
{
    public Transform targetPos;
    void Update()
    {
        transform.position = targetPos.position;
    }
}
