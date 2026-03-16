using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class InteractionController: MonoBehaviour
    {
        private static LayerMask InteractibleLayerMask => LayerMask.GetMask("Interactible");
        private static Camera Cam => Camera.main;
        
        [SerializeField] private float range = 5f;
        
        [HideInInspector] public InteractibleObject focusedInteractible;

        public event Action OnFocusedSomething;
        public event Action OnLostFocus;
        public event Action OnInteractionStart;
        public event Action OnInteractionEnd;

        private Ray CameraRay => new(Cam.transform.position, Cam.transform.forward * range);

        private void Update()
        {
            if (Physics.Raycast(CameraRay, out RaycastHit hit, range, InteractibleLayerMask.value))
            {
                InteractibleObject interactible = hit.transform.GetComponent<InteractibleObject>();
                
                if (!interactible) return;
                
                Focus(interactible);
            }
            else // No mira ningun Interactuable
            {
                LoseFocus();
            }
        }

        private void Focus(InteractibleObject newInteractible)
        {
            if (focusedInteractible == newInteractible) return;
            
            LoseFocus();
            focusedInteractible = newInteractible;
            focusedInteractible.OnFocus();
            OnFocusedSomething?.Invoke();
        }
        
        private void LoseFocus()
        {
            if (focusedInteractible == null) return;
            
            focusedInteractible.OnLostFocus();
            focusedInteractible = null;
            OnLostFocus?.Invoke();
        }

        private void OnInteract(InputValue value)
        {
            if (focusedInteractible == null) return;

            if (value.isPressed)
            {
                focusedInteractible.OnInteract();
                OnInteractionStart?.Invoke();
            }
            else
            {
                focusedInteractible.OnEndInteraction();
                OnInteractionEnd?.Invoke();
            }
        }
    }
}
