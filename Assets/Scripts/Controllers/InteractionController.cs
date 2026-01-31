using StateDriven_FSM.States;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Controllers
{
    public class InteractionController: Singleton<InteractionController>
    {
        private static LayerMask InteractibleLayerMask => LayerMask.GetMask("Interactable");
        private static Camera Cam => Camera.main;
        
        public Interactible focusedInteractible;

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
                interactible.SwitchState(Interactible.FocusState);
        }
        
        private void LoseFocus()
        {
            if (!focusedInteractible) return;
            
            focusedInteractible.SwitchState(Interactible.ActiveState);
            focusedInteractible = null;
        }

        private void OnInteract(InputValue value)
        {
            bool holdInteractionInput = value.isPressed;
            if (focusedInteractible)
                focusedInteractible.SwitchState(holdInteractionInput
                    ? Interactible.InteractingState
                    : Interactible.FocusState);
        }
    }
}