using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private CinemachineManager cameraSettings;
    private AudioOptionsManager audioSettings;
    private ResolutionManager resolutionSettings;

    private AudioManager audioManager;
    private CustomButton menuButton;
    private CustomButton endButton;

    private LevelSound levelSound;

    private void Start()
    {
        cameraSettings = gameObject.GetComponent<CinemachineManager>();
        audioSettings = GameObject.Find("AudioManager").GetComponent<AudioOptionsManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        resolutionSettings = gameObject.GetComponent<ResolutionManager>();

        levelSound = audioManager.GetComponent<LevelSound>();
    }
    public void SetScene(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void LoadAllSettings()
    {

        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            print("load camera settings");
            cameraSettings.LoadCameraSettings();
            cameraSettings.LoadNewCameraListenerSettings();

            audioSettings.LoadSoundSettings();
            audioSettings.LoadNewAudioListenerSettings();

            resolutionSettings.LoadResolutionsSettings();

            GameObject water = GameObject.Find("Water").gameObject;
            GameObject fan = GameObject.Find("Fan").gameObject;
            GameObject player = GameObject.Find("PlayerCat");
            GameObject playerCat = player.transform.Find("CatToon").gameObject;
            GameObject pipe = GameObject.Find("Pipe").gameObject;
            levelSound.FirstLevel(water, fan, pipe);
            levelSound.PlayerSound(player, playerCat);

            menuButton = GameObject.Find("UI").transform.Find("PauseScreen").Find("PauseMenu").Find("Layout").Find("Main Menu").GetComponent<CustomButton>();
            endButton = GameObject.Find("UI").transform.Find("End").Find("EndButton").GetComponent<CustomButton>();
            menuButton.OnEvent.AddListener(() => SetScene(0));
            endButton.OnEvent.AddListener(() => SetScene(0));

        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadAllSettings();
        print("load settings");
    }
}
