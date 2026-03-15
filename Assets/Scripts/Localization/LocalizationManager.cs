using System.Collections.Generic;
using System.Linq;
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

        [ShowInInspector] private List<LocalizedElement> localizedElements;

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
            localizedElements = new List<LocalizedElement>();
            localizedElements.AddRange(localizationData.GetAllLocalizedElementsOnScene());
            localizedElements.AddRange(localizationData.GetAllLocalizedDropdownsOnScene());
            
            foreach (LocalizedElement localizedElement in
                     localizedElements.Where(localizedElement => localizedElement != null))
            {
                localizedElement.UpdateLanguage(lang);
            }
        }
        
    }
}
