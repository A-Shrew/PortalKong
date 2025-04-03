using UnityEngine;

public class MusicVolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject soundManager = GameObject.Find("SoundManager");
        musicSource = soundManager.GetComponent<Transform>().GetChild(0).GetComponent<AudioSource>();

        float startVolume = PlayerPrefs.GetFloat("musicVolume");
        musicSource.volume = startVolume;

    }

    public void OnSliderUpdate(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat("musicVolume", value);
    }
}
