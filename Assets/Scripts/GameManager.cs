using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Camera cameraA;
    public Camera cameraB;
    public Material cameraMatA;
    public Material cameraMatB;

    [SerializeField] private TextMeshProUGUI currentTimeText;
    [SerializeField] private TextMeshProUGUI dashText;
    [SerializeField] private TextMeshProUGUI slowTimeText;
    [SerializeField] private Player player;

    private float currentTime;
    private bool timerActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        timerActive = true;
        currentTime = 0;
    }

    // Update is called every frame
    void Update()
    {
        UpdateTextUI();
    }

    private void UpdateTextUI()
    {
        if (timerActive)
        {
            currentTime += Time.deltaTime;
            PlayerPrefs.SetFloat("timer", currentTime);
            currentTimeText.text = currentTime.ToString("n2");
        }

        if (player.canDash)
        {
            dashText.color = Color.green;
            dashText.text = "Dash:Ready";
        }
        else
        {
            dashText.color = Color.red;
            dashText.text = "Dash:Cooldown";
        }

        if (player.canSlowTime)
        {
            slowTimeText.color = Color.green;
            slowTimeText.text = "SlowTime:Ready";
        }
        else
        {
            slowTimeText.color = Color.red;
            slowTimeText.text = "SlowTime:Cooldown";
        }
    }

    public void PlayerWins()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("VictoryMenu");
    }

    public void PlayerDies()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("DeathMenu");
    }

    // Loads the render textures into the cameras
    public void LoadTextures()
    {
        if(cameraB.targetTexture != null)
        {
            cameraB.targetTexture.Release();
        }
        cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatB.mainTexture = cameraB.targetTexture;

        if (cameraA.targetTexture != null)
        {
            cameraA.targetTexture.Release();
        }
        cameraA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatA.mainTexture = cameraA.targetTexture;
    }
}
