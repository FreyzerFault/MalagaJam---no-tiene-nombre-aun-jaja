using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using AYellowpaper.SerializedCollections;
using Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Dialogue
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        [SerializeField, SerializedDictionary("Animal", "SFX")]
        private SerializedDictionary<Dialogue.Character, AudioClip> characterSfxDictionary =  new(new List<KeyValuePair<Dialogue.Character, AudioClip>>()
        {
            new (Dialogue.Character.Perro, null),
            new (Dialogue.Character.Faisan, null),
            new (Dialogue.Character.Macaco, null),
            new (Dialogue.Character.Momotaro, null),
            new (Dialogue.Character.Ogro, null),
        });
        
        [HideInInspector] public bool dialogueOnCourse;
        private DialogueSequenceSO currentDialogueSequence;
        private int dialogueIndex;
        
        public event Action OnDialogueStart;
        public event Action<Dialogue> OnDialogueContinue;
        public event Action OnDialogueEnd;

        public Dialogue CurrentDialogue => currentDialogueSequence.dialogues[dialogueIndex];
        public Dialogue.Character CurrentCharacter => CurrentDialogue.character;
        public Dialogue.Mood CurrentMood => CurrentDialogue.mood;
        private bool HasEndedSequence => dialogueIndex >= currentDialogueSequence.dialogues.Count - 1;


        public void StartDialogue(DialogueSequenceSO dialogueSequence)
        {
            if (dialogueSequence.hasEnded)
                Debug.LogWarning("Se está repitiendo un Diálogo que ya salió. Quizá sea un BUG");
            
            currentDialogueSequence = dialogueSequence;
            currentDialogueSequence.Start();
            BlockPlayerMovement();
            
            // AUDIO
            AudioManager.Instance.PlaySFX(characterSfxDictionary[currentDialogueSequence.dialogues[0].character]);
            
            OnDialogueStart?.Invoke();

            dialogueIndex = -1;
            ContinueDialogue();
        }

        private void ContinueDialogue()
        {
            dialogueIndex++;

            // Despues del ultimo dialogo
            if (HasEndedSequence)
                EndDialogue();
            else
            {
                if (CurrentDialogue.IsAuto)
                    Invoke(nameof(ContinueDialogue), CurrentDialogue.duration);
                OnDialogueContinue?.Invoke(CurrentDialogue);
            }
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
            if (dialogueOnCourse && value.isPressed && !CurrentDialogue.IsAuto)
                ContinueDialogue();
        }

        #endregion
    }
}
