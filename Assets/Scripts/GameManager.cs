using System;
using UnityEngine.SceneManagement;
using Utils;

public class GameManager : SingletonPersistent<GameManager>
{
    public event Action OnMaskEnable; 
    public event Action OnMaskDisable; 
    
    private void Start() => ResetGame();

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        DisableMask();
        ResetMaskFragments();
    }

    
    #region PLAYER MASK

    private bool hasMask;
    
    public bool HasMask => hasMask;
    public void EnableMask()
    {
        hasMask = true;
        OnMaskEnable?.Invoke();
    }

    public void DisableMask()
    {
        hasMask = false;
        OnMaskDisable?.Invoke();
    }

    #endregion
    

    #region MASK FRAGMENTS

    public int maskFragments;

    public void AddMaskFragment() => maskFragments++;
    public void ResetMaskFragments() => maskFragments = 0;

    #endregion

}
