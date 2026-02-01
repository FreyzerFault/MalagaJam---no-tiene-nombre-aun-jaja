using System.Collections.Generic;
using Audio;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Controllers
{
    public class PlayerSfxController : MonoBehaviour
    {
        public enum SFXTag
        { MaskOn, MaskOff, OnDeath, OnRespawn }
        
        [SerializeField, SerializedDictionary("Tag", "Audio Clip")]
        private SerializedDictionary<SFXTag, AudioClip> sfxDictionary = new(new List<KeyValuePair<SFXTag, AudioClip>>()
        {
            new(SFXTag.MaskOn, null),
            new(SFXTag.MaskOff, null),
            new(SFXTag.OnDeath, null),
            new(SFXTag.OnRespawn, null),
        });

        public void PlayAudio(SFXTag tag) => AudioManager.Instance.PlaySFX(sfxDictionary[tag]);
    }
}