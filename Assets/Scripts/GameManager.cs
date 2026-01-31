using UI;
using UnityEngine;
using Utils;

public class GameManager : Singleton<GameManager>
{
    #region PLAYER MASK

    private bool hasMask;
    
    public bool HasMask => hasMask;
    
    public void OnPlayerTakeMask() {
        hasMask = true;
        HUDManager.Instance.ShowInput(HUDManager.InputTypes.Mask);
    }

    #endregion
    

    #region MASK FRAGMENTS

    public int maskFragments;

    public void AddMaskFragment() => maskFragments++;

    #endregion


    #region PUZZLES

    public enum PuzzleType { Faisan = 0, Macaco = 1 }

    private bool[] activePuzzles;
    
    public void ActivatePuzzle(PuzzleType puzzle) => activePuzzles[(int)puzzle] = true;
    public void DeactivatePuzzle(PuzzleType puzzle) => activePuzzles[(int)puzzle] = false;

    #endregion
}
