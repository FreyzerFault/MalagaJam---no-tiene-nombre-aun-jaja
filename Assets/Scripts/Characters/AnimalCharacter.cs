using UnityEngine;

public abstract class AnimalCharacter : MonoBehaviour
{
    public void OnPlayerNear()
    {
        //poner interfaz dialogo sin dibujo
        //enseñardibujo1
        //enseñardibujo2
        if (!DialogueManager.Instance.dialogueOnCourse)
        {
            DialogueManager.Instance.StartDialogue();
        }
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
}
