using System;
using TMPro;
using UnityEngine;

namespace Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        public new string tag = "default_tag";
        [SerializeField] private LocalizationDataSO localizationData;

        private TMP_Text txtComp;

        public string Text
        {
            get => txtComp.text;
            set => txtComp.text = value;
        }

        private void Awake() => txtComp = GetComponent<TMP_Text>();

        private void Start() => UpdateLanguage(LocalizationManager.Instance.Language); 
        
        public void UpdateLanguage(Language lang) =>
            txtComp.text = localizationData.ToLanguage(tag, lang);
    }
}
