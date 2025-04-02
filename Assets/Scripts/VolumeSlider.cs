using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject soundManager = GameObject.Find("SoundManager");
        audioSource = soundManager.GetComponentInChildren<AudioSource>();

        float startVolume = PlayerPrefs.GetFloat("volume");
        audioSource.volume = startVolume;

    }

    public void OnSliderUpdate(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat("volume", value);
    }
}
