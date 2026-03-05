using System;
using Controllers;
using DG.Tweening;
using Dialogue;
using Dialogue.Data;
using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private Image characterImage;
        [SerializeField] private Image characterImageAux;
        [SerializeField] private TMP_Text dialogueTxt;
        [SerializeField] private GameObject skipDialogueIcon;
        [SerializeField] private float spriteJumpEffectPower;

        private void Start() => ResetHUD();

        private void OnEnable()
        {
            if (PlayerController.Instance)
            {
                PlayerController.Instance.maskController.OnMaskOn += OnMaskOn;
                PlayerController.Instance.maskController.OnMaskOff += OnMaskOff;
            }
            if (DialogueManager.Instance)
            {
                DialogueManager.Instance.OnDialogueStart += OnDialogueStart;
                DialogueManager.Instance.OnDialogueContinue += OnDialogueContinue;
                DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;
            }
        }

        private void OnDisable()
        {
            if (PlayerController.Instance)
            {
                PlayerController.Instance.maskController.OnMaskOn -= OnMaskOn;
                PlayerController.Instance.maskController.OnMaskOff -= OnMaskOff;
            }
            if (DialogueManager.Instance)
            {
                DialogueManager.Instance.OnDialogueStart -= OnDialogueStart;
                DialogueManager.Instance.OnDialogueContinue -= OnDialogueContinue;
                DialogueManager.Instance.OnDialogueEnd -= OnDialogueEnd;
            }
        }
        
        private void ResetHUD() => HideDialogue();

        private void OnDialogueStart(SequenceSO sequenceSO) => ShowDialogue(sequenceSO.FirstMsg);
        private void OnDialogueContinue(Message message) => UpdateDialogue(message);
        private void OnDialogueEnd(SequenceSO sequenceSO) => HideDialogue();

        private void HideDialogue() => dialoguePanel.SetActive(false);

        private void ShowDialogue(Message msg)
        {
            dialoguePanel.SetActive(true);
            UpdateDialogue(msg);
        }

        public void ToggleDialogue(bool value) => dialoguePanel.SetActive(value);

        private void UpdateDialogue (Message msg) {
            SwapSprite();
            
            dialogueTxt.text = LocalizationManager.Instance.Language switch
            {
                Language.Spanish => msg.TextEs,
                Language.English => msg.textEn.Length == 0 ? msg.TextEs : msg.textEn,
                Language.French =>  msg.textFr.Length == 0 ? msg.TextEs : msg.textFr,
                _ => msg.TextEs
            };
            
            // TODO No funciona esta animacion de salto
            characterImage.rectTransform.parent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            characterImage.rectTransform.parent.GetComponent<RectTransform>()
                .DOJump(characterImage.rectTransform.parent.position, spriteJumpEffectPower, 1, .3f).Play();

            // Si no es auto se puede skippear => Mostramos el icono de skipeo
            skipDialogueIcon.SetActive(!msg.IsAuto);
        }
        

        #region SPRITE
        
        // Para los espíritus hay que mostrar su sprite unknown cuando no lleves la máscara
        private void OnMaskOn()
        {
            if (DialogueManager.Instance.DialogueOnCourse)
                SwapSprite();
        }
        private void OnMaskOff()
        {
            if (DialogueManager.Instance.DialogueOnCourse)
                SwapSprite();
        }

        private bool isUsingImageAux;
        private void SwapSprite()
        {
            Sprite newSprite = 
                PlayerController.Instance.maskController.IsMaskOn
                || DialogueManager.Instance.CurrentCharacter == Character.Momotaro
                    ? DialogueManager.Instance.dialogueData.GetSprite(DialogueManager.Instance.CurrentMsg)
                    : DialogueManager.Instance.dialogueData.GetUnknownSprite(DialogueManager.Instance.CurrentMsg.character);
            
            if (isUsingImageAux)
            {
                characterImageAux.DOFade(0, .3f);
                characterImage.DOFade(1, .3f);
                characterImage.sprite = newSprite;
            }
            else
            {
                characterImage.DOFade(0, .3f);
                characterImageAux.DOFade(1, .3f);
                characterImageAux.sprite = newSprite;
            }
        }

        #endregion
    }
}
