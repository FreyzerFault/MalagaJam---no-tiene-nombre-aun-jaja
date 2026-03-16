using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool freeRotation;
    
    private Camera cam;

    private void Awake() => cam = Camera.main;

    private void Update()
    {
        if (cam == null)
            return;


        if (freeRotation)
            transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        else
            transform.rotation = Quaternion.LookRotation(
                Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized,
                cam.transform.up);
    }
}
