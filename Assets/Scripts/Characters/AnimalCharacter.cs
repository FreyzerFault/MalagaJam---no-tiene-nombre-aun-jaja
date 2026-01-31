using UnityEngine;
using Dialogue;

namespace Characters
{
    public abstract class AnimalCharacter : MonoBehaviour
    {
        public virtual void OnPlayerNear()
        {
            if (!DialogueManager.Instance.dialogueOnCourse) 
                DialogueManager.Instance.StartDialogue();
        }

        public void OnMaskOn()
        {
            //poner dibujo
            if (!DialogueManager.Instance.dialogueOnCourse)
            {
                DialogueManager.Instance.StartDialogue();
            }
        }

        public void OnMaskOff()
        {
            //quitar dibujo
        }

        public abstract void OnDialogueEnd(DialogueManager.DialogueTag currentDialogue);
    }
}
