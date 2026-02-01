using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Controllers;
using DG.Tweening;
using Dialogue;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utils;
using Character = Dialogue.Dialogue.Character;
using Mood = Dialogue.Dialogue.Mood;

namespace UI
{
    public class HUDManager : Singleton<HUDManager>
    {
        private void Start() => ResetHUD();
        
        private void OnEnable()
        {
            PlayerController.Instance.maskController.OnMaskOn += OnMaskOn;
            PlayerController.Instance.maskController.OnMaskOff += OnMaskOff;
            PlayerController.Instance.maskController.OnSanityUpdate += OnSanityUpdate;
            
            GameManager.Instance.OnMaskEnable += ActivateMaskInput;
            GameManager.Instance.OnFragmentCollected += UpdateMaskFragments;

            DialogueManager.Instance.OnDialogueStart += ShowDialogue;
            DialogueManager.Instance.OnDialogueContinue += UpdateDialogue;
            DialogueManager.Instance.OnDialogueEnd += HideDialogue;
        }
        private void OnDisable()
        {
            PlayerController.Instance.maskController.OnMaskOn -= OnMaskOn;
            PlayerController.Instance.maskController.OnMaskOff -= OnMaskOff;
            PlayerController.Instance.maskController.OnSanityUpdate -= OnSanityUpdate;
            
            GameManager.Instance.OnMaskEnable -= ActivateMaskInput;
            GameManager.Instance.OnFragmentCollected -= UpdateMaskFragments;
            
            DialogueManager.Instance.OnDialogueStart -= ShowDialogue;
            DialogueManager.Instance.OnDialogueContinue -= UpdateDialogue;
            DialogueManager.Instance.OnDialogueEnd -= HideDialogue;
        }

        private void ResetHUD()
        {
            UpdateMaskFragments();
            DeactivateMaskInput();
            HideDialogue();
            ResetMaskImage();
        }

        
        #region INPUT PANEL

        public enum InputTypes { Mask, Interact }
        
        // Panel de Inputs (poner máscara, y lo que sea...)
        [BoxGroup("Inputs"), SerializeField] private GameObject[] inputPanels;
        
        
        public void ToggleInput(InputTypes inputType, bool active)
        {
            int inputIndex = inputType switch
            {
                InputTypes.Mask => 0,
                InputTypes.Interact => 1,
                _ => 0
            };
            inputPanels[inputIndex].SetActive(active);
        }

        public void ActivateMaskInput() => ToggleInput(InputTypes.Mask, true);
        public void DeactivateMaskInput() => ToggleInput(InputTypes.Mask, false);

        private void OnMaskOn()
        {
            ShowMask();
            
            if (DialogueManager.Instance.dialogueOnCourse)
                HideCharacterSprite();
        }

        private void OnMaskOff()
        {
            HideMask();
            if (DialogueManager.Instance.dialogueOnCourse)
                ShowCharacterSprite();
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
        [BoxGroup("Dialogue"), SerializeField] private TMP_Text dialogueTxt;
        
        public void HideDialogue() => dialoguePanel.SetActive(false);
        public void ShowDialogue() => dialoguePanel.SetActive(true);
        
        public void ToggleDialogue(bool value) => dialoguePanel.SetActive(value);

        #region CHARACTER SPRITE

        [BoxGroup("Dialogue"), SerializeField, SerializedDictionary("Nombre", "Sprite")]
        private SerializedDictionary<Character, List<Sprite>> spriteDictionary = new(
            new List<KeyValuePair<Character, List<Sprite>>>
            {
                new(Character.Macaco, new List<Sprite>()),
                new(Character.Faisan, new List<Sprite>()),
                new(Character.Perro, new List<Sprite>()),
                new(Character.Momotaro, new List<Sprite>()),
                new(Character.Ogro, new List<Sprite>()),
                new(Character.Unknown, new List<Sprite>())
            });

        public void HideCharacterSprite() => characterImage.sprite = spriteDictionary[Character.Unknown][0];
        public void ShowCharacterSprite() => 
            characterImage.sprite = 
                GetSprite(DialogueManager.Instance.CurrentCharacter, DialogueManager.Instance.CurrentMood);
        
        private Sprite GetSprite(Character character, Mood mood) => spriteDictionary[character][(int)mood];
        

        #endregion

        public void UpdateDialogue (Dialogue.Dialogue dialogue) {
            Character character = dialogue.character;
            Mood mood = dialogue.mood;
            string frase = dialogue.text;

            List<Sprite> spriteList = spriteDictionary[character];

            characterImage.sprite = spriteList[character == Character.Unknown ? 0 : (int)mood];
            dialogueTxt.text = frase;
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
        private bool IsFading => maskPlaceTween != null && maskPlaceTween.IsPlaying();

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
