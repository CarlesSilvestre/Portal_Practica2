using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform playerCamera;
    public List<Transform> ValidPoints;
    public List<Material> materials;
    public MeshRenderer rend;
    public Portal otherPortal;
    public Transform teleport;
    public Camera camera;
    private MeshCollider collider;
    private Collider trigger;
    private MeshRenderer childRend;
    private bool valid = false;
    private bool set = false;

    public bool Valid { get => valid; set => valid = value; }
    public bool Set { get => set; set => set = value; }

    private void Start()
    {
        collider = GetComponent<MeshCollider>();
        trigger = GetComponent<BoxCollider>();
        childRend = teleport.GetComponent<MeshRenderer>();
        Hide();
    }
    private void Update()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        childRend.enabled = otherPortal.Set && valid;
        Vector3 playerPos = playerCamera.position;
        Vector3 playerDir = playerCamera.forward;

        Vector3 relativePos = transform.InverseTransformPoint(playerPos);
        Vector3 relativeDir = transform.InverseTransformDirection(playerDir);

        relativePos = new Vector3(-relativePos.x, relativePos.y, -relativePos.z);
        otherPortal.SetCameraPositionAndRotation(relativePos, relativeDir);
    }

    private void SetCameraPositionAndRotation(Vector3 relativePos, Vector3 relativeDir)
    {
        camera.transform.position = transform.TransformPoint(relativePos);
        camera.transform.rotation = Quaternion.LookRotation(relativeDir, Vector3.up);
    }

    public void SetSprite(int i)
    {
        rend.material = materials[i];
    }

    internal void Preview(Vector3 position, Vector3 normal)
    {
        rend.enabled = true;
        Quaternion rotation = Quaternion.LookRotation(-normal);

        transform.rotation = rotation;
        transform.position = position + normal * 0.02f;
    }

    internal void Hide()
    {
        this.rend.enabled = false;
        childRend.enabled = false;
        collider.enabled = false;
        trigger.enabled = false;
    }

    internal void SetPortal()
    {
        if (!valid) return;

        set = true;
        collider.enabled = true;
        trigger.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!otherPortal.Set) return;
        Transform go = other.transform;
        Rigidbody rb = go.GetComponent<Rigidbody>();
        if (other.gameObject.tag.Equals("Cube"))
        {
            rb.velocity = Vector3.zero;
            go.localScale = new Vector3(otherPortal.transform.localScale.x, otherPortal.transform.localScale.y, otherPortal.transform.localScale.z);
            go.transform.position = otherPortal.transform.position + otherPortal.transform.forward * -((go.localScale.magnitude/3)*2);
        }
        else
        { 
            
            CharacterController cc = go.GetComponent<CharacterController>();
            Vector3 vel = rb != null ? rb.velocity : Vector3.zero;
            Vector3 dir = go.forward;
            Vector3 relativeDir = transform.InverseTransformDirection(dir);

            if (cc != null)
                cc.enabled = false;
            go.transform.position = otherPortal.teleport.position + otherPortal.teleport.forward * -2f;
            if (cc != null)
                cc.enabled = true;

            if (rb != null)
                rb.velocity = otherPortal.teleport.TransformDirection(vel);
            camera.transform.rotation = Quaternion.LookRotation(relativeDir, Vector3.up);

            GameObject.FindGameObjectWithTag("Player").GetComponent<FPSController>().UpdateLook(Quaternion.LookRotation(-otherPortal.transform.forward));

            go.localScale = new Vector3(otherPortal.transform.localScale.x, otherPortal.transform.localScale.y, otherPortal.transform.localScale.z);
        }
    }
}
