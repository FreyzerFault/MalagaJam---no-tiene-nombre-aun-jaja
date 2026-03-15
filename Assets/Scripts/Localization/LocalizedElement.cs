using TMPro;
using UnityEngine;

namespace Localization
{
    public abstract class LocalizedElement : MonoBehaviour
    {
        protected abstract string Text { get; set; }

        public abstract void UpdateLanguage(Language lang);

    }
}
