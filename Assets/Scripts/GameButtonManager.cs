using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtonManager : MonoBehaviour
{
    public GameObject ObjectivePanel;
    public void StartGame()
    {
        ObjectivePanel.SetActive(false);
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
