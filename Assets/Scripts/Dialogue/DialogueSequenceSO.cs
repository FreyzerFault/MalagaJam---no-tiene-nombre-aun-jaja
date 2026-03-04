using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "DialogueSequence", menuName = "Matsuri/DialogueSequence")]
    public class DialogueSequenceSO : ScriptableObject
    {
        public List<Dialogue> dialogues = new();
        
        [HideInInspector] public bool hasEnded;
        
        public event Action OnStart;
        public event Action OnEnded;
        
        private void Awake() => AddDefaultDialogue();

        private void AddDefaultDialogue() => dialogues.Add(
            new Dialogue {
                character = Dialogue.Character.Momotaro,
                mood =  Dialogue.Mood.Default,
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
