using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<FPSController>().Die();
        }
        else if (other.CompareTag("Turret") || other.CompareTag("Cube"))
        {
            Destroy(other.gameObject);
        }
    }
}
