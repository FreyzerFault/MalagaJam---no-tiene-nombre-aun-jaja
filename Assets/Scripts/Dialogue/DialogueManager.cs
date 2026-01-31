using System;
using Controllers;
using Utils;

public class DialogueManager : Singleton<DialogueManager>
{
    public event Action<DialogueTag> OnDialogueStart;
    public event Action<DialogueTag> OnDialogueEnd;
    
    public enum DialogueTag { None = -1, Perro, Faisan, Macaco }

    private DialogueTag currentDialogueAnimal;
    
    public bool dialogueOnCourse = false;
    public void StartDialogue()
    {
        // TODO
        // if (playerTieneQueQuedarseQuieto)
        PlayerController.Instance.enabled = false;
        
        OnDialogueStart?.Invoke(currentDialogueAnimal);
    }

    public void ContinueDialogue()
    {
        // TODO Dialogo
        // Recorrer Dialogos
        
        
        // Despues del ultimo dialogo
        EndDialogue();
    }

    public void EndDialogue()
    {
        // animals.ForEach((animal) => animal.OnDialogueEnd(currentDialogue));
        
        // TODO finalizar dialogo en UI
        
        OnDialogueEnd?.Invoke(currentDialogueAnimal);
    }
}