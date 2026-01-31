using UnityEngine;

public class InteractionController: MonoBehaviour
{
    private LayerMask InteractibleLayerMask => LayerMask.GetMask("Interactable", "Character"); 
    private Camera Cam => Camera.main;
        
    private void Interact()
    {
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, 10f, InteractibleLayerMask.value))
            hit.transform.GetComponent<IInteractable>().OnInteract();
    }
        
    private void OnInteract() => Interact();
}