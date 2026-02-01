using System;
using Controllers;
using UI;
using UnityEngine;
using Utils;
namespace Dialogue{ 

namespace Dialogue
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        [Serializable]
        public struct Dialogue 
        {
            public enum Character { Macaco, Faisan, Perro, Momotaro, Ogro, Unknown }
            public enum Mood { Default = 0, Enfadado = 1, Asustado = 2, Feliz = 3, Molesto = 4, } // TODO Añadir todos
            
            public Character character;
            public Mood mood;
            public string text;
        }

        
        public event Action OnDialogueStart;
        public event Action<Dialogue> OnDialogueContinue;
        public event Action OnDialogueEnd;
    
        public enum DialogueTag { None = -1, Perro, Faisan, Macaco }

        private DialogueSequence currentDialogueSequence;
        [HideInInspector] public bool dialogueOnCourse = false;
        private int dialogueIndex;

        public Dialogue CurrentDialogue => currentDialogueSequence.dialogues[dialogueIndex];
        public Dialogue.Character CurrentCharacter => CurrentDialogue.character;
        public Dialogue.Mood CurrentMood => CurrentDialogue.mood;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
            {
                ContinueDialogue();
            }
        }

        public void StartDialogue(DialogueSequence dialogueSequence)
        {
            currentDialogueSequence = dialogueSequence;
            // TODO
            // if (playerTieneQueQuedarseQuieto)
            PlayerController.Instance.enabled = false;
            OnDialogueStart?.Invoke();
        }

        public void ContinueDialogue()
        {
            dialogueIndex++;

            // Despues del ultimo dialogo
            if (dialogueIndex >= currentDialogueSequence.dialogues.Count)
            {
                EndDialogue();
                return;
            }
            
            Dialogue dialogue = currentDialogueSequence.dialogues[dialogueIndex];
            OnDialogueContinue?.Invoke(dialogue);
        }

        public void EndDialogue()
        {
            PlayerController.Instance.enabled = true;
            OnDialogueEnd?.Invoke();
        }
    }
}
}