using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableState { Base, Hover, Active, Disable }
    
    private InteractableState state;
    public InteractableState State
    {
        get => state;
        set
        {
            state = value;
            TweenIntensity(state);
        }
    }
    
    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        state = InteractableState.Base;
    }

    public virtual void OnFocusLost() => State = InteractableState.Base; // El player deja de mirarlo
    public virtual void OnFocus() => State = InteractableState.Hover; // El player lo mira
    public virtual void OnInteract() => State = InteractableState.Active; // El player interactua
    public virtual void OnDeactivate() => State = InteractableState.Disable; // Desactivado por si bloqueamos la interaccion

    #region MATERIAL INTENSITY ANIMATION

    private static readonly int IntensityID = Shader.PropertyToID("_FresnelIntensity");

    private MeshRenderer mr;
    private Material Material => mr.sharedMaterial;

    [SerializeField, SerializedDictionary("State", "Bright Intensity")]
    private SerializedDictionary<InteractableState, float> brightnessEffectIntensityTarget = new(
        new List<KeyValuePair<InteractableState, float>>
        {
            new(InteractableState.Base, .1f),
            new(InteractableState.Hover, .6f),
            new(InteractableState.Active, .9f),
            new(InteractableState.Disable, 0f),
        });
    
    public float brightIntensityAnimDuration = .5f;

    private void TweenIntensity(InteractableState newState)
    {
        float intensityTarget = brightnessEffectIntensityTarget[newState];
        
        DOTween.To(
            () => Material.GetFloat(IntensityID),
            intensity =>  Material.SetFloat(IntensityID, intensity),
            intensityTarget, brightIntensityAnimDuration).Play();
    }

    #endregion
}