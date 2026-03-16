using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Yokai
{
    public class YokaiCardUI : MonoBehaviour
    {
        private Image img;
        private TMP_Text txt;

        private void Awake()
        {
            img = GetComponent<Image>();
            txt = GetComponentInChildren<TMP_Text>();
            
            if (!img || !txt)
                Debug.LogError("YokaiCardUI debe tener como hijos los componentes Image y TMP_Text", this);
        }

        public void UpdateData(YokaiDataSO.YokaiData data)
        {
            img.sprite = data.sprite;
            txt.text = data.yokaiType.ToString();
        }
    }
}
