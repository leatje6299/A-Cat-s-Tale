using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float FOV;
    public float sensitivity;
    public float musicVolume;
    public float soundEffectVolume;
    public bool hasDoneTutorial;
    //local data save
    public void SaveData()
    {
        SaveSystem.SavePlayerData(this);
    }

    public PlayerData LoadData()
    {
        PlayerData data = SaveSystem.LoadPlayerData();
        if(data != null)
        {
            FOV = data.FOV;
            sensitivity = data.sensitivity;
            musicVolume = data.musicVolume;
            soundEffectVolume = data.soundEffectVolume;
            hasDoneTutorial = data.hasDoneTutorial;
        }
        else
        {
            FOV = 80f;
            sensitivity = 150f;
            musicVolume = 0.5f;
            soundEffectVolume = 0.5f;
            hasDoneTutorial = false;
        }
        return data;
    }
}
