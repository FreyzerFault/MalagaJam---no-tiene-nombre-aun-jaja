using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Dialogue.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/DialogueData")]
    public class DialogueDataSO : ScriptableObject
    {
        [SerializeField, SerializedDictionary("Character", "Dialogue")]
        private List<SequenceSO> sequences = new();

        public SequenceSO GetSequence(string sequenceName)
        {
            if (sequences.Exists(seq => seq.name == sequenceName))
                return sequences.First(seq => seq.name == sequenceName);

            Debug.LogError("No hay una sequence con nombre " + sequenceName, this);
            return null;
        }
        

        #region SFX

        [SerializeField, SerializedDictionary("Character", "SFX")]
        private SerializedDictionary<Character, AudioClip> sfxDictionary = new()
        {
            [Character.Momotaro] = null,
            [Character.Perro] = null,
            [Character.Macaco] = null,
            [Character.Faisan] = null,
            [Character.Ogro] = null,
        };

        public AudioClip GetSfx(Character ch)
        {
            if (sfxDictionary.TryGetValue(ch, out AudioClip audio))
                return audio;

            Debug.LogError($"Falta un audio SFX para el personaje {ch}");
            return null;
        }
        
        #endregion

        
        #region SPRITES

        [BoxGroup("Sprites"), SerializeField, SerializedDictionary("Character", "Sprites")]
        private SerializedDictionary<Character, SerializedDictionary<Mood, Sprite>> spriteDictionary = new()
        {
            [Character.Momotaro] = new SerializedDictionary<Mood, Sprite> { [Mood.Default] = null },
            [Character.Perro] = new SerializedDictionary<Mood, Sprite> { [Mood.Default] = null },
            [Character.Macaco] = new SerializedDictionary<Mood, Sprite> { [Mood.Default] = null },
            [Character.Faisan] = new SerializedDictionary<Mood, Sprite> { [Mood.Default] = null },
            [Character.Ogro] = new SerializedDictionary<Mood, Sprite> { [Mood.Default] = null },
        };
        
        [BoxGroup("Sprites"), SerializeField, SerializedDictionary("Character", "Unknown Sprites")]
        private SerializedDictionary<Character, Sprite> unknownSpriteDictionary = new()
        {
            [Character.Momotaro] = null,
            [Character.Perro] = null,
            [Character.Macaco] = null,
            [Character.Faisan] = null,
            [Character.Ogro] = null,
        };

        public Sprite GetSprite(Message msg) => GetSprite(msg.character, msg.mood);
        public Sprite GetSprite(Character ch, Mood mood = Mood.Default)
        {
            if (spriteDictionary.TryGetValue(ch, out SerializedDictionary<Mood, Sprite> spritesByMood))
                if (spritesByMood.TryGetValue(mood, out Sprite sprite))
                    return sprite;
                else if (spritesByMood.TryGetValue(Mood.Default, out Sprite defaultSprite))
                {
                    Debug.LogWarning($"No hay un Sprite seteado para el Personaje {ch} para el Mood {mood}");
                    return defaultSprite;
                }

            Debug.LogError($"No hay un Sprite seteado para el Personaje {ch} para el Mood {mood} " +
                           $"ni tiene el Sprite por defecto");
            return null;
        }
        
        public Sprite GetUnknownSprite(Character ch)
        {
            if (unknownSpriteDictionary.TryGetValue(ch, out Sprite sprite))
                return sprite;

            Debug.LogError($"No hay un Sprite Unknown seteado para el Personaje {ch}");
            return null;
        }
        
        #endregion

        public void ResetProgress()
        {
            foreach (SequenceSO sequence in sequences) 
                sequence.Reset();
        }
    }
}
