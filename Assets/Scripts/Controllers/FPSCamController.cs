using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class FPSCamController: MonoBehaviour
    {
        public bool lockCursor = true;
        public float maxPitch = 70f;
        
        [Space]
        
        // MOUSE SENSITIVITY
        [BoxGroup("Sensitivity")] public float mouseSensitivity = 100f;
        [BoxGroup("Sensitivity")] public float camSpeedMultiplierWithMask = .5f;
        private float CamSpeed => mouseSensitivity * (maskController.IsMaskOn ? camSpeedMultiplierWithMask : 1f);
        
        [Space]
        
        // ZOOM
        [BoxGroup("FoV")] public float standardFoV = 90;
        [BoxGroup("FoV")] public float zoomMaxFoV = 70;
        [BoxGroup("FoV")] public float zoomMinFoV = 40;
        [BoxGroup("FoV")] public float zoomSmoothDuration = 0.2f;
        
        private float SanityPercent => maskController.sanity / maskController.maxSanity;
        private float ZoomBySanity => Mathf.Lerp(zoomMinFoV, zoomMaxFoV, SanityPercent);
        
        private Camera cam;

        private Vector2 lookInput;
        private float xRotation;

        private MaskController maskController;

        private void Awake()
        {
            cam = Camera.main;
            maskController = GetComponent<MaskController>();
            
            cam.fieldOfView = standardFoV;
            UpdateLockState();
        }

        private void OnEnable()
        {
            maskController.OnMaskOn += OnMaskOn;
            maskController.OnMaskOff += OnMaskOff;
        }
        private void OnDisable()
        {
            maskController.OnMaskOn -= OnMaskOn;
            maskController.OnMaskOff -= OnMaskOff;
        }

        private void Update()
        {
            // Actualiza el Zoom segun la Sanity pero espera a que las animaciones de Zoom al poner o quitar la mascara terminen
            if (maskController.IsMaskOn && !zoomTween.IsPlaying()) 
                cam.fieldOfView = ZoomBySanity;
            
            lookInput *= CamSpeed * Time.deltaTime;
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

        
        #region MASK LIMITATIONS

        private Tweener zoomTween;
        private bool IsPlaying => zoomTween != null && zoomTween.IsPlaying();

        private void OnMaskOn()
        {
            // Zoom IN
            if (IsPlaying)
                zoomTween.Kill();
            
            zoomTween = cam.DOFieldOfView(ZoomBySanity, zoomSmoothDuration);
            zoomTween.Play();
        }

        private void OnMaskOff()
        {
            // Zoom OUT
            if (IsPlaying)
                zoomTween.Kill();
            
            zoomTween = cam.DOFieldOfView(standardFoV, zoomSmoothDuration);
            zoomTween.Play();
        }

        #endregion
    }
}
