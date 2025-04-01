using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Victory()
    {
        SceneManager.LoadScene("VictoryMenu");
    }

    public void PlayGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        SceneManager.LoadScene("LevelThreeScene"); 
    }

    public void NextLevel()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        SceneManager.LoadScene("LevelOneScene");
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
