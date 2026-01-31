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

    private bool[] puzzlesActive;
    
    public void ActivatePuzzle() => puzzlesActive = new bool[puzzlesActive.Length];

    #endregion
}
