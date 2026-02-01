using System;
using Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class MaskController: MonoBehaviour
    {
        public float maxSanity = 100;
        public float sanity = 100;
    
        private bool maskOn;
        private bool isPossesed; // Poseido significa que la sanity está a 0 y la secuencia de muerte esta activa

        public bool IsMaskOn => maskOn;
    
        [SerializeField] private float sanityDecreaseSpeed = 1;
        [SerializeField] private float sanityIncreaseSpeed = 1;

        public event Action OnMaskOn;
        public event Action OnMaskOff;
        public event Action<float> OnSanityUpdate;
        public event Action OnStartDeathSequence;
        public event Action OnEndDeathSequence;

        private void Start() => ResetSanity();

        // Resetea la Sanity cada vez que inicia un diálogo
        private void OnEnable() => DialogueManager.Instance.OnDialogueStart += ResetSanity;
        private void OnDisable() => DialogueManager.Instance.OnDialogueStart -= ResetSanity;

        private void Update()
        {
            // Baja, pero NO cuando está en diálogo
            if (maskOn && !DialogueManager.Instance.dialogueOnCourse)
                DecreaseSanity(sanityDecreaseSpeed * Time.deltaTime);
        
            // Cuando no tiene la máscara sube la cordura
            if (!maskOn)
                IncreaseSanity(sanityIncreaseSpeed * Time.deltaTime);
        
            if (sanity <= 0)
                StartDeathSequence();
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

        
        #region DEATH SEQUENCE

        [SerializeField] private DialogueSequence deathDialogue;

        private void StartDeathSequence()
        {
            isPossesed = true;
            maskOn = true;
            
            OnStartDeathSequence?.Invoke();
            
            // TODO Sonido Muerte
            // TODO Niebla aparece
            DialogueManager.Instance.StartDialogue(deathDialogue);
            ResetSanity();
        }
        
        Transform SpawnPointT => GameObject.FindGameObjectWithTag("InitialPosition").transform;

        private void OnEndDeathDialogue()
        {
            transform.position = SpawnPointT.position;
            // TODO Niebla va desapareciendo
            isPossesed = false;
            maskOn = false;
            
            OnEndDeathSequence?.Invoke();
        }

        #endregion

        
        #region INPUTS

        private void PutMaskOn() => OnMaskOn?.Invoke();
        private void RemoveMask() => OnMaskOff?.Invoke();

        private void OnPutMask(InputValue value)
        {
            if (!GameManager.Instance.HasMask || isPossesed) return;
            
            bool newMaskOn = value.Get<float>() > 0;
        
            if (maskOn != newMaskOn && newMaskOn)
                PutMaskOn();
            else if (maskOn != newMaskOn && !newMaskOn)
                RemoveMask();
        
            maskOn = newMaskOn;
        }

        #endregion
    }
}