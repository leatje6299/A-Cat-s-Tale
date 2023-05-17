using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CheckpointManager : MonoBehaviour
{
    private TailSwitchOrder order;
    private TailStateManager tailState;

    [Header("Cut Scenes")]
    [SerializeField] private PlayableDirector director;
    public int cutSceneOrder = -1;

    [Header("Checkpoints")]
    [SerializeField] private GameObject levelLoad;
    private Game game;
    [SerializeField] private GameObject loadingObject;
    [SerializeField] private GameObject blurEffect;

    [Header("Robot")]
    [SerializeField] private GameObject robotAI;

    void Start()
    {
        order = GetComponent<TailSwitchOrder>();
        game = GameObject.FindGameObjectWithTag("Manager").GetComponent<Game>();
        game.lastCheckPoint = transform.position;
        tailState = GameObject.Find("PlayerCat").GetComponent<TailStateManager>();

        loadingObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "tailLevel")
        {
            order.levelOrder++;
            other.gameObject.SetActive(false);
            game.lastCheckPoint = transform.position;

            if(other.name == "NewLevel2")
            {
                levelLoad.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                game.lastCheckPoint = transform.position;
                Time.timeScale = 0f;
            }
            loadingObject.SetActive(true);
            loadingObject.GetComponent<Animator>().SetTrigger("isLoading");
        }

        if(other.tag == "cutSceneLevel")
        {
            cutSceneOrder++;
            other.gameObject.SetActive(false);
            if (cutSceneOrder == 0)
            {
                director.initialTime = 3f;
                director.Play();
            }
        }
    }

    public void LoadNewLevel(bool cutScene)
    {
        levelLoad.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        if (cutScene)
        {
            director.initialTime = 12f;
            director.Play();
            Invoke("EnableRobot", 30f);
        }
    }
    public void LastCheckPoint()
    {
        transform.position = game.lastCheckPoint;
        blurEffect.SetActive(false);
        tailState.ResetState();
        game.ResetCheckPointObjects();
    }

    private void EnableRobot()
    {
        robotAI.GetComponent<RobotAi>().enabled = true;
    }
}
