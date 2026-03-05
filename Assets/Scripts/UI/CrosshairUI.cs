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

        private void Awake()
        {
            crosshairImg = GetComponent<Image>();
            OnLostFocus();
        }

        private void OnEnable()
        {
            InteractionController intController = PlayerController.Instance?.interactionController;
            if (intController == null) return;
            intController.OnInteractionStart += OnInteractionStart;
            intController.OnInteractionEnd += OnInteractionEnd;
            intController.OnFocusedSomething += OnFocusedSomething;
            intController.OnLostFocus += OnLostFocus;
        }
        private void OnDisable()
        {
            InteractionController intController = PlayerController.Instance?.interactionController;
            if (intController == null) return;
            intController.OnInteractionStart -= OnInteractionStart;
            intController.OnInteractionEnd -= OnInteractionEnd;
            intController.OnFocusedSomething -= OnFocusedSomething;
            intController.OnLostFocus -= OnLostFocus;
        }

        private void OnFocusedSomething() => crosshairImg.enabled = true;
        private void OnLostFocus() => crosshairImg.enabled = false;

        private void OnInteractionEnd()
        {
            crosshairImg.sprite = baseSprite;
            
            if (PlayerController.Instance.interactionController.focusedInteractible == null)
                OnLostFocus();
        }

        private void OnInteractionStart() => crosshairImg.sprite = interactingSprite;
    }
}
