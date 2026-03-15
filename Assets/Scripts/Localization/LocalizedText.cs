using TMPro;
using UnityEngine;

namespace Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : LocalizedElement
    {
        public new string tag = "default_tag";
        [SerializeField] private LocalizationDataSO localizationData;

        private TMP_Text txtComp;

        protected override string Text
        {
            get => txtComp.text;
            set => txtComp.text = value;
        }

        private void Awake() => txtComp = GetComponent<TMP_Text>();

        private void Start() => UpdateLanguage(LocalizationManager.Instance.Language); 
        
        public override void UpdateLanguage(Language lang) =>
            Text = localizationData.ToLanguage(tag, lang);
    }
}
