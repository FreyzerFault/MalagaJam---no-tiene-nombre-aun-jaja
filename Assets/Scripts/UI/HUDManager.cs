using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class HUDManager : Singleton<HUDManager>
    {
        private void Start()
        {
            UpdateMaskFragments();
            ToggleInput(InputTypes.Mask, false);
        }

        #region INPUT PANEL

        public enum InputTypes { Mask, Interact }
        
        // Panel de Inputs (poner máscara, y lo que sea...)
        [SerializeField] private GameObject[] inputPanels;
        
        public void ToggleInput(InputTypes inputType, bool active)
        {
            inputPanels[inputType switch
            {
                InputTypes.Mask => 0,
                InputTypes.Interact => 1,
                _ => 0
            }].SetActive(active);
        }

        #endregion
        
        
        #region MASK FRAGMENTS

        // Fragmentos de Máscara Malvada
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Image maskFragmentsSprite;
        
        private static int NumMasks => GameManager.Instance.maskFragments;

        public void UpdateMaskFragments()
        {
            if (NumMasks == 0)
                maskFragmentsSprite.color = new Color(0, 0, 0, 0);
            else
            {
                maskFragmentsSprite.color = Color.white;
                maskFragmentsSprite.sprite = sprites[NumMasks - 1];
            }
        }

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
