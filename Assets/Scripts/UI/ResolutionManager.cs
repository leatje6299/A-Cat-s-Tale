using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionManager : MonoBehaviour
{
    private TMP_Dropdown resolutionDropdown;
    private Player player;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolution;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        AssignObjects();
        LoadResolutionsSettings();
    }

    public void LoadResolutionsSettings()
    {
        AssignObjects();
        resolutions = Screen.resolutions;

        filteredResolution = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filteredResolution.Add(resolutions[i]);
            }
        }


        List<string> options = new List<string>();

        for (int i = 0; i < filteredResolution.Count; i++)
        {
            string resolutionOption = filteredResolution[i].width + "x" + filteredResolution[i].height + " " + filteredResolution[i].refreshRate + "Hz";
            options.Add(resolutionOption);
            if (filteredResolution[i].width == Screen.width && filteredResolution[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void AssignObjects()
    {
        player = GameObject.Find("PlayerCat").GetComponent<Player>();
        resolutionDropdown = GameObject.Find("UI").transform.Find("PauseScreen").Find("OptionsMenu").GetChild(0).Find("Dropdown").GetComponent<TMP_Dropdown>();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolution[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
}
