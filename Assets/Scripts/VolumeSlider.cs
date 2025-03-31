using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI volumeText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volumeText.text = "Volume: ";
        audioSource.volume = 0.01f;
    }

    public void OnSliderUpdate(float value)
    {
        volumeText.text = "Volume: " + value.ToString("n2");
        audioSource.volume = value;
    }
}
