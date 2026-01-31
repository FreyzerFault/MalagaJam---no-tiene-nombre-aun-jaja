using System;
using System.Collections.Generic;
using Controllers;
using UI;
using UnityEngine.SceneManagement;
using Utils;

public class GameManager : SingletonPersistent<GameManager>
{
    private void Start()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ToggleMaskController(false);
        ResetMaskFragments();
    }

    
    #region PLAYER MASK

    public bool HasMask => MaskController.Instance.enabled;
    
    public void OnPlayerTakeMask() {
        ToggleMaskController(true);
    }
    
    public void ToggleMaskController(bool activated)
    {
        MaskController.Instance.enabled = activated;
        HUDManager.Instance.ToggleInput(HUDManager.InputTypes.Mask, activated);
    }

    #endregion
    

    #region MASK FRAGMENTS

    public int maskFragments;

    public void AddMaskFragment() => maskFragments++;
    public void ResetMaskFragments() => maskFragments = 0;

    #endregion

}
