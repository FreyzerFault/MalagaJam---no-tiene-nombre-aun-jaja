using System.Linq;
using TMPro;
using UnityEngine;

namespace Localization
{
    public class LocalizedDropdown : LocalizedElement
    {
        public new string tag = "default_tag";
        [SerializeField] private LocalizationDataSO localizationData;

        private TMP_Dropdown dropdown;

        protected override string Text
        {
            get => string.Join(", ", dropdown.options.Select(o => o.text));
            set { 
                string[] options = value.Split(",");
                for (int i = 0; i < dropdown.options.Count; i++)
                    dropdown.options[i].text = options[i];
                dropdown.RefreshShownValue();
            }
        }

        private void Awake() => dropdown = GetComponent<TMP_Dropdown>();

        private void Start() => UpdateLanguage(LocalizationManager.Instance.Language);


        public override void UpdateLanguage(Language lang)
        {
            Text = localizationData.ToLanguage(tag, lang);
        }
    }
}
