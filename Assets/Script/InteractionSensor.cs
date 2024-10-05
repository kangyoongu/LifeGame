using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionSensor : MonoBehaviour
{
    public UnityEvent InteractionStay;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Girl") || other.transform.CompareTag("Man"))
        {
            other.transform.GetComponent<MoveNPCCore>().look = true;
        }
        if (other.transform.parent != null)
        {
            if (other.transform.parent.CompareTag("Girl") || other.transform.parent.CompareTag("Man"))
            {
                other.transform.parent.GetComponent<NPCcore>().look = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Girl") || other.transform.CompareTag("Man"))
        {
            other.transform.GetComponent<MoveNPCCore>().look = false;
        }
        if (other.transform.parent != null)
        {
            if (other.transform.parent.CompareTag("Girl") || other.transform.parent.CompareTag("Man"))
            {
                other.transform.parent.GetComponent<NPCcore>().look = false;
            }
        }
    }
}
