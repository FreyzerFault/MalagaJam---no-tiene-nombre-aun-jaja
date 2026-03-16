using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class SettingsMenuUI : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        
        private Slider[] sliders;
        private TMP_Dropdown langDropdown;

        private const float MinVolume = -80f;
        private const float MaxVolume = 20f;

        private static float VolumeToDecibels(float volume) =>
            Mathf.Lerp(MinVolume, MaxVolume, Mathf.Log10(10 * Mathf.Pow(volume, 0.5f)));
        
        public void SetGlobalVolume(float volume) =>
            mixer.SetFloat("MasterVolume", VolumeToDecibels(volume));
        public void SetMusicVolume(float volume) => 
            mixer.SetFloat("MusicVolume", VolumeToDecibels(volume));
        public void SetSfxVolume(float volume) => 
            mixer.SetFloat("SfxVolume", VolumeToDecibels(volume));

        public void SetLanguage(int index) => LocalizationManager.Instance.Language = (Language)index;
        public void SetLanguage(Language lang) => LocalizationManager.Instance.Language = lang;
        
        private void Awake()
        {
            sliders = GetComponentsInChildren<Slider>();
            langDropdown = GetComponentInChildren<TMP_Dropdown>();

            Reset();
        }

        private void Reset() => langDropdown.value = (int)LocalizationManager.Instance.Language;
    }
}
