using Controllers;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class MaskObject : InteractibleObject
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

    public override void OnInteract()
    {
        base.OnInteract();
        
        GameManager.Instance.EnableMask();
        DestroySequence();
    }

    private void DestroySequence()
    {
        lightOnPLayerNear.enabled = false;
        transform.DOScale(0, .5f).SetEase(Ease.OutQuad).OnComplete(() => Destroy(gameObject));
    }
}
