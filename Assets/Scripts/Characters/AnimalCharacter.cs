using UnityEngine;

public class AnimalCharacter : MonoBehaviour
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
