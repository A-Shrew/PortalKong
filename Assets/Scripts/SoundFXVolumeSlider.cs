using UnityEngine;

public class SoundFXVolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioSource soundfxSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject soundManager = GameObject.Find("SoundManager");
        soundfxSource = soundManager.GetComponent<Transform>().GetChild(1).GetComponent<AudioSource>();

        float startVolume = PlayerPrefs.GetFloat("soundfxVolume");
        soundfxSource.volume = startVolume;

    }

    public void OnSliderUpdate(float value)
    {
        soundfxSource.volume = value;
        PlayerPrefs.SetFloat("soundfxvolume", value);
    }
}
