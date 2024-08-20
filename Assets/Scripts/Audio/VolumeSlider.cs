using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();

        // Ustawienie wartości początkowej slidera na aktualny poziom głośności
        if (AudioManager.instance != null)
        {
            slider.value = AudioManager.instance.musicSource.volume;
            slider.onValueChanged.AddListener(delegate { AudioManager.instance.SetVolume(slider.value); });
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found");
        }
    }
}
