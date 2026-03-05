using System;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    private void Awake() => cam = Camera.main;

    void Update()
    {
        if (cam == null)
            return;
        
        transform.rotation =
            Quaternion.LookRotation(Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized,
                cam.transform.up);
    }
}
