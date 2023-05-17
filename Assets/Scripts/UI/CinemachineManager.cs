using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class CinemachineManager : MonoBehaviour
{
    public static GameObject instance; 
    private Player player;
    private CinemachineFreeLook cam;

    private Slider sensitivitySlider;
    private Slider fovSlider;

    private TMP_Text sensitivityText;
    private TMP_Text fovText;
    // Start is called before the first frame update
    void Start()
    {
        AssignObjects();

        if (instance != null)
            Destroy(instance);

        instance = gameObject;
        DontDestroyOnLoad(this);

        LoadCameraSettings();
    }

    public void LoadCameraSettings()
    {
        AssignObjects();
        PlayerData data = player.LoadData();
        if (data != null)
        {
            cam.m_XAxis.m_MaxSpeed = data.sensitivity;
            sensitivitySlider.value = data.sensitivity;
            sensitivityText.text = (data.sensitivity / 100f).ToString();
            fovSlider.value = data.FOV;
            fovText.text = data.FOV.ToString();
            cam.m_Lens.FieldOfView = data.FOV ;
        }
        else
        {
            cam.m_XAxis.m_MaxSpeed = 150f;
            sensitivitySlider.value = 150f;
            sensitivityText.text = (150f / 100f).ToString();
            fovSlider.value = 80f;
            fovText.text = 80f.ToString();
            cam.m_Lens.FieldOfView = 80f;
        }
    }

    private void AssignObjects()
    {
        cam = GameObject.Find("CM FreeLook1").GetComponent<CinemachineFreeLook>();
        player = GameObject.Find("PlayerCat").GetComponent<Player>();

        Transform optionMenu = GameObject.Find("UI").transform.Find("PauseScreen").Find("OptionsMenu").GetComponent<Transform>();
        sensitivitySlider =optionMenu.GetChild(0).Find("Sensitivity").GetComponent<Slider>();
        fovSlider = optionMenu.GetChild(0).Find("FOV").GetComponent<Slider>();
        sensitivityText = optionMenu.Find("Values").Find("Sensitivity").GetComponent<TMP_Text>();
        fovText = optionMenu.Find("Values").Find("FOV").GetComponent<TMP_Text>();
    }

    public void LoadNewCameraListenerSettings()
    {
        sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderValueChange);
        fovSlider.onValueChanged.AddListener(OnFOVSliderValueChange);
    }

    public void OnSensitivitySliderValueChange(float value)
    {
        player.sensitivity = value;
        cam.m_XAxis.m_MaxSpeed = value;
        sensitivityText.text = (value / 100f).ToString();
        player.SaveData();
    }
    public void OnFOVSliderValueChange(float value)
    {
        player.FOV = value;
        cam.m_Lens.FieldOfView = value;
        fovText.text = value.ToString();
        player.SaveData();
    }
}
