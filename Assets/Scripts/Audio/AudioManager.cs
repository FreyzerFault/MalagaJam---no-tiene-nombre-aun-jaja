using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource musicSource;
        [ShowInInspector] private List<AudioSource> sfxSources = new();
        private AudioSource TermplateSfxSource => sfxSources[0];
        
        private void Start()
        {
            GetAllSfxSources();
            if (sfxSources.Count == 0)
                Debug.LogError("AudioManager no tiene ningun AudioSource hijo para SFX.\n" +
                               "Crea un AudioSource mínimo para usar de Template", this);
            else
                sfxSources.Add(sfxSources[0]);
        }

        
        #region SFX
        
        private bool IsSomeSfxSourceIdle => sfxSources.Exists(src => !src.isPlaying);
        private AudioSource FirstIdleSfxSource => sfxSources.First(src => !src.isPlaying);

        private void GetAllSfxSources()
        {
            musicSource = GetComponent<AudioSource>();
            sfxSources = GetComponentsInChildren<AudioSource>().ToList();
            sfxSources.Remove(musicSource);
        }
        
        public void PlaySFX(AudioClip clip, float volume = 1, float pitch = 1)
        {
            if (sfxSources == null || sfxSources.Count == 0)
                GetAllSfxSources();
            
            AudioSource usedSrc = IsSomeSfxSourceIdle ? FirstIdleSfxSource : InstantiateAdditionalSfxSource();

            PlaySound(usedSrc, clip, volume, pitch);
        }


        private static void PlaySound(AudioSource src, AudioClip clip, float volume = 1, float pitch = 1)
        {
            src.clip = clip;
            src.volume = volume;
            src.pitch = pitch;
            src.Play();
        }
        

        #endregion
        
        
        #region MIXER

        [SerializeField] private AudioMixer masterMixer;

        public float MasterPitch
        {
            get
            {
                masterMixer.GetFloat("Pitch", out float value);
                return value;
            }
            set => masterMixer.SetFloat("Pitch", value);
        }

        #endregion

        
        #region AUDIO CREATION
        
        [FoldoutGroup("TESTING"), Button("Initialize Audio Sources", ButtonSizes.Medium)]
        private void InitializeSomeAudioSources(int numSources)
        {
            for (int i = 0; i < numSources; i++) 
                InstantiateAdditionalSfxSource();
        }

        private AudioSource InstantiateAdditionalSfxSource()
        {
            // Duplicate TemplateAudioSource
            AudioSource templateSrc = TermplateSfxSource;
            AudioSource newSfxSource = Instantiate(templateSrc, transform);

            newSfxSource.clip = templateSrc.clip;
            newSfxSource.outputAudioMixerGroup = templateSrc.outputAudioMixerGroup;
            newSfxSource.loop = false;
            newSfxSource.playOnAwake = false;
            newSfxSource.volume = templateSrc.volume;
            newSfxSource.pitch = templateSrc.pitch;
            newSfxSource.spatialBlend = templateSrc.spatialBlend;
            newSfxSource.maxDistance = templateSrc.maxDistance;
            newSfxSource.minDistance = templateSrc.minDistance;
            
            sfxSources.Add(newSfxSource);
            return newSfxSource;
        }

        [FoldoutGroup("TESTING"), Button("Clear Additional Audio Sources", ButtonSizes.Medium)]
        private void ClearAdditionalAudioSources()
        {
            GetAllSfxSources();

            for (var index = 1; index < sfxSources.Count; index++)
            {
                AudioSource audioSource = sfxSources[index];
                if (Application.isPlaying)
                    Destroy(audioSource.gameObject);
                else
                    DestroyImmediate(audioSource.gameObject);
            }

            sfxSources.Clear();
        }

        #endregion
        
        
        #region TESTING

        [PropertySpace]
        [FoldoutGroup("TESTING"), Button("Test SFX", ButtonSizes.Medium)]
        private void TestAudio() => PlaySFX(Resources.Load<AudioClip>("Audio/SFX/mask_off"));

        #endregion
    }
}
