using System;
using System.Linq;
using Controllers;
using DG.Tweening;
using Dialogue;
using Dialogue.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utils;
using Character = Dialogue.Data.Character;

namespace UI
{
    public class HUDManager : Singleton<HUDManager>
    {
        [SerializeField] private GameObject debugPanel;

        protected override void Awake()
        {
            base.Awake();
            Debug.Log("AWAKE" + GetInstanceID(), this);

            inputPanels = GetComponentsInChildren<InputPanel>();
        }

        private void Start()
        {
            Debug.Log("START" + GetInstanceID(), this);
            ResetHUD();
        }

        private void OnDestroy()
        {
            Debug.Log("ONDESTROY" + GetInstanceID(), this);
        }

        private void OnEnable()
        {
            Debug.Log("ONENABLE" + GetInstanceID(), this);
            if (PlayerController.Instance == null) return;
            
            PlayerController.Instance.maskController.OnMaskOn += OnMaskOn;
            PlayerController.Instance.maskController.OnMaskOff += OnMaskOff;
            PlayerController.Instance.maskController.OnSanityUpdate += OnSanityUpdate;
            
            GameManager.Instance.OnMaskEnable += ActivateMaskInput;
            GameManager.Instance.OnFragmentCollected += UpdateMaskFragments;

            DialogueManager.Instance.OnDialogueStart += OnDialogueStart;
            DialogueManager.Instance.OnDialogueContinue += OnDialogueContinue;
            DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;
        }
        private void OnDisable()
        {
            Debug.Log("ONDISABLE" + GetInstanceID(), this);
            if (PlayerController.Instance == null) return;
            
            PlayerController.Instance.maskController.OnMaskOn -= OnMaskOn;
            PlayerController.Instance.maskController.OnMaskOff -= OnMaskOff;
            PlayerController.Instance.maskController.OnSanityUpdate -= OnSanityUpdate;
            
            GameManager.Instance.OnMaskEnable -= ActivateMaskInput;
            GameManager.Instance.OnFragmentCollected -= UpdateMaskFragments;
            
            DialogueManager.Instance.OnDialogueStart -= OnDialogueStart;
            DialogueManager.Instance.OnDialogueContinue -= OnDialogueContinue;
            DialogueManager.Instance.OnDialogueEnd -= OnDialogueEnd;
        }

        private void ResetHUD()
        {
            UpdateMaskFragments();
            DeactivateMaskInput();
            HideDialogue();
            ResetMaskImage();
            
            debugPanel.SetActive(GameManager.Instance.debugMode); 
        }

        
        #region INPUT PANEL

        public enum InputTypes { Mask, Interact }
        
        // Panel de Inputs (poner máscara, y lo que sea...)
        [BoxGroup("Inputs"), SerializeField] private InputPanel[] inputPanels;
        
        
        public void ToggleInput(InputTypes inputType, bool active)
        {
            int inputIndex = inputType switch
            {
                InputTypes.Mask => 0,
                InputTypes.Interact => 1,
                _ => 0
            };
            inputPanels[inputIndex].gameObject.SetActive(active);
        }

        public void ActivateMaskInput() => ToggleInput(InputTypes.Mask, true);
        public void DeactivateMaskInput() => ToggleInput(InputTypes.Mask, false);

        private void OnMaskOn()
        {
            ShowMask();
            
            if (DialogueManager.Instance.DialogueOnCourse)
                SwapSprite();
        }

        private void OnMaskOff()
        {
            HideMask();
            if (DialogueManager.Instance.DialogueOnCourse)
                SwapSprite();
        }

        #endregion
        
        
        #region MASK FRAGMENTS

        // Fragmentos de Máscara Malvada
        [BoxGroup("Mask Fragments"), SerializeField] private Sprite[] maskFragmentsSprites;
        [BoxGroup("Mask Fragments"), SerializeField] private Image maskFragmentsSprite;
        
        private static int NumMasks => GameManager.Instance.maskFragments;

        private void UpdateMaskFragments()
        {
            if (NumMasks == 0)
                maskFragmentsSprite.color = new Color(0, 0, 0, 0);
            else
            {
                maskFragmentsSprite.color = Color.white;
                maskFragmentsSprite.sprite = maskFragmentsSprites[NumMasks - 1];
            }
        }

        #endregion
        

        #region DIALOGUE
        
        // DIALOGUE
        [BoxGroup("Dialogue"), SerializeField] private GameObject dialoguePanel;
        [BoxGroup("Dialogue"), SerializeField] private Image characterImage;
        [BoxGroup("Dialogue"), SerializeField] private Image characterImageAux;
        [BoxGroup("Dialogue"), SerializeField] private TMP_Text dialogueTxt;
        [BoxGroup("Dialogue"), SerializeField] private Image skipDialogueIcon;
        [BoxGroup("Dialogue"), SerializeField] private float spriteJumpEffectPower;

        private void OnDialogueStart(SequenceSO sequenceSO) => ShowDialogue(sequenceSO.FirstMsg);
        private void OnDialogueContinue(Message message) => UpdateDialogue(message);
        private void OnDialogueEnd(SequenceSO sequenceSO) => HideDialogue();
        
        public void HideDialogue() =>
            dialoguePanel.SetActive(false);
        
        public void ShowDialogue(Message msg)
        {
            dialoguePanel.SetActive(true);
            UpdateDialogue(msg);
        }

        public void ToggleDialogue(bool value) => dialoguePanel.SetActive(value);

        public void UpdateDialogue (Message msg) {
            SwapSprite();
            
            dialogueTxt.text = msg.Text;

            // TODO No funciona esta animacion de salto
            characterImage.rectTransform.parent.GetComponent<RectTransform>()
                .DOJumpAnchorPos(characterImage.rectTransform.anchoredPosition + Vector2.up, spriteJumpEffectPower, 1, .3f).Play();

            // Si no es auto se puede skippear => Mostramos el icono de skipeo
            skipDialogueIcon.enabled = !msg.IsAuto;
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


        #region MASK IMAGE
        
        [BoxGroup("Mask"), SerializeField] private Image maskImg;
        [BoxGroup("Mask"), SerializeField] private Image maskVFXImg;
        
        [BoxGroup("Mask"), SerializeField] private float maskFadeDuration = .3f;
        [BoxGroup("Mask"), SerializeField] private float maskFadeOffset = 4000;

        private Tweener maskFadeTween;
        private Tweener maskVFXFadeTween;
        private Tweener maskPlaceTween;
        private Tweener maskVFXPlaceTween;
        private bool IsFading => maskPlaceTween != null && maskPlaceTween.IsActive() && maskPlaceTween.IsPlaying();

        private void OnSanityUpdate(float sanity)
        {
            float sanityPercent = sanity / PlayerController.Instance.maskController.maxSanity;
            maskVFXImg.material.SetFloat(EffectIntensityID, 1 - sanityPercent);
            UpdateSanityTxt(sanity);
        }

        private void ShowMask()
        {
            if (IsFading) // Lo para y empieza el Fade correspondiente
            {
                // maskFadeTween.Kill();
                maskVFXFadeTween.Kill();
                maskPlaceTween.Kill();
                maskVFXPlaceTween.Kill();
            }
            
            // maskFadeTween = FadeInTween(maskImg, maskFadeDuration).Play();
            maskPlaceTween = PlaceInTween(maskImg, maskFadeDuration).Play();
            maskVFXFadeTween = IncreaseIntensityTween(maskVFXImg.material, maskFadeDuration).Play();
            maskVFXPlaceTween = PlaceInTween(maskVFXImg, maskFadeDuration).Play();
        }

        private void HideMask()
        {
            if (IsFading) // Lo para y empieza el Fade correspondiente
            {
                // maskFadeTween.Kill();
                maskVFXFadeTween.Kill();
                maskPlaceTween.Kill();
                maskVFXPlaceTween.Kill();
            }
            
            // maskFadeTween = FadeOutTween(maskImg, maskFadeDuration).Play();
            maskPlaceTween = PlaceOutTween(maskImg, maskFadeDuration).Play();
            maskVFXFadeTween = DecreaseIntensityTween(maskVFXImg.material, maskFadeDuration).Play();
            maskVFXPlaceTween = PlaceOutTween(maskVFXImg, maskFadeDuration).Play();
        }

        private void ResetMaskImage()
        {
            // maskImg.color = new Color(maskImg.color.r, maskImg.color.g, maskImg.color.b, 0);
            maskVFXImg.material.SetFloat(EffectIntensityID, 0);
            maskImg.rectTransform.anchoredPosition = new Vector2(maskImg.rectTransform.anchoredPosition.x, maskFadeOffset);
            maskVFXImg.rectTransform.anchoredPosition = new Vector2(maskImg.rectTransform.anchoredPosition.x, maskFadeOffset);
        }

        private Tweener FadeInTween(Image img, float duration = .3f) => img.DOFade(endValue: 1, duration);
        private Tweener FadeOutTween(Image img, float duration = .3f) => img.DOFade(endValue: 0, duration);
        private Tweener PlaceInTween(Image img, float duration = .3f) => img.rectTransform.DOAnchorPosY(endValue: 0, duration);
        private Tweener PlaceOutTween(Image img, float duration = .3f) => img.rectTransform.DOAnchorPosY(endValue: maskFadeOffset, duration);
        
        
        private static readonly int EffectIntensityID = Shader.PropertyToID("_EffectIntensity");
        
        private Tweener IncreaseIntensityTween(Material mat, float duration = .3f) =>
            DOTween.To(
                () => mat.GetFloat(EffectIntensityID), 
                intensity => mat.SetFloat(EffectIntensityID, intensity), 
                1, duration);

        private Tweener DecreaseIntensityTween(Material mat, float duration = .3f) =>
            DOTween.To(
                () => mat.GetFloat(EffectIntensityID),
                intensity => mat.SetFloat(EffectIntensityID, intensity),
                0, duration);

        #endregion


        #region DEBUGGING

        [SerializeField] private TMP_Text sanityValueTxt;

        private void UpdateSanityTxt(float sanity) =>
            sanityValueTxt.text = $"{sanity:F0} / {PlayerController.Instance.maskController.maxSanity}";

        #endregion
    }
}
