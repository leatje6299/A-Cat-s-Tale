using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChristmasLight : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement playerMovement;
    [SerializeField] private Material material;
    [SerializeField] private Animator fairyLightAnimator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerCat").gameObject;
        playerMovement = player.GetComponent<PlayerMovement>();
        material.SetColor("_Color", new Color(0.1f, 0.1f, 0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.objectCollided == "LightPlug")
        {
            print("hello");
            material.SetColor("_Color", new Color(12f,12f,0f));
            playerMovement.hasStomped = false;
            playerMovement.objectCollided = null;
            fairyLightAnimator.SetTrigger("pluggedIn");
        }
    }
}
