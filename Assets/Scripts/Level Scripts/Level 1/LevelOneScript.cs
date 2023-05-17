using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneScript : MonoBehaviour
{
    [Header("Wind Variables")]
    [SerializeField] private GameObject windZone;

    [Header("Fan")]
    [SerializeField] private Animator fanAnimator;
    [SerializeField] private Material switchLight;
    [SerializeField] private GameObject fan;
    [SerializeField] private GameObject windParticles;
    private AudioSource fanSound;

    [Header("Player")]
    private GameObject player;
    private TailSwitchOrder tailOrder;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerCat").gameObject;
        tailOrder = player.GetComponent<TailSwitchOrder>();
        playerMovement = player.GetComponent<PlayerMovement>();

        ResetSwitchColor();
        fanSound = fan.GetComponent<AudioSource>();

        windParticles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.objectCollided == "Plug")
        {
            fanAnimator.SetBool("Socket", true);
            tailOrder.levelOrder++;
            AnimateWholeFan();
            playerMovement.hasStomped = false;
            playerMovement.objectCollided = null;
            windParticles.SetActive(true);
        }
    }

    private void AnimateWholeFan()
    {
        fanSound.Play();
        fanAnimator.SetBool("PlugIn", true);
        windZone.SetActive(true);
        switchLight.SetColor("_Top_Color", new Color(0f, 6f, 0f));
    }

    private void ResetSwitchColor()
    {
        switchLight.SetColor("_Top_Color", new Color(6f, 0f, 0f));
    }
}
