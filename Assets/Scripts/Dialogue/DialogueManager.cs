using System;
using Audio;
using Controllers;
using Dialogue.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Dialogue
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        [SerializeField] public DialogueDataSO dialogueData;
        
        public event Action<SequenceSO> OnDialogueStart;
        public event Action<Message> OnDialogueContinue;
        public event Action<SequenceSO> OnDialogueEnd;

        private SequenceSO currentSequence;
        
        public Message CurrentMsg => currentSequence.CurrentMsg;
        public Character CurrentCharacter => CurrentMsg.character;
        public Mood CurrentMood => CurrentMsg.mood;
        private bool HasEndedSequence => currentSequence == null || currentSequence.HasEnded;
        public bool DialogueOnCourse => !HasEndedSequence;

        private Timer dialogueTimer;

        private void Start() => dialogueData.ResetProgress();

        private void Update() => dialogueTimer?.Update(Time.deltaTime);

        
        #region PLAYER CONTROL

        private void BlockPlayerMovement() => PlayerController.Instance.enabled = false;
        private void EnablePlayerMovement() => PlayerController.Instance.enabled = true;

        #endregion

        
        #region DIALOGUE FLOW

        public void StartDialogue(SequenceSO sequence)
        {
            if (sequence.HasEnded)
                Debug.LogWarning("Se está repitiendo un Diálogo que ya salió. Quizá sea un BUG");
            
            // Empezamos la secuencia de dialogo
            currentSequence = sequence;
            sequence.Start();
            
            // AUDIO de Inicio segun el personaje
            AudioManager.Instance.PlaySFX(dialogueData.GetSfx(sequence.FirstMsg.character));
            
            OnDialogueStart?.Invoke(sequence);
            
            ContinueDialogue();
        }


        private void ContinueDialogue()
        {
            if (currentSequence == null) return;
            
            currentSequence.Continue();
            
            if (HasEndedSequence)
            {
                EndDialogue();
                return;
            }
            
            if (CurrentMsg.IsAuto)
            {
                dialogueTimer = new Timer(CurrentMsg.duration);
                dialogueTimer.OnTimerEnd += ContinueDialogue;
                
                EnablePlayerMovement();
            }
            else
            {
                dialogueTimer.enabled = false;
                BlockPlayerMovement();
            }
            
            OnDialogueContinue?.Invoke(CurrentMsg);
        }
        
        private void EndDialogue()
        {
            OnDialogueEnd?.Invoke(currentSequence);
            currentSequence = null;
            
            EnablePlayerMovement();
        }

        #endregion
        

        #region INPUTS
        
        [SerializeField] private InputAction continueAction;

        private void OnEnable()
        {
            continueAction.Enable();
            continueAction.performed += OnContinue;
        }
        private void OnDisable()
        {
            continueAction.performed -= OnContinue;
            continueAction.Disable();
        }

        private void OnContinue(InputAction.CallbackContext ctx) => ContinueDialogue();

        #endregion
    }
}
