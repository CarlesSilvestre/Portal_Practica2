using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private LineRenderer lr;
    public Transform shootPoint;
    public LayerMask layerMask;
    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = true;
    }
    private void Update()
    {
        lr.SetPosition(0, shootPoint.position);
        lr.SetPosition(1, shootPoint.forward * 50f);
        RaycastHit point;
        if(Physics.Raycast(shootPoint.transform.position, shootPoint.forward,out point, 50f,layerMask))
        {
            GameObject collisionObject = point.collider.gameObject;
            if (collisionObject.tag.Equals("Player")&&lr.enabled){
                collisionObject.GetComponent<FPSController>().Die();
            }
            else
            {
                lr.SetPosition(1,point.point);
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Cube") || collision.gameObject.CompareTag("Turret"))
        {
            lr.enabled = false;
        }
    }
}
