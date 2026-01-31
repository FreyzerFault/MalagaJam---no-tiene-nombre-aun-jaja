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
            public enum Mood { Angry = 0, Shocked = 1, } // TODO Añadir todos
            
            public Character character;
            public Mood mood;
            public string text;
        }

        
        public event Action OnDialogueStart;
        public event Action OnDialogueEnd;
    
        public enum DialogueTag { None = -1, Perro, Faisan, Macaco }

        public DialogueSequence currentDialogueSequence;
        private int dialogueIndex;
        public bool dialogueOnCourse = false;

        public Dialogue CurrentDialogue => currentDialogueSequence.dialogues[dialogueIndex];
        public Dialogue.Character CurrentCharacter => CurrentDialogue.character;
        public Dialogue.Mood CurrentMood => CurrentDialogue.mood;

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
            if (Input.GetKeyDown(KeyCode.E)){
                dialogueIndex++;
            }

            // Despues del ultimo dialogo
            if (dialogueIndex >= currentDialogueSequence.dialogues.Count)
            {
                EndDialogue();
                return;
            }
            
            Dialogue dialogue = currentDialogueSequence.dialogues[dialogueIndex];
            HUDManager.Instance.UpdateDialogue(dialogue);
        }

        public void EndDialogue()
        {
            PlayerController.Instance.enabled = true;
            HUDManager.Instance.OnEndDialogue();
            OnDialogueEnd?.Invoke();
        }
    }
}
}