using System;
using Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Dialogue
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        [HideInInspector] public bool dialogueOnCourse;
        private DialogueSequence currentDialogueSequence;
        private int dialogueIndex;
        
        public event Action OnDialogueStart;
        public event Action<Dialogue> OnDialogueContinue;
        public event Action OnDialogueEnd;

        private Dialogue CurrentDialogue => currentDialogueSequence.dialogues[dialogueIndex];
        public Dialogue.Character CurrentCharacter => CurrentDialogue.character;
        public Dialogue.Mood CurrentMood => CurrentDialogue.mood;
        private bool HasEndedSequence => dialogueIndex >= currentDialogueSequence.dialogues.Count;


        public void StartDialogue(DialogueSequence dialogueSequence)
        {
            if (dialogueSequence.hasEnded)
                Debug.LogWarning("Se está repitiendo un Diálogo que ya salió. Quizá sea un BUG");
            
            currentDialogueSequence = dialogueSequence;
            currentDialogueSequence.Start();
            BlockPlayerMovement();
            OnDialogueStart?.Invoke();
        }

        private void ContinueDialogue()
        {
            dialogueIndex++;

            // Despues del ultimo dialogo
            if (HasEndedSequence)
                EndDialogue();
            else
                OnDialogueContinue?.Invoke(CurrentDialogue);
        }

        private void EndDialogue()
        {
            currentDialogueSequence.End();
            currentDialogueSequence = null;
            EnablePlayerMovement();
            OnDialogueEnd?.Invoke();
        }
        
        private void BlockPlayerMovement() => PlayerController.Instance.enabled = false;
        private void EnablePlayerMovement() => PlayerController.Instance.enabled = true;


        #region INPUTS

        private void OnInteract(InputValue value)
        {
            if (value.Get<float>() > 0.1f) 
                ContinueDialogue();
        }

        #endregion
    }
}