using System;
using Dialogue.Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Controllers
{
    public class MaskController: MonoBehaviour
    {
        [SerializeField] private float maxSanity = 100;
    
        private float sanity = 100;
        private bool maskOn;
    
        private float sanityDecreaseSpeed;
        private float sanityIncreaseSpeed;

        public event Action OnMaskOn;
        public event Action OnMaskOff;
        public event Action<float> OnSanityUpdate;

        private void Start() => ResetSanity();

        private void OnEnable() => DialogueManager.Instance.OnDialogueStart += OnDialogueEnd;
        private void OnDisable() => DialogueManager.Instance.OnDialogueStart -= OnDialogueEnd;

        private void OnDialogueEnd() => ResetSanity();

        private void Update()
        {
            // Baja, pero NO cuando está en diálogo
            if (maskOn && !DialogueManager.Instance.dialogueOnCourse)
                DecreaseSanity(sanityDecreaseSpeed * Time.deltaTime);
        
            // Cuando no tiene la máscara sube la cordura
            if (!maskOn)
                IncreaseSanity(sanityIncreaseSpeed * Time.deltaTime);
        
            if (sanity <= 0)
                DeathSequence();
        }

        #region SANITY

        private void DecreaseSanity(float quantity)
        {
            sanity -= quantity;
            OnSanityUpdate?.Invoke(sanity);
        }

        private void IncreaseSanity(float quantity)
        {
            sanity += quantity;
            OnSanityUpdate?.Invoke(sanity);
        }

        private void ResetSanity() => sanity = maxSanity;

        #endregion
        
    
        private void DeathSequence()
        {
            // TODO Transportar al jugador despues de animacion de muerte y lo cubra la niebla
            ResetSanity();
        }

        private void PutMaskOn() => OnMaskOn?.Invoke();
        private void RemoveMask() => OnMaskOff?.Invoke();

        private void OnPutMask(InputValue value)
        {
            if (!GameManager.Instance.HasMask) return;
            
            bool newMaskOn = value.Get<float>() > 0;
        
            if (maskOn != newMaskOn && newMaskOn)
                PutMaskOn();
            else if (maskOn != newMaskOn && !newMaskOn)
                RemoveMask();
        
            maskOn = newMaskOn;
        }
    }
}