using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    public GameObject tutorialPanel; // Assign the UI Panel in the Inspector
    public GameObject player; // Reference to the player
    public GameObject hudCanvas; // Reference to the in-game HUD Canvas

    private Vector3 initialPlayerPosition; // Store player's initial position

    private void Start()
    {
        Time.timeScale = 0f; // Pause the game while the tutorial is open
        tutorialPanel.SetActive(true);
        hudCanvas.SetActive(false); // Hide the HUD initially

        if (player != null)
        {
            initialPlayerPosition = player.transform.position; // Store initial position
            player.SetActive(false); // Hide the player until tutorial is dismissed
        }
        else
        {
            Debug.LogError("Player reference is not set in TutorialPopup script!");
        }
    }

    public void OnOkButtonPressed()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        tutorialPanel.SetActive(false);
        hudCanvas.SetActive(true); // Show the HUD when tutorial is dismissed

        if (player != null)
        {
            player.transform.position = initialPlayerPosition; // Restore player's position
            player.SetActive(true);
        }

        Time.timeScale = 1f; // Resume the game
    }
}
