using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Localization
{
    public class LocalizationManager : SingletonPersistent<LocalizationManager>
    {
        [SerializeField] private Language currentLanguage;
        [SerializeField] private LocalizationDataSO localizationData;

        [ShowInInspector] private LocalizedText[] localizedTexts;

        public Language Language
        {
            get => currentLanguage;
            set
            {
                currentLanguage = value;
                TranslateAllTo(currentLanguage);
            }
        }

        private void TranslateAllTo(Language lang)
        {
            localizedTexts = GetAllLocalizedTextsOnScene();
            foreach (LocalizedText localizedText in localizedTexts)
            {
                if (localizedText)
                    localizedText.UpdateLanguage(lang);
            }
        }
        
        private LocalizedText[] GetAllLocalizedTextsOnScene() => 
            FindObjectsByType<LocalizedText>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
    }
}
