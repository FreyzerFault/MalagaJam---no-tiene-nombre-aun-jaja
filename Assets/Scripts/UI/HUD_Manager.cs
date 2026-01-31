using UnityEngine;
using Utils;

namespace UI
{
    public class HUDManager : Singleton<HUDManager>
    {
        #region INPUT PANEL

        public enum InputTypes { Mask, Interact }
        
        // Panel de Inputs (poner máscara, y lo que sea...)
        [SerializeField] private GameObject[] inputPanels;
        
        public void ShowInput(InputTypes inputType)
        {
            inputPanels[inputType switch
            {
                InputTypes.Mask => 0,
                InputTypes.Interact => 1,
                _ => 0
            }].SetActive(true);
        }

        #endregion
        
        
        #region MASK FRAGMENTS

        // Fragmentos de Máscara Malvada
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private SpriteRenderer maskFragmentsSprite;
        
        private static int NumMasks => GameManager.Instance.maskFragments;

        public void UpdateMaskFragments() => 
            maskFragmentsSprite.sprite = NumMasks == 0 ? null : sprites[NumMasks - 1];

        #endregion
        

        #region DIALOGUE
        
        // DIALOGUE
        [SerializeField] private GameObject dialoguePanel;
        
        public void ShowDialogue() => dialoguePanel.SetActive(true);
        public void HideDialogue() => dialoguePanel.SetActive(false);
        public void ToggleDialogue(bool value) => dialoguePanel.SetActive(value);
        
        #endregion
    }
}
