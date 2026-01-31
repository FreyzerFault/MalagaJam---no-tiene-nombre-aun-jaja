using System;
using UnityEngine;
using Utils;

public class DialogueManager : Singleton<DialogueManager>
{
    public event Action OnDialogueEnd;
    
    public bool dialogueOnCourse = false;
    public void StartDialogue()
    {
        
    }

    public void ContinueDialogue()
    {
        // TODO Dialogo
        
        // TERMINA:
        OnDialogueEnd?.Invoke();
    }
}