using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject BluePortalObj;
    public GameObject OrangePortalObj;
    public Camera camera;
    public float scrollSpeed = 1.0f;
    public float minScale = 0.5f;
    public float maxScale = 2.0f;

    private Transform cube;
    private Portal bluePortal;
    private Portal orangePortal;
    private bool heldRight = false;
    private bool heldLeft = false;

    void Start()
    {
        bluePortal = BluePortalObj.GetComponent<Portal>();
        orangePortal = OrangePortalObj.GetComponent<Portal>();
    }
    // Update is called once per frame
    void Update()
    {
        CheckInputs();
        if (heldLeft) PreviewPortal(bluePortal);
        if (heldRight) PreviewPortal(orangePortal);
        if(cube != null)
        {
            cube.position = camera.transform.parent.position + camera.transform.parent.forward * 3.5f;
        }
    }


    private void GravityShoot()
    {
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && !hit.collider.CompareTag("Cube"))
        {
            return;
        }
        if (cube == null)
        {
            cube = hit.transform;
            cube.GetComponent<Rigidbody>().useGravity = false;
            cube.position = camera.transform.parent.position + camera.transform.parent.forward * 3.5f;
            //cube.parent = camera.transform.parent;
        }
        else
        {
            //cube.parent = null;
            cube.GetComponent<Rigidbody>().useGravity = true;
            float speed = 12f;
            Vector3 dir = camera.transform.parent.forward;
            Vector3 vel = dir * speed;
            cube.GetComponent<Rigidbody>().velocity = vel;
            cube = null;
        }
    }

    private void CheckInputs()
    {
        if (Input.GetMouseButtonDown(0) && !heldRight)
        {
            heldLeft = true;
            GravityShoot();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            bluePortal.SetPortal();
            heldLeft = false;
        }
        if (Input.GetMouseButtonDown(1) && !heldLeft)
        {
            if (cube != null)
            {
                //cube.parent = null;
                cube.GetComponent<Rigidbody>().useGravity = true;
                cube = null;
            }
            heldRight = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            orangePortal.SetPortal();
            heldRight = false;
        }
    }
    private void PreviewPortal(Portal portal)
    {

        float scroll = Input.mouseScrollDelta.y;
        SetScale(scroll, portal);
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Pintable") && !hit.collider.CompareTag("Cube"))
        {
            portal.transform.position = hit.point;
            Validate(portal);
            if (portal.Valid) portal.Preview(hit.point, hit.normal);
            else portal.Hide();
        }
    }

    private void SetScale(float scroll, Portal portal)
    {
        Vector3 current = portal.transform.localScale;
        float newScale = Mathf.Clamp(current.x + scroll * scrollSpeed, minScale, maxScale);
        portal.transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    private void Validate(Portal portal)
    {
        portal.Valid = true;
        foreach (Transform point in portal.ValidPoints)
        {
            if (!isValid(point))
            {
                portal.Valid = false;
                break;
            }
        }
    }
    private bool isValid(Transform point)
    {
        Vector3 directionToPoint = (point.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, directionToPoint, out hit) && hit.collider.CompareTag("Pintable"))
        {
            return true;
        }

        return false;
    }
}
