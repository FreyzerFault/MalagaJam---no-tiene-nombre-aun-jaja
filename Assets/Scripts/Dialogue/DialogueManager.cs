using System;
using Controllers;
using Utils;
namespace Dialogue{ 
public class DialogueManager : Singleton<DialogueManager>
{
    public event Action<DialogueTag> OnDialogueStart;
    public event Action<DialogueTag> OnDialogueEnd;
    
    public enum DialogueTag { None = -1, Perro, Faisan, Macaco }

    
    public DialogueList CurrentList;
    public bool dialogueOnCourse = false;
    public void StartDialogue(DialogueList Lista)
    {
            CurrentList= Lista;
        // TODO
        // if (playerTieneQueQuedarseQuieto)
        PlayerController.Instance.enabled = false;
        
        OnDialogueStart?.Invoke(Lista);
    }

    public void ContinueDialogue()
    {
        // TODO Dialogo
        // Recorrer Dialogos
    
        if (Input.GetKeyDown(KeyCode.E)){
            index++;
        }

        // Despues del ultimo dialogo
        if (index >= CurrentList.ListaDialogos.Count()){
            EndDialogue();
            return;
        }
         
        
        Dialogue dialogue = CurrentList[index];
        HUDManager.Instance.UpdateDialogue(dialogue);
    }

    public void EndDialogue()
    {
        // animals.ForEach((animal) => animal.OnDialogueEnd(currentDialogue));
        
        // TODO finalizar dialogo en UI
        
        OnDialogueEnd?.Invoke(currentDialogueAnimal);
    }
}
}