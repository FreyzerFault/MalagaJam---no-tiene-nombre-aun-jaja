using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "DialogueSequence", menuName = "Matsuri/DialogueSequence")]
    public class DialogueSequence : ScriptableObject
    {
        public List<global::Dialogue.Dialogue> dialogues = new();
        
        [HideInInspector] public bool hasEnded;
        
        public event Action OnStart;
        public event Action OnEnded;
        
        private void Awake() => AddDefaultDialogue();

        private void AddDefaultDialogue() => dialogues.Add(
            new global::Dialogue.Dialogue {
                character = global::Dialogue.Dialogue.Character.Momotaro,
                mood =  global::Dialogue.Dialogue.Mood.Default,
                text = "Texto del Dialogo"
            });


        public void Start() => OnStart?.Invoke();

        public void End()
        {
            hasEnded = true;
            OnEnded?.Invoke();
        }
    }
}