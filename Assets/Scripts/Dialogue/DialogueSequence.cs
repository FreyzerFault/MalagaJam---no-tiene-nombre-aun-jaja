using System;
using System.Collections.Generic;
using Dialogue.Dialogue;
using UnityEngine;
using Character = Dialogue.Dialogue.DialogueManager.Dialogue.Character;
using Mood = Dialogue.Dialogue.DialogueManager.Dialogue.Mood;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "DialogueSequence", menuName = "Matsuri/DialogueSequence")]
    public class DialogueSequence : ScriptableObject
    {
        public List<DialogueManager.Dialogue> dialogues = new();
        
        private void Start()
        {
            //Forma de añadir posicion
            dialogues.Add(new DialogueManager.Dialogue {
                character = Character.Macaco,
                mood =  Mood.Angry,
                text = "LOREM IPSUM MECAGO MUCHO SOCORROOOOOO"
            });
        }
    }
}