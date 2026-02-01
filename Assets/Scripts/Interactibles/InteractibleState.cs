using System;
using UnityEngine;

namespace Interactibles
{
    [Serializable]
    public abstract class InteractibleState<T> where T : Interactible
    {
        private const bool DebugLogState = false; // Si false no muestra nada por consola cuando cambia de Estado

        public override string ToString() => "State";

        public virtual Color Color => Color.white;
            
        public abstract void Execute(T interactible);
        
        public virtual void OnEnter(Interactible interactible)
        {
            if (DebugLogState)
                Debug.Log($"{interactible} ENTERING state {ToString()}");
        }

        public virtual void OnExit(Interactible interactible)
        {
            if (DebugLogState)
                Debug.Log($"{interactible} EXITING state {ToString()}");
        }

        
        // Brightness Animation
        
        public float brightnessValue = 0;
        public float brightIntensityAnimDuration = .5f;
    }
}
