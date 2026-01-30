using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCamController : MonoBehaviour
{
    public bool lockCursor = true;
    public float mouseSensitivity = 100f;
    public float maxPitch = 70f;
        
    private Camera cam;

    private Vector2 lookInput;
    private float xRotation;

    private void Awake()
    {
        cam = Camera.main;
        UpdateLockState();
    } 
        
    private void Update()
    {
        lookInput *= mouseSensitivity * Time.deltaTime;
        xRotation -= lookInput.y;
        xRotation = Mathf.Clamp(xRotation, -maxPitch, maxPitch);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(transform.up * lookInput.x);
    }
        
    private void UpdateLockState()
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }

    // INPUT
    private void OnLook(InputValue value) => lookInput = value.Get<Vector2>();
}
