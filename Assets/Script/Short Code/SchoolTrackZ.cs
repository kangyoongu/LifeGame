using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolTrackZ : MonoBehaviour
{
    public static bool isMove = true;
    void Update()
    {
        if(isMove == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, GameManager.Instance.playerTrm.position.z * 0.5f);
        }
    }
}
