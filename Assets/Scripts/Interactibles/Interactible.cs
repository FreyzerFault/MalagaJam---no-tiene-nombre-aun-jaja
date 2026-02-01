using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using Interactibles.States;
using UnityEditor;
using UnityEngine;

namespace Interactibles
{
    public class Interactible : MonoBehaviour
    {
        public static readonly InteractibleState<Interactible> ActiveState = new ActiveInteractibleState();
        public static readonly InteractibleState<Interactible> InactiveState = new InactiveInteractibleState();
        public static readonly InteractibleState<Interactible> FocusState = new FocusInteractibleState();
        public static readonly InteractibleState<Interactible> InteractingState = new InteractingInteractibleState();

        protected virtual InteractibleState<Interactible> InitialState => ActiveState;
        protected InteractibleState<Interactible> state = ActiveState;
    
        public bool IsActive => state == ActiveState || state == FocusState || state == InteractingState;
        public bool IsFocused => state == FocusState || state == InteractingState;
        public bool IsInteracting => state == InteractingState;


        public InteractibleState<Interactible> State => state;

        public virtual void SwitchState(InteractibleState<Interactible> newState)
        {
            state.OnExit(this);
            state = newState;
            state.OnEnter(this);
            
            TweenBrightIntensity();
        
        }
    
        protected virtual void Awake() => mr = GetComponent<MeshRenderer>();

        protected virtual void Start() => Reset();
        protected virtual void Update() => state.Execute(this);
        protected virtual void Reset() => SwitchState(InitialState);
    
    
        #region BRIGHTNESS ANIMATION

        private static readonly int IntensityID = Shader.PropertyToID("_FresnelIntensity");

        private MeshRenderer mr;
        private Material Material => mr.material;

        [SerializeField, SerializedDictionary("State", "Brightness Target")]
        private SerializedDictionary<string, float> brightnessTargetValues = new(
            new List<KeyValuePair<string, float>>
            {
                new("Active", 0.1f),
                new("Focus", 0.6f),
                new("Interacting", 0.8f),
                new("Inactive", 0f),
            });

        private void TweenBrightIntensity()
        {
            DOTween.To(
                () => Material.GetFloat(IntensityID),
                intensity =>  Material.SetFloat(IntensityID, intensity),
                brightnessTargetValues[state.ToString()], state.brightIntensityAnimDuration).Play();
        }

        #endregion
    
    
        #region DEBUGGING

        protected virtual void OnDrawGizmos()
        {
            Vector3 overHeadPosition = transform.position + Vector3.up * 1f;
            
#if UNITY_EDITOR
            GUIStyle style = new GUIStyle
            { 
                normal = { textColor = State.Color }, 
                fontSize = 24,
                
            };
            Handles.color = State.Color;
            Handles.Label(overHeadPosition, $"{State}", style);
#endif
        }

        #endregion
    }
}