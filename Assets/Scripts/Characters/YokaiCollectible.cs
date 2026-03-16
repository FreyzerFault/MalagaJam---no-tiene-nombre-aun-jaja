using Controllers;
using Data;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Billboard))]
    public class YokaiCollectible : InteractibleObject
    {
        public YokaiType yokaiType;
        public YokaiDataSO yokaiData;

        private new Renderer renderer;
        private ParticleSystem collectParticles;
        private new Collider collider;

        protected override void Awake()
        {
            base.Awake();
            
            renderer = GetComponent<Renderer>();
            collectParticles = GetComponentInChildren<ParticleSystem>();
            collider = GetComponent<Collider>();
            
            if (yokaiData.IsCollected(yokaiType))
                Disable();
        }

        public override void OnInteract()
        {
            base.OnInteract();
            
            Collect();
        }

        private void Collect()
        {
            collectParticles.Play();
            Disable();
        }

        private void Disable()
        {
            renderer.enabled = false;
            collider.enabled = false;
        }
    }
}
