using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource sfxMonoSource;

        [SerializeField] private AudioMixer masterMixer;

        private void Start()
        {
            sfxMonoSource.playOnAwake = false;
            sfxMonoSource.loop = false;
        }

        public void PlaySFX(AudioClip clip)
        {
            sfxMonoSource.clip = clip;
            sfxMonoSource.Play();
        }

        public float MasterPitch
        {
            get
            {
                masterMixer.GetFloat("Pitch", out float value);
                return value;
            }
            set => masterMixer.SetFloat("Pitch", value);
        }
    }
}
