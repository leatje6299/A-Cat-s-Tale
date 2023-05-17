using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSound : MonoBehaviour
{
    private AudioManager audioManager;
    public void Awake()
    {
        // Check if an existing AudioManager object exists
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlayerSound(GameObject player, GameObject playerCat)
    {
        audioManager.PlayOnObject("Hissing", player, false);
        audioManager.PlayOnObject("Swing", player, false);
        audioManager.PlayOnObject("PickUp", player, false);
        audioManager.PlayOnObject("Drop", player, false);
        audioManager.PlayOnObject("Step", playerCat, false);
        audioManager.PlayOnObject("Transition", player, false);
        audioManager.PlayOnObject("TransitionReverse", player, false);
    }

    public void FirstLevel(GameObject water, GameObject fan, GameObject pipe)
    {
        audioManager.PlayOnObject("Wind", water, true, 90f);
        audioManager.PlayOnObject("Water", water, true, 90f);
        audioManager.PlayOnObject("Fan", fan, false);
        audioManager.PlayOnObject("Fan", pipe, true, 20f);
    }

    public void SecondLevel(GameObject robot, GameObject human)
    {
        audioManager.PlayOnObject("RobotMoving", robot, false);
        audioManager.PlayOnObject("RobotDetection", robot, false);
        audioManager.PlayOnObject("Hmm", human, false);
    }

    public void ThirdLevel(GameObject turbine, GameObject water)
    {
        audioManager.PlayOnObject("Fan", turbine, true);
        audioManager.PlayOnObject("Water", water, true, 30f);
        audioManager.PlayOnObject("Water Running", water, true, 30f);
    }
}
 