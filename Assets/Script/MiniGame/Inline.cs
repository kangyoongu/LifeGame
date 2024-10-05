using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inline : MonoBehaviour
{
    public float line;
    bool trg = false;
    private void OnEnable()
    {
        trg = false;
    }
    void Update()
    {
        if (trg == false)
        {
            float scale = (transform.parent.localScale.x - line) / transform.parent.localScale.x;
            transform.localScale = new Vector2(scale, scale);
            trg = true;
        }
    }
}
