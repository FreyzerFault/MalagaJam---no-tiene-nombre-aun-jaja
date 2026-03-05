using Controllers;
using DG.Tweening;
using Dialogue;
using Puzzles;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Billboard), typeof(SpriteRenderer))]
    public abstract class AnimalCharacter : MonoBehaviour
    {
        public Dialogue.Data.SequenceSO meetSequenceSO;
        public Dialogue.Data.SequenceSO puzzleCompletedSequenceSO;
        
        private SpriteRenderer sr;

        private void Awake() => sr = GetComponent<SpriteRenderer>();

        protected virtual void Start() => Visible = false;

        private void OnEnable()
        {
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.maskController.OnMaskOn += OnMaskOn;
                PlayerController.Instance.maskController.OnMaskOff += OnMaskOff;
            }
            
            if (PuzzleManager.Instance != null)
                PuzzleManager.Instance.OnCompletedPuzzle += OnCompletedPuzzle;
            
            if (DialogueManager.Instance != null)
                DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;
        }
        private void OnDisable()
        {
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.maskController.OnMaskOn -= OnMaskOn;
                PlayerController.Instance.maskController.OnMaskOff -= OnMaskOff;
            }
            
            if (PuzzleManager.Instance != null) 
                PuzzleManager.Instance.OnCompletedPuzzle -= OnCompletedPuzzle;
            
            if (DialogueManager.Instance != null) 
                DialogueManager.Instance.OnDialogueEnd -= OnDialogueEnd;
        }

        
        #region DIALOGUE

        private void OnDialogueEnd(Dialogue.Data.SequenceSO sequenceSO)
        {
            if (sequenceSO.name == meetSequenceSO.name) OnMeetDialogueEnd();
            else if (sequenceSO.name == puzzleCompletedSequenceSO.name) OnCompletedPuzzleDialogueEnd();
        }
        
        public virtual void OnPlayerNear()
        {
            if (!DialogueManager.Instance.DialogueOnCourse && !meetSequenceSO.HasEnded) 
                DialogueManager.Instance.StartDialogue(meetSequenceSO);
        }

        protected virtual void OnMeetDialogueEnd()
        {
            if (NoPuzzleConfigured) return;

            // TODO Antes de implementar los puzzles provisionalmente activamos que completa el puzzle
            // Puzzle.Start();
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

        protected void OnCompletedPuzzle(PuzzleManager.PuzzleType type)
        {
            if (type != PuzzleType) return;
            
            DialogueManager.Instance.StartDialogue(puzzleCompletedSequenceSO);
        }

        #endregion
    }
}
