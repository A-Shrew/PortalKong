using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager: MonoBehaviour
{
    private int level;
    private Scene currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        level = PlayerPrefs.GetInt("level");
    }
        

    public void Menu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void Victory()
    {
        CheckScene();
        if(currentScene.name == "TutorialScene")
        {
            PlayerPrefs.SetInt("level", 0);
        }
        if(currentScene.name == "LevelOneScene")
        {
            PlayerPrefs.SetInt("level", 1);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("VictoryMenu");
    }

    public void PlayGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("LevelOneScene"); 
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt("level", level + 1);
        if (PlayerPrefs.GetInt("level") == 1)
        {
            SceneManager.LoadScene("LevelOneScene");
        }
        else if (PlayerPrefs.GetInt("level") == 2)
        {
            SceneManager.LoadScene("LevelTwoScene");
        }
        else
        {
            PlayerPrefs.SetInt("level", 0);
            SceneManager.LoadScene("MainMenu");
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void TutorialMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("TutorialMenu");
    }

    public void PlayTutorial()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("button");
        SceneManager.LoadScene("TutorialScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void CheckScene()
    {
        currentScene = SceneManager.GetActiveScene();
    }
}
