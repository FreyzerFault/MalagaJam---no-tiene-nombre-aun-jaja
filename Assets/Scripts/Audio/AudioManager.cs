using UnityEngine;
using Utils;

namespace Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource sfxMonoSource;

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
    }
}