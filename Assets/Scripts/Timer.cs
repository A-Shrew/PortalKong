using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(PlayerPrefs.HasKey("timer"))
        {
            timerText.text = "Time: " + PlayerPrefs.GetFloat("timer").ToString("n2");
        }
    }
}
