using Controllers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class HUDManager : Singleton<HUDManager>
    {
        private void Start()
        {
            ResetHUD();
            MaskController.Instance.OnMaskOn += OnMaskOn;
            MaskController.Instance.OnMaskOff += OnMaskOff;
        }

        private void ResetHUD()
        {
            HideDialogue();
            UpdateMaskFragments();
            ToggleInput(InputTypes.Mask, false);

            DialogueManager.Instance.OnDialogueStart += ShowDialogue;
            DialogueManager.Instance.OnDialogueEnd += HideDialogue;
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

        private void OnMaskOn()
        {
            
        }
        
        private void OnMaskOff()
        {
            
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
        
        public void ShowDialogue(DialogueManager.DialogueTag dialogueTag = DialogueManager.DialogueTag.None) 
            => dialoguePanel.SetActive(true);
        public void HideDialogue(DialogueManager.DialogueTag dialogueTag = DialogueManager.DialogueTag.None) 
            => dialoguePanel.SetActive(false);
        
        public void ToggleDialogue(bool value, DialogueManager.DialogueTag dialogueTag = DialogueManager.DialogueTag.None) 
            => dialoguePanel.SetActive(value);
        
        #endregion
    }
}
