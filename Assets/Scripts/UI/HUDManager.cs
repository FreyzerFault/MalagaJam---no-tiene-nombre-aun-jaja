using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Controllers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Dialogue.Dialogue;
using TMPro;
using Character = Dialogue.Dialogue.DialogueManager.Dialogue.Character;
using Mood = Dialogue.Dialogue.DialogueManager.Dialogue.Mood;

namespace UI
{
    public class HUDManager : Singleton<HUDManager>
    {
        private void Start() => ResetHUD();

        private void OnEnable()
        {
            MaskController.Instance.OnMaskOn += OnMaskOn;
            MaskController.Instance.OnMaskOff += OnMaskOff;

            DialogueManager.Instance.OnDialogueStart += ShowDialogue;
            DialogueManager.Instance.OnDialogueContinue += UpdateDialogue;
            DialogueManager.Instance.OnDialogueEnd += HideDialogue;
        }
        private void OnDisable()
        {
            MaskController.Instance.OnMaskOn -= OnMaskOn;
            MaskController.Instance.OnMaskOff -= OnMaskOff;
            
            DialogueManager.Instance.OnDialogueStart -= ShowDialogue;
            DialogueManager.Instance.OnDialogueContinue -= UpdateDialogue;
            DialogueManager.Instance.OnDialogueEnd -= HideDialogue;
        }

        private void ResetHUD()
        {
            ResetMaskImage();
            HideDialogue();
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

        private void OnMaskOn() => HideCharacterSprite();
        private void OnMaskOff() => ShowCharacterSprite();

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
        [SerializeField] private Image characterImage;
        [SerializeField] private TMP_Text dialogueTxt;
        
        public void HideDialogue() => dialoguePanel.SetActive(false);
        public void ShowDialogue() => dialoguePanel.SetActive(true);
        
        public void ToggleDialogue(bool value) => dialoguePanel.SetActive(value);
        
        public void HideCharacterSprite() => characterImage.sprite = spriteDictionary[Character.Unknown][0];
        public void ShowCharacterSprite() => 
            GetSprite(DialogueManager.Instance.CurrentCharacter, DialogueManager.Instance.CurrentMood);
        
        [SerializeField, SerializedDictionary("Nombre", "Sprite")]
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

        private Sprite GetSprite(Character character, Mood mood) => spriteDictionary[character][(int)mood];


        public void UpdateDialogue (DialogueManager.Dialogue dialogue) {
            Character character = dialogue.character;
            Mood mood = dialogue.mood;
            string frase = dialogue.text;

            List<Sprite> spriteList = spriteDictionary[character];

            characterImage.sprite = spriteList[character == Character.Unknown ? 0 : (int)mood];
            dialogueTxt.text = frase;
        }

        #endregion


        #region MASK IMAGE

        [SerializeField] private Image maskImg;
        [SerializeField] private float maskFadeDuration = .3f;

        private Tweener maskFadeTween;
        private Tweener maskPlaceTween;
        private bool IsFading => maskFadeTween.IsPlaying();
        
        public void ShowMask()
        {
            if (IsFading) // Lo para y empieza el Fade correspondiente
            {
                maskFadeTween.Kill();
                maskPlaceTween.Kill();
            }
            
            maskFadeTween = FadeInTween.Play();
            maskPlaceTween = PlaceInTween.Play();
        }

        public void HideMask()
        {
            if (IsFading) // Lo para y empieza el Fade correspondiente
            {
                maskFadeTween.Kill();
                maskPlaceTween.Kill();
            }
            
            maskFadeTween = FadeOutTween.Play();
            maskPlaceTween = PlaceOutTween.Play();
        }

        private void ResetMaskImage()
        {
            maskImg.color = new Color(maskImg.color.r, maskImg.color.g, maskImg.color.b, 0);
            maskImg.rectTransform.anchoredPosition = new (maskImg.rectTransform.anchoredPosition.x, Screen.height);
        }

        private Tweener FadeInTween => maskImg.DOFade(endValue: 1, duration: maskFadeDuration);
        private Tweener FadeOutTween => maskImg.DOFade(endValue: 0, duration: maskFadeDuration);
        
        private Tweener PlaceInTween => maskImg.rectTransform.DOAnchorPosY(endValue: 0, duration: maskFadeDuration);
        private Tweener PlaceOutTween => maskImg.rectTransform.DOAnchorPosY(endValue: Screen.height, duration: maskFadeDuration);

        #endregion
    }
}
