using System;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameManager : SingletonPersistent<GameManager>
{
    public bool debugMode;
    
    public event Action OnMaskEnable; 
    public event Action OnMaskDisable; 
    public event Action OnFragmentCollected;
    
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

    [ContextMenu("EnableMask")]
    public void EnableMask()
    {
        hasMask = true;
        OnMaskEnable?.Invoke();
    }

    [ContextMenu("DisableMask")]
    public void DisableMask()
    {
        hasMask = false;
        OnMaskDisable?.Invoke();
    }

    #endregion
    

    #region MASK FRAGMENTS

    [SerializeField] private AudioClip allMaskFragmentsCollectedSfx;

    private const int MaxMaskFragments = 3;
    [HideInInspector] public int maskFragments;

    [ContextMenu("AddMaskFragment")]
    public void AddMaskFragment()
    {
        maskFragments++;
        OnFragmentCollected?.Invoke();
        
        if (maskFragments == MaxMaskFragments)
            StartEndGameSequence();
    }

    private void StartEndGameSequence()
    {
        AudioManager.Instance.PlaySFX(allMaskFragmentsCollectedSfx);
        // TODO La niebla se disipa
        // TODO Iluminacion cambia pa que se vea mas de dia
        // TODO Spawnear Perro al lado
        // TODO Dialogo Perro final que te dira que vayas a la salida
        // TODO Dirigir al Player a la salida
    }

    public void ResetMaskFragments() => maskFragments = 0;

    #endregion

}