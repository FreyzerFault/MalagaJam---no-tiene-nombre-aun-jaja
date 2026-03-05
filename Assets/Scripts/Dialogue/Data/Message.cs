using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dialogue.Data
{
    [Serializable]
    public class Message
    {
        public Character character;
        public Mood mood;
        [TextArea(2, 6)] public string text;
        public bool auto;
        [ShowIf("auto")] public int duration;

        private string AutoTag => "[auto]";
        public bool IsAuto => auto || text.StartsWith(AutoTag);

        public string Text => IsAuto && text.StartsWith(AutoTag) ? text.Substring(AutoTag.Length, text.Length - AutoTag.Length) : text;
    }
}
