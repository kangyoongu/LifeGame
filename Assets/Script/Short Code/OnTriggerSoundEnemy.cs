using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(RandomAudio))]
public class OnTriggerSoundEnemy : MonoBehaviour
{
    private RandomAudio rand;
    private NavMeshAgent navMeshAgent;
    float time = 0;
    private void Awake()
    {
        rand = GetComponent<RandomAudio>();
        navMeshAgent = transform.root.GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        time += Time.deltaTime;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor") && navMeshAgent.isStopped == false && time >= 0.4f)
        {
            rand.Play();
            time = 0;
        }
    }
}