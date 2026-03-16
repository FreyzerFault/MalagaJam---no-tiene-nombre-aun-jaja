using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utils;

namespace UI
{
    public class PauseMenu : Singleton<PauseMenu>
    {
        [SerializeField] private GameObject pausePanel;

        private bool IsPaused => pausePanel.activeSelf;

        protected override void Awake()
        {
            base.Awake();
            
            Unpause();
        }

        public void Pause() => SetPause(true);
        public void Unpause() => SetPause(false);
        public void TogglePause() => SetPause(!IsPaused);

        private void SetPause(bool pause)
        {
            pausePanel.SetActive(pause);
            Time.timeScale = pause ? 0 : 1;
            Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = pause;
        }
        
        public void GoToMainMenu() => SceneManager.LoadScene(0);
        
        public void RestartGame() => GameManager.Instance.ResetGame();

        
        #region INPUTS

        [SerializeField] private InputAction pauseAction;

        private void OnEnable()
        {
            pauseAction.Enable();
            pauseAction.performed += OnPause;
        }

        private void OnDisable()
        {
            pauseAction.Disable();
            pauseAction.performed -= OnPause;
        }

        private void OnPause(InputAction.CallbackContext obj) => TogglePause();

        #endregion
        
    }
}
