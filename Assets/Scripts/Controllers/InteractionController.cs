using System;
using Interactibles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class InteractionController: MonoBehaviour
    {
        private static LayerMask InteractibleLayerMask => LayerMask.GetMask("Interactable");
        private static Camera Cam => Camera.main;
        
        public Interactible focusedInteractible;

        public event Action OnFocusedSomething;
        public event Action OnLostFocus;
        public event Action OnInteractionStart;
        public event Action OnInteractionEnd;

        private void Update()
        {
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, 10f, InteractibleLayerMask.value))
            {
                Interactible interactible = hit.transform.GetComponent<Interactible>();
                
                if (!interactible) return;
                
                if (focusedInteractible != interactible)
                    LoseFocus();
                
                Focus(interactible);
            }
            else // No mira ningun Interactuable
            {
                if (focusedInteractible)
                    LoseFocus();
            }
        }

        private void Focus(Interactible interactible)
        {
            focusedInteractible = interactible;
            
            if (!interactible.IsFocused)
            {
                interactible.SwitchState(Interactible.FocusState);
                OnFocusedSomething?.Invoke();
            }
        }
        
        private void LoseFocus()
        {
            if (!focusedInteractible) return;
            
            focusedInteractible.SwitchState(Interactible.ActiveState);
            focusedInteractible = null;
            OnLostFocus?.Invoke();
        }

        private void OnInteract(InputValue value)
        {
            if (focusedInteractible == null) return;
            if (value.isPressed)
            {
                focusedInteractible.SwitchState(Interactible.InteractingState);
                OnInteractionStart?.Invoke();
            }
            else
            {
                focusedInteractible.SwitchState(Interactible.FocusState);
                OnInteractionEnd?.Invoke();
            }
        }
    }
}