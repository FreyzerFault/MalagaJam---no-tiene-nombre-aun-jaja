using System;
using Controllers;
using Dialogue;
using UnityEngine;
using Dialogue.Dialogue;

namespace Characters
{
    public abstract class AnimalCharacter : MonoBehaviour
    {
        public DialogueSequence sequence;
        private SpriteRenderer sr;

        private void OnEnable()
        {
            PlayerController.Instance.maskController.OnMaskOn += OnMaskOn;
            PlayerController.Instance.maskController.OnMaskOff += OnMaskOff;
        }

        private void OnDisable()
        {
            PlayerController.Instance.maskController.OnMaskOn -= OnMaskOn;
            PlayerController.Instance.maskController.OnMaskOff -= OnMaskOff;
        }

        // Empieza el Diálogo cuando se acerca el Player (entra en un Trigger)
        public virtual void OnPlayerNear()
        {
            if (!DialogueManager.Instance.dialogueOnCourse) 
                DialogueManager.Instance.StartDialogue(sequence);
        }

        // Visibilidad para solo mostrarlo cuando tienes la máscara puesta
        private void OnMaskOn() => Visible = true;
        private void OnMaskOff() => Visible = false;

        public bool Visible
        {
            get => sr.enabled;
            set => sr.enabled = value;
        }

        public abstract void OnDialogueEnd();
    }
}
