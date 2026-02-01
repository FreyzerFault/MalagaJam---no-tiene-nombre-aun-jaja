using System;
using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CrosshairUI : MonoBehaviour
    {
        [SerializeField] private Sprite baseSprite;
        [SerializeField] private Sprite interactingSprite;
        
        private Image crosshairImg;
        
        private void OnEnable()
        {
            PlayerController.Instance.interactionController.OnInteractionStart += SetInteractingSprite;
            PlayerController.Instance.interactionController.OnInteractionEnd += SetBaseSprite;
            PlayerController.Instance.interactionController.OnFocusedSomething += ShowCrosshair;
            PlayerController.Instance.interactionController.OnLostFocus += HideCrosshair;
        }

        private void OnDisable()
        {
            PlayerController.Instance.interactionController.OnInteractionStart -= SetInteractingSprite;
            PlayerController.Instance.interactionController.OnInteractionEnd -= SetBaseSprite;
            PlayerController.Instance.interactionController.OnFocusedSomething -= ShowCrosshair;
            PlayerController.Instance.interactionController.OnLostFocus -= HideCrosshair;
        }

        private void ShowCrosshair() => crosshairImg.enabled = true;
        private void HideCrosshair() => crosshairImg.enabled = false;

        private void SetBaseSprite() => crosshairImg.sprite = baseSprite;
        private void SetInteractingSprite() => crosshairImg.sprite = interactingSprite;
    }
}
