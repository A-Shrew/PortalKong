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
    private float currentTime;
    private bool timerActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        timerActive = true;
        currentTime = 0;
    }

    // Update is called every frame
    void Update()
    {
        if (timerActive)
        {
            currentTime += Time.deltaTime;
            currentTimeText.text = currentTime.ToString("n2");
        }
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
