using DG.Tweening;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(Collider), typeof(Outline))]
    public class InteractibleObject : MonoBehaviour
    {
        private Outline outline;
        private float outlineInitialWidth = 5f;
        
        protected virtual void Awake()
        {
            outline = GetComponent<Outline>();
            outlineInitialWidth = outline.OutlineWidth;
            outline.OutlineColor = new Color(outline.OutlineColor.r, outline.OutlineColor.g, outline.OutlineColor.b, 0);
        }

        public virtual void OnInteract() =>
            DOTween.To(() => outline.OutlineWidth, x => outline.OutlineWidth = x, outline.OutlineWidth + 5, .2f)
                .SetEase(Ease.OutQuad);
        
        public virtual void OnEndInteraction() =>
            DOTween.To(() => outline.OutlineWidth, x => outline.OutlineWidth = x, outline.OutlineWidth - 5, .2f)
                .SetEase(Ease.OutQuad);
        
        public virtual void OnFocus() =>
            DOTween.ToAlpha(() => outline.OutlineColor, x => outline.OutlineColor = x, 1, .2f)
                .SetEase(Ease.OutQuad);
        
        public virtual void OnLostFocus() => 
            DOTween.ToAlpha(() => outline.OutlineColor, x => outline.OutlineColor = x, 0, .2f)
                .SetEase(Ease.OutQuad);
    }
}
