using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class VolumeSlider : MonoBehaviour, IPointerUpHandler {
    private Slider volumeSlider;
    public TextMeshProUGUI volumeTextUI;
    public AudioMixer mixer;
    public string masterName;

    private void Start() {
        Initialize();
    }

    public void Initialize() {
        volumeSlider = GetComponent<Slider>();
        LoadVolume();
    }

    private void Reset() {
        SetVolume(1);
        PlayerPrefs.DeleteKey(masterName);
    }

    private void SetVolume(float sliderValue) {
        mixer.SetFloat(masterName, Mathf.Log10(sliderValue) * 20);
        volumeTextUI.text = (sliderValue * 100).ToString("0");
        volumeSlider.value = sliderValue;
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (PauseMenu.paused) { //otherwise triggers even when pause menu closed
            SaveVolume();
            if (masterName == "SFXVol") {
                AudioManager.instance.PlaySound(Sound.player_gunfire);
            }
        }
    }

    private void SaveVolume() {
        PlayerPrefs.SetFloat(masterName, volumeSlider.value);
    }

    private void LoadVolume() {
        SetVolume(PlayerPrefs.GetFloat(masterName, 1));
    }
}
