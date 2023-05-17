using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations.Rigging;

public class Human : MonoBehaviour
{
    private Game game;

    [Header("Death Cut Scene")]
    [SerializeField] private PlayableDirector deathCutScene;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject blurEffect;

    [Header("Player")]
    private Transform player;
    private PlayerMovement playerMovement;
    [SerializeField] private LayerMask playerMask;
    private VFX playerVFX;

    [Header("Human")]
    public bool isHumanDistracted;
    public Animator animatorHuman;
    private Vector3 sightPosition;
    public MultiAimConstraint multiAimConstraint;

    void Start()
    {
        player = GameObject.Find("PlayerCat").transform;
        playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        playerVFX = player.Find("CatToon").GetComponent<VFX>();

        game = GameObject.FindGameObjectWithTag("Manager").GetComponent<Game>();

        isHumanDistracted = false;
        sightPosition = transform.GetChild(0).position;

        animatorHuman = gameObject.GetComponent<Animator>();

        multiAimConstraint.weight =0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        if(!playerMovement.isScared && !isHumanDistracted)
        {
            Vector3 dirToPlayer = (player.position - sightPosition).normalized;
            if (Vector3.Angle(transform.GetChild(0).forward, dirToPlayer) < 50)
            {
                gameObject.GetComponent<AudioSource>().Play();
                multiAimConstraint.weight = 1f;
                playerMovement.isScared = true;
                playerVFX.PlayPlayerSoundEffect("Hissing");
                Invoke("PlayerIsScared", 2f);
            }
            else
            {
                playerMovement.isScared = false;
            }
        }
    }

    public void AnimationOnPlayerDetect()
    {
        animatorHuman.SetBool("playerIsDetected", true);
    }
    public void AnimationOnPlayerEnd()
    {
        deathUI.SetActive(true);
        blurEffect.SetActive(true);

        animatorHuman.SetBool("playerIsDetected", false);

        playerMovement.isScared = false;
        playerMovement.animator.SetBool("isScared", false);
        player.transform.position = game.lastCheckPoint;

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayerIsScared()
    {
        deathCutScene.initialTime = 14f;
        deathCutScene.Play();
        transform.position = new Vector3(119f, 52f, -55f);
        isHumanDistracted = true;
    }

    public void CheckIfPlayerDead()
    {
        if(playerMovement.isScared == true)
        {
            PlayerIsScared();
        }
    }
}
