using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InputPanel : MonoBehaviour
    {
        [SerializeField] private Image inputImg; 
        [SerializeField] private Image symbolImg; 
        
        private Sprite[] inputSprites;
        private Sprite[] symbolSprites;

        private bool active;

        private void ToggleActive(bool activated)
        {
            active = activated;
            inputImg.sprite = inputSprites[active ? 1 : 0];
            symbolImg.sprite = symbolSprites[active ? 1 : 0];
        }
    }
}