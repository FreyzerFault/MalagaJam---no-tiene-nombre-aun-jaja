using Dialogue;
using UnityEngine;
using Dialogue.Dialogue;

namespace Characters
{
    public abstract class AnimalCharacter : MonoBehaviour
    {
        public DialogueSequence sequence;
        
        public virtual void OnPlayerNear()
        {
            if (!DialogueManager.Instance.dialogueOnCourse) 
                DialogueManager.Instance.StartDialogue(sequence);
        }

        public void OnMaskOff()
        {
            //quitar dibujo
        }

        public abstract void OnDialogueEnd();
    }
}
