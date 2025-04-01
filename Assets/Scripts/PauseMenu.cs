using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign the UI panel in the Inspector
    public InputManager inputManager; // Reference to InputManager script
    public MonoBehaviour playerLookScript; // Reference to camera movement script (e.g., MouseLook or FirstPersonController)

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false); // Hide the menu at the start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Re-enable input processing
        if (inputManager != null)
        {
            inputManager.enabled = true;
        }

        // Re-enable player camera movement
        if (playerLookScript != null)
        {
            playerLookScript.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Disable input processing
        if (inputManager != null)
        {
            inputManager.enabled = false;
        }

        // Disable player camera movement
        if (playerLookScript != null)
        {
            playerLookScript.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale before switching scenes
        SceneManager.LoadScene("MainMenu");
    }
}
