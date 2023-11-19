using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActivator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Button"))
        {
            other.gameObject.GetComponent<ButtonFunction>().activate();
        }
    }
}
