using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Interactibles
{
    public class MaskObject : Interactible
    {
        [SerializeField, Required]
        private Light lightOnPLayerNear;

        protected override void Awake()
        {
            base.Awake();
            
            lightOnPLayerNear.enabled = false;
        }

        public void OnPlayerNear()
        {
            lightOnPLayerNear.enabled = true;
        }

        public override void SwitchState(InteractibleState<Interactible> newState)
        {
            base.SwitchState(newState);
        
            // Cuando interaccione con la mascara
            if (IsInteracting)
            {
                GameManager.Instance.EnableMask();
                DestroySequence();
            }
        }
        
        public void DestroySequence()
        {
            lightOnPLayerNear.enabled = false;
            Tweener destroyTween = transform.DOScale(0, .5f).OnComplete(() => Destroy(gameObject));
            destroyTween.Play();
        }
    }
}
