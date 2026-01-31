using UnityEngine;

public abstract class AnimalCharacter : MonoBehaviour
{
    public void OnPlayerNear()
    {
        //poner interfaz dialogo sin dibujo
       
        if (!DialogueManager.Instance.dialogueOnCourse)
        {
            DialogueManager.Instance.StartDialogue();
        }
    }

    public void OnMaskOn()
    {

        if (!DialogueManager.Instance.dialogueOnCourse)
        {
            DialogueManager.Instance.StartDialogue();
        }
    }

    public void OnMaskOff()
    {
        //quitar dibujo
    }
}
