using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionCubeGenerator : MonoBehaviour
{
    public GameObject generator;
    public GameObject poolObjects;
    public GameObject creation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (poolObjects.transform.childCount != 0)
            {
                Destroy(poolObjects.transform.GetChild(0).gameObject);
            }
            Instantiate(creation, new Vector3(generator.transform.position.x,generator.transform.position.y-0.8f,generator.transform.position.z), Quaternion.identity, poolObjects.transform);
        }
    }
}
