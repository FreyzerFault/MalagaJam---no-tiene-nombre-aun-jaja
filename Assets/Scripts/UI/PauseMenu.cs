using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace UI
{
    public class PauseMenu : Singleton<PauseMenu>
    {
        public void QuitGame() => SceneManager.LoadScene(0);
        
        public void RestartGame()
        {
            GameManager.Instance.ResetGame();
        }
    }
}