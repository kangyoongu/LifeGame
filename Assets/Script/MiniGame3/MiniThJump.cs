using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniThJump : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            MiniThPlayerMove.canJump = true;
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            MiniThPlayerMove.canJump = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            MiniThPlayerMove.canJump = false;
    }
}
