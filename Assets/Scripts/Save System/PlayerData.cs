using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public float FOV;
    public float sensitivity;
    public float musicVolume;
    public float soundEffectVolume;
    public bool hasDoneTutorial;

    public PlayerData(Player player)
    {
        FOV = player.FOV;
        sensitivity = player.sensitivity;
        musicVolume = player.musicVolume;
        soundEffectVolume = player.soundEffectVolume;
        hasDoneTutorial = player.hasDoneTutorial;
    }
}
