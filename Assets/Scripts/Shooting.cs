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
    public float minScale = 0.1f;
    public float maxScale = 3.0f;

    private Portal bluePortal;
    private Portal orangePortal;
    private bool heldRight;
    private bool heldLeft;

    private void Start()
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
    }

    private void CheckInputs()
    {
        if (Input.GetMouseButtonDown(0) && !heldRight)
        {
            heldLeft = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            bluePortal.SetPortal();
            heldLeft = false;
        }
        if (Input.GetMouseButtonDown(1) && !heldLeft)
        {
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
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        float scroll = Input.mouseScrollDelta.y;
        SetScale(scroll, portal);
        if (Physics.Raycast(ray, out hit))
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

        if (Physics.Raycast(transform.position, directionToPoint, out hit))
        {
            if (hit.collider.CompareTag("Pintable"))
            {
                return true;
            }
        }

        return false;
    }
}
