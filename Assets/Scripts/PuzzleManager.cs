using AYellowpaper.SerializedCollections;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public enum PuzzleType { Faisan = 0, Macaco = 1 }

    [SerializedDictionary("Puzzle", "Data")]
    public SerializedDictionary<PuzzleType, PuzzleSo> puzzles = new();

    private void Start() => ResetAllPuzzles();

    public void StartPuzzle(PuzzleType puzzleType) => puzzles[puzzleType].Start();
    public void CompletePuzzle(PuzzleType puzzle) => puzzles[puzzle].Complete();
    
    public void ResetAllPuzzles()
    {
        foreach (PuzzleSo puzzle in puzzles.Values) 
            puzzle.Reset();
    }
}