using UnityEngine;
using Utils;

namespace Controllers
{
    public class InteractionController: Singleton<InteractionController>
    {
        private static LayerMask InteractibleLayerMask => LayerMask.GetMask("Interactable");
        private static Camera Cam => Camera.main;
        
        private void OnInteract()
        {
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, 10f, InteractibleLayerMask.value))
                hit.transform.GetComponent<IInteractable>().OnInteract();
        }
    }
}