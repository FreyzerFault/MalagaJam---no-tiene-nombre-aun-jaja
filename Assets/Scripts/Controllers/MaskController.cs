using System;
using Dialogue;
using Dialogue.Dialogue;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Controllers
{
    public class MaskController: Singleton<MaskController>
    {
        [SerializeField] private float maxSanity = 100;
    
        private float sanity = 100;
        private bool maskOn;
    
        private float sanityDecreaseSpeed;
        private float sanityIncreaseSpeed;

        public event Action OnMaskOn;
        public event Action OnMaskOff;

        private void Start()
        {
            ResetSanity();
        }

        private void OnEnable()
        {
            DialogueManager.Instance.OnDialogueStart += OnDialogueEnd;
            HUDManager.Instance.ToggleInput(HUDManager.InputTypes.Mask, true);
        }
        private void OnDisable()
        {
            DialogueManager.Instance.OnDialogueStart -= OnDialogueEnd;
            HUDManager.Instance.ToggleInput(HUDManager.InputTypes.Mask, false);
        }

        private void OnDialogueEnd() => ResetSanity();

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

        private void PutMaskOn() => OnMaskOn?.Invoke();
        private void RemoveMask() => OnMaskOff?.Invoke();

        private void OnPutMask(InputValue value)
        {
            bool newMaskOn = value.Get<float>() > 0;
        
            if (maskOn != newMaskOn && newMaskOn)
                PutMaskOn();
            else if (maskOn != newMaskOn && !newMaskOn)
                RemoveMask();
        
            maskOn = newMaskOn;
        }
    }
}