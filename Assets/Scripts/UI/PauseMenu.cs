using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace UI
{
    public class PauseMenu : Singleton<PauseMenu>
    {
        [SerializeField] private GameObject pausePanel;

        private bool IsPaused => pausePanel.activeSelf;

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
                TogglePause(!IsPaused);
        }

        public void Pause() => TogglePause(true);
        public void Unpause() => TogglePause(false);

        private void TogglePause(bool pause)
        {
            pausePanel.SetActive(pause);
            Time.timeScale = pause ? 0 : 1;
        }
        
        public void QuitGame() => SceneManager.LoadScene(0);
        
        public void RestartGame()
        {
            GameManager.Instance.ResetGame();
        }
    }
}