using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Controllers
{
    public class InteractionController: Singleton<InteractionController>
    {
        private static LayerMask InteractibleLayerMask => LayerMask.GetMask("Interactable");
        private static Camera Cam => Camera.main;
        
        public Interactable focusedInteractable;

        private void Update()
        {
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, 10f, InteractibleLayerMask.value))
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if (interactable == null) return;
                
                focusedInteractable = interactable;
                
                // Activar el Focus
                if (focusedInteractable.State == Interactable.InteractableState.Base)
                {
                    interactable.OnFocus();
                }
            }
            else
            {
                // Pierde el focus si lo tenía en un Interactable
                if (focusedInteractable)
                {
                    focusedInteractable.OnFocusLost();
                    focusedInteractable = null;
                }
            }
        }

        private void OnInteract(InputValue value)
        {
            bool holdInteractionInput = value.isPressed;
            Debug.Log(holdInteractionInput ? "Interacting" : "NOT Interacting");
            if (focusedInteractable)
                focusedInteractable.State = 
                    holdInteractionInput
                        ? Interactable.InteractableState.Active 
                        : Interactable.InteractableState.Hover;
        }
    }
}