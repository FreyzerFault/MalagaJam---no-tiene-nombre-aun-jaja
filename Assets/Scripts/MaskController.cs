using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaskController: MonoBehaviour
{
    [SerializeField] private float maxSanity = 100;
    
    private float sanity = 100;
    private bool maskOn;
    
    private float sanityDecreaseSpeed;
    private float sanityIncreaseSpeed;

    private void Start()
    {
        ResetSanity();
        DialogueManager.Instance.OnDialogueStart += _ => ResetSanity();
    }

    private void Update()
    {
        // Baja, pero NO cuando está en diálogo
        if (maskOn && !DialogueManager.Instance.dialogueOnCourse)
            sanity -= sanityDecreaseSpeed * Time.deltaTime;
        
        // Cuando no tiene la máscara sube la cordura
        if (!maskOn)
            sanity += sanityIncreaseSpeed * Time.deltaTime;
        
        if (sanity <= 0)
            DeathSequence();
    }

    private void ResetSanity() => sanity = maxSanity;
    
    private void DeathSequence()
    {
        // TODO Transportar al jugador despues de animacion de muerte y lo cubra la niebla
        ResetSanity();
    }

    private void OnPutMask()
    {
        // TODO
    }

    private void OnRemoveMask()
    {
        // TODO
    }

    private void OnPutMask(InputValue value)
    {
        Debug.Log("PUT MASK");
        bool newMaskOn = value.Get<float>() > 0;
        
        if (maskOn != newMaskOn && newMaskOn)
            OnPutMask();
        else if (maskOn != newMaskOn && !newMaskOn)
            OnRemoveMask();
        
        maskOn = newMaskOn;
    }
}