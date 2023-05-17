using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerPosition : MonoBehaviour
{
    private LevelSound audioManager;
    private GameObject player;

    [SerializeField] private GameObject level2Spawn;
    [SerializeField] private GameObject robotAI;
    [SerializeField] private GameObject human;

    [SerializeField] private GameObject turbine;
    [SerializeField] private GameObject water;
    private void Start()
    {
        player = GameObject.Find("PlayerCat");
        audioManager = GameObject.Find("AudioManager").GetComponent<LevelSound>();
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if(gameObject.name == "NewLevel2")
        {
            player.transform.position = level2Spawn.transform.position;
            audioManager.SecondLevel(robotAI, human);
            robotAI.GetComponent<RobotAi>().canFindPlayer = false;
            robotAI.GetComponent<RobotAi>().enabled = true;
        }

        if(gameObject.name == "NewLevel3")
        {
            audioManager.ThirdLevel(turbine, water);
        }
    }
}
