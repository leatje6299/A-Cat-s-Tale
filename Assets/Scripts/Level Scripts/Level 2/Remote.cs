using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Remote : MonoBehaviour
{
    [SerializeField] private Material staticEffectMaterial;
    [SerializeField] private Material remoteEffectMaterial;
    [SerializeField] private AudioSource tvSound;

    [SerializeField] private PlayableDirector director;
    [SerializeField] private RobotAi robot;

    private Human human;

    private bool tvON;

    private void Start()
    {
        staticEffectMaterial.SetFloat("_TVOn", 0f);
        remoteEffectMaterial.SetColor("_Top_Color", new Color(6f,0f,0f));
        human = GameObject.Find("Human").GetComponent<Human>();
        tvON = false;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "pushSpot")
        {
            TurnTVON();
        }
    }

    private void TurnTVON()
    {
        staticEffectMaterial.SetFloat("_TVOn", 1f);
        remoteEffectMaterial.SetColor("_Top_Color", new Color(0f, 6f, 0f));
        tvON = true;
        human.isHumanDistracted = true;
        human.multiAimConstraint.weight = 0.05f;
        human.animatorHuman.SetBool("isDistracted", human.isHumanDistracted);

        tvSound.enabled = true;

        director.Play();
        robot.canFindPlayer = false;
    }

    public void CheckTVON()
    {
        if(tvON)
        {
            human.isHumanDistracted = true;
            human.multiAimConstraint.weight = 0.05f;
        }
    }
}
