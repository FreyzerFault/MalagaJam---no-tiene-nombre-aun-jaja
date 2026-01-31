using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController: MonoBehaviour
{
    private static LayerMask InteractibleLayerMask => LayerMask.GetMask("Interactable");
    private static Camera Cam => Camera.main;

    private void Update()
    {
        // TODO Esto es una warreria pero no va el OnInteract()
        if (Input.GetMouseButtonDown(0))
            Debug.Log("BOTTON RATON");
    }

        
    private void OnInteract()
    {
        Debug.Log("INTERACTION");
        
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, 10f, InteractibleLayerMask.value))
            hit.transform.GetComponent<IInteractable>().OnInteract();
    }
}