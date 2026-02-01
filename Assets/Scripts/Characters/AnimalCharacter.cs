using Controllers;
using DG.Tweening;
using Dialogue;
using Puzzles;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Billboard))]
    public abstract class AnimalCharacter : MonoBehaviour
    {
        public DialogueSequence meetSequence;
        public DialogueSequence puzzleCompletedSequence;
        
        private SpriteRenderer sr;

        private void Awake() => sr = GetComponent<SpriteRenderer>();

        protected virtual void Start()
        {
            meetSequence.OnEnded += OnMeetDialogueEnd;
            puzzleCompletedSequence.OnEnded += OnCompletedPuzzleDialogueEnd;
            
            Visible = false;
        }

        private void OnEnable()
        {
            PlayerController.Instance.maskController.OnMaskOn += OnMaskOn;
            PlayerController.Instance.maskController.OnMaskOff += OnMaskOff;

            PuzzleManager.Instance.OnCompletedPuzzle += OnCompletedPuzzle;
        }
        private void OnDisable()
        {
            PlayerController.Instance.maskController.OnMaskOn -= OnMaskOn;
            PlayerController.Instance.maskController.OnMaskOff -= OnMaskOff;
        }

        
        
        #region DIALOGUE

        public virtual void OnPlayerNear()
        {
            if (!DialogueManager.Instance.dialogueOnCourse) 
                DialogueManager.Instance.StartDialogue(meetSequence);
        }

        protected virtual void OnMeetDialogueEnd()
        {
            if (NoPuzzleConfigured) return;

            Puzzle.Start();
        }
        
        private void OnCompletedPuzzleDialogueEnd() => GameManager.Instance.AddMaskFragment();

        #endregion

        
        #region MASK ON VISIBILITY

        // Visibilidad para solo mostrarlo cuando tienes la máscara puesta
        private void OnMaskOn() => Visible = true;
        private void OnMaskOff() => Visible = false;

        public bool Visible
        {
            get => sr.color.a > 0;
            set => sr.DOFade(value ? 1 : 0, 0.2f);
        }

        #endregion

        
        #region SEQUENCE & PUZZLE

        protected virtual PuzzleManager.PuzzleType PuzzleType => PuzzleManager.PuzzleType.None; 
        protected Puzzle Puzzle => PuzzleManager.Instance.GetPuzzle(PuzzleType);
        private bool NoPuzzleConfigured => PuzzleType == PuzzleManager.PuzzleType.None;
        
        private void OnCompletedPuzzle(PuzzleManager.PuzzleType type)
        {
            if (type != PuzzleType) return;
            
            DialogueManager.Instance.StartDialogue(puzzleCompletedSequence);
        }

        #endregion
    }
}
