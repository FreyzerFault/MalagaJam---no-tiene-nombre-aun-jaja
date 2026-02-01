using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Utils;

namespace Puzzles
{
    public class PuzzleManager : Singleton<PuzzleManager>
    {
        public enum PuzzleType { None = -1, Faisan = 0, Macaco = 1 }

        [SerializeField, SerializedDictionary("Puzzle", "Data")]
        private SerializedDictionary<PuzzleType, Puzzle> puzzles = new(new List<KeyValuePair<PuzzleType, Puzzle>>
        {
            new(PuzzleType.None, null),
            new(PuzzleType.Faisan, new Puzzle()),
            new(PuzzleType.Macaco, new Puzzle()),
        });

        public event Action<PuzzleType> OnStartPuzzle;
        public event Action<PuzzleType> OnCompletedPuzzle;

        private void Start() => ResetAllPuzzles();

        public void StartPuzzle(PuzzleType puzzleType)
        {
            puzzles[puzzleType].Start();
            OnStartPuzzle?.Invoke(puzzleType);
        }

        public void CompletePuzzle(PuzzleType puzzleType)
        {
            puzzles[puzzleType].Complete();
            OnCompletedPuzzle?.Invoke(puzzleType);
        }

        public Puzzle GetPuzzle(PuzzleType type) => puzzles[type]; 


        #region CONCRETE METHODS

        [ContextMenu("Start Puzzle del Faisan")]
        public void StartFaisanPuzzle() => StartPuzzle(PuzzleType.Faisan);
        
        [ContextMenu("Complete Puzzle del Faisan")]
        public void CompleteFaisanPuzzle() => CompletePuzzle(PuzzleType.Faisan);
        
        [ContextMenu("Start Puzzle del Macaco")]
        public void StartMacacoPuzzle() => StartPuzzle(PuzzleType.Macaco);
        
        [ContextMenu("Complete Puzzle del Macaco")]
        public void CompleteMacacoPuzzle() => CompletePuzzle(PuzzleType.Macaco);
    
        [ContextMenu("Reset All Puzzles")]
        public void ResetAllPuzzles()
        {
            foreach (Puzzle puzzle in puzzles.Values) 
                puzzle.Reset();
        }

        #endregion
    }
}