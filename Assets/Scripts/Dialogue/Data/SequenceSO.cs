using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Data
{
    [Serializable, CreateAssetMenu(fileName = "Sequence", menuName = "Dialogue/Sequence")]
    public class SequenceSO : ScriptableObject
    {
        public new string name;
        public List<Message> messages = new();
        
        private int messageIndex;

        public Message CurrentMsg => messages[messageIndex == -1 ? 0 : messageIndex];
        public Message FirstMsg => messages[0];
        public bool HasEnded => messageIndex >= messages.Count;

        public SequenceSO()
        {
            messageIndex = 0;
            
            messages.Add(new Message {character = Character.Momotaro});
        }

        public SequenceSO(Character ch)
        {
            messageIndex = 0;
            
            messages.Add(new Message {character = ch});
        }

        public void Start() => messageIndex = -1;
        public void Continue() => messageIndex++;

        public void Reset() => messageIndex = 0;

        public override string ToString()
        {
            return $"Sequence {name} [{messages.Count} messages]. " +
                   (HasEnded
                       ? "Already Ended with last Message:\n" +
                         $"{messages[^1].character} [{messages[^1].mood}]: {messages[^1].text}"
                       : $"Current Msg:\n" +
                         $"{CurrentMsg.character} [{CurrentMsg.mood}]: {CurrentMsg.text}");
        }
    }
}
