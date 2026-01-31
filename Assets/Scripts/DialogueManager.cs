using System;
using Utils;

public class DialogueManager : Singleton<DialogueManager>
{
    public event Action<AnimalType> OnDialogueStart;
    public event Action<AnimalType> OnDialogueEnd;
    
    public enum AnimalType { Perro, Faisan, Macaco }

    private AnimalType currentDialogueAnimal;
    
    public bool dialogueOnCourse = false;
    public void StartDialogue()
    {
        // TODO
        // if (playerTieneQueQuedarseQuieto)
        PlayerControllerCc.Instance.enabled = false;
        
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