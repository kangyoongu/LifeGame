using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RandomAudio))]
public class OnTriggerSoundPlay : MonoBehaviour
{
    private RandomAudio rand;
    private void Awake()
    {
        rand = GetComponent<RandomAudio>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor") && PlayerController.walkSound)
        {
            rand.Play();
        }
    }
}