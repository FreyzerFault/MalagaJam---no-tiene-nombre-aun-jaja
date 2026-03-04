using System;
using System.Collections.Generic;
using Audio;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Audio;

namespace Controllers
{
    public class PlayerSfxController : MonoBehaviour
    {
        [SerializeField] private AudioSource maskSanitySfx;
        [SerializeField] private float maxPitchWhenMaskOn = 0.9f;
        [SerializeField] private float minPitchWhenMaskOn = 0.5f;
        private float defaultPitch;
        
        private MaskController maskController;

        private void Awake()
        {
            maskController = GetComponent<MaskController>();
            defaultPitch = AudioManager.Instance.MasterPitch;
        }

        private void Update()
        {
            UpdatePitch();
        }

        /// <summary>
        /// Hace que se escuche to más grave cuanta peor sanity tienes
        /// </summary>
        private void UpdatePitch()
        {
            float sanityPercent = maskController.sanity / maskController.maxSanity;
            AudioManager.Instance.MasterPitch = Mathf.Lerp(minPitchWhenMaskOn, maxPitchWhenMaskOn, sanityPercent);
        }


        #region EVENTOS

        private void OnEnable()
        {
            maskController.OnMaskOn += OnMaskOn;
            maskController.OnMaskOff += OnMaskOff;
            maskController.OnStartDeathSequence += OnDeath;
            maskController.OnEndDeathSequence += OnRespawn;
            maskController.OnSanityUpdate += OnSanityUpdate;
        }
        private void OnDisable()
        {
            maskController.OnMaskOn -= OnMaskOn;
            maskController.OnMaskOff -= OnMaskOff;
            maskController.OnStartDeathSequence -= OnDeath;
            maskController.OnEndDeathSequence -= OnRespawn;
            maskController.OnSanityUpdate -= OnSanityUpdate;
        }
        
        private void OnMaskOn() => PlayAudio(SfxTag.MaskOn);
        private void OnMaskOff() => PlayAudio(SfxTag.MaskOff);

        private void OnDeath() => PlayAudio(SfxTag.OnDeath);
        private void OnRespawn() => PlayAudio(SfxTag.OnRespawn);
        
        private void OnSanityUpdate(float sanity)
        {
            float sanityPercent = sanity / maskController.maxSanity;
            maskSanitySfx.volume = 1 - sanityPercent;
        }

        #endregion
        
        
        #region SFX

        public enum SfxTag
        { MaskOn, MaskOff, OnDeath, OnRespawn }
        
        [SerializeField, SerializedDictionary("Tag", "Audio Clip")]
        private SerializedDictionary<SfxTag, AudioClip> sfxDictionary = new(new List<KeyValuePair<SfxTag, AudioClip>>()
        {
            new(SfxTag.MaskOn, null),
            new(SfxTag.MaskOff, null),
            new(SfxTag.OnDeath, null),
            new(SfxTag.OnRespawn, null),
        });

        public void PlayAudio(SfxTag tag) => AudioManager.Instance.PlaySFX(sfxDictionary[tag]);

        #endregion

    }
}
