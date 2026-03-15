using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Localization
{
    public enum Language { Spanish, English, French }
    
    [CreateAssetMenu(fileName = "LocalizationData", menuName = "Localization/LocalizationData")]
    public class LocalizationDataSO : ScriptableObject
    {
        [SerializeField, SerializedDictionary("Tag", "Translations")]
        private SerializedDictionary<string, SerializedDictionary<Language, string>> translationDictionary = new();

        public string ToLanguage(string tag, Language lang)
        {
            if (translationDictionary.TryGetValue(tag, out SerializedDictionary<Language, string> traslations))
                if (traslations.TryGetValue(lang, out string translatedText))
                    return translatedText;

            Debug.LogWarning($"Not found Translated Text for {lang} by the tag:\n{tag}");
            
            return tag + @" // translation not found \\";
        }
        
        public string ToSpanish(string tag) => ToLanguage(tag, Language.Spanish);
        public string ToEnglish(string tag) => ToLanguage(tag, Language.English);
        public string ToFrench(string tag) => ToLanguage(tag, Language.French);

        
        #region FINDING Localized Elements

        public LocalizedText[] GetAllLocalizedElementsOnScene() => 
            FindObjectsByType<LocalizedText>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
        
        public LocalizedDropdown[] GetAllLocalizedDropdownsOnScene() => 
            FindObjectsByType<LocalizedDropdown>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        
        [Button("Add All Localized Text From Scene", ButtonSizes.Medium)]
        private void AddAllLocalizedTextsFromScene()
        {
            LocalizedText[] texts = GetAllLocalizedElementsOnScene();
            foreach (LocalizedText text in texts)
            {
                if (!translationDictionary.ContainsKey(text.tag))
                    translationDictionary.Add(text.tag, new SerializedDictionary<Language, string>()
                    {
                        [Language.Spanish] = "",
                        [Language.English] = "",
                        [Language.French] = "",
                    });
            }
        }
        
        [Button("Add All Localized Dropdowns From Scene", ButtonSizes.Medium)]
        private void AddAllLocalizedDropdownsFromScene()
        {
            LocalizedDropdown[] dropdowns = GetAllLocalizedDropdownsOnScene();
            foreach (LocalizedDropdown dd in dropdowns)
            {
                if (!translationDictionary.ContainsKey(dd.tag))
                    translationDictionary.Add(dd.tag, new SerializedDictionary<Language, string>()
                    {
                        [Language.Spanish] = "Opción1 , Opción2 , Opción3",
                        [Language.English] = "Option1 , Option2 , Option3",
                        [Language.French] = "Option1 , Option2 , Option3",
                    });
            }
        }

        #endregion
    }
}
