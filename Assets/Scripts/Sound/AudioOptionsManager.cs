using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioOptionsManager : MonoBehaviour
{
    public static float musicVolume { get; private set; }
    public static float soundEffectVolume { get; private set; }

    private Slider _musicSlider, _soundEffectSlider;
    private Player player;

    private TMP_Text musicVolumeText;
    private TMP_Text effectVolumeText;

    private void Start()
    {
        AssignObjects();

        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("Soundtrack");
        if(musicObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        LoadSoundSettings();
    }

    public void LoadSoundSettings()
    {
        AssignObjects();

        PlayerData data = player.LoadData();

        if(data !=null)
        {
            _musicSlider.value = data.musicVolume;
            _soundEffectSlider.value = data.soundEffectVolume;

            musicVolumeText.text = Mathf.RoundToInt(data.musicVolume * 100f).ToString();
            effectVolumeText.text = Mathf.RoundToInt(data.soundEffectVolume * 100f).ToString();
        }
        else
        {
            _musicSlider.value = 0.5f;
            _soundEffectSlider.value = 0.5f;

            musicVolumeText.text = Mathf.RoundToInt(0.5f * 100f).ToString();
            effectVolumeText.text = Mathf.RoundToInt(0.5f * 100f).ToString();
        }
        AudioManager.Instance.UpdateAudioMixerVolume();
    }

    private void AssignObjects()
    {
        player = GameObject.Find("PlayerCat").GetComponent<Player>();
        Transform optionMenu = GameObject.Find("UI").transform.Find("PauseScreen").Find("OptionsMenu").GetComponent<Transform>();

        _musicSlider = optionMenu.GetChild(0).Find("Music").GetComponent<Slider>();
        _soundEffectSlider = optionMenu.GetChild(0).Find("Sound Effects").GetComponent<Slider>();

        musicVolumeText = optionMenu.Find("Values").Find("MusicVolume").GetComponent<TMP_Text>();
        effectVolumeText = optionMenu.Find("Values").Find("EffectsVolume").GetComponent<TMP_Text>();
    }

    public void LoadNewAudioListenerSettings()
    {
        AssignObjects();
        _musicSlider.onValueChanged.AddListener(OnMusicSliderValueChange);
        _soundEffectSlider.onValueChanged.AddListener(OnSoundEffectSliderValueChange);
    }

    public void OnMusicSliderValueChange(float value)
    {
        player.musicVolume = value;
        musicVolume = value;
        musicVolumeText.text = Mathf.RoundToInt(value * 100f).ToString();
        
        AudioManager.Instance.UpdateAudioMixerVolume();
        player.SaveData();
    }
    public void OnSoundEffectSliderValueChange(float value)
    {
        player.soundEffectVolume = value;
        soundEffectVolume = value;
        effectVolumeText.text = Mathf.RoundToInt(value * 100f).ToString();
        
        AudioManager.Instance.UpdateAudioMixerVolume();
        player.SaveData();
    }
}
