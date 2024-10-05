using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudio : MonoBehaviour
{
    private AudioSource aud;
    [SerializeField] private Vector2 pitchRange;
    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }
    public void Play()
    {
        aud.pitch = Random.Range(pitchRange.x, pitchRange.y);
        aud.PlayOneShot(aud.clip);
    }
}
