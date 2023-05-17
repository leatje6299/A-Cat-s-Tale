using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    public bool tutorialDone = false; 

    [Header("Inputs")]
    [SerializeField] private InputActionReference interactControl;

    [Header("Text")]
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private Animator textAnimator;
    private string[] tutorialTexts;
    public int tutorialTextIndex = -1;
    public int previousTutorialTextIndex = -1;

    [Header("Tail")]
    private TailStateManager stateManager;
    [SerializeField] private GameObject currentTail;
    private Player player;

    [Header("Pulse Effect")]
    private float timer = 0f;
    private float lerpValue;
    private float pulseDuration = 1.5f;
    private float pulseMagnitude = 1.15f;
    private Vector3 defaultScale;
    public bool isPulsating = false;

    private GameObject previousState;

    [Header("Tutorial Materials")]
    [SerializeField] private List<Material> treeMaterials;
    [SerializeField] private GameObject tree;
    [SerializeField] private List<Material> plankMaterials;
    [SerializeField] private GameObject plank;
    [SerializeField] private List<Material> plugMaterials;
    [SerializeField] private GameObject plug;
    private int materialIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerCat").GetComponent<Player>();
        stateManager = player.gameObject.GetComponent<TailStateManager>();
        tutorialTexts = new string[] { "Current Tail : Strong Tail \n \n You can pick up objects \n Rotate Them and drop them!",
                                        "Next Tail : Long Tail \n \n Use it on this tree to swing",
                                        "Next Tail : Puffy Tail \n \n Use it to jump higher and become undectectable in certain situations", 
                                        "The strong tail also allows you to stomp on objects \n with enough height", "",
                                        "The top right checkpoint icon means you've reached a checkpoint \n You can return to it later via the option menu.", ""};
        previousState = currentTail.transform.Find("Strong").gameObject;
    }

    private void Update()
    {
        if (tutorialTextIndex != previousTutorialTextIndex)
        {
            timer = 0f;
            previousTutorialTextIndex = tutorialTextIndex;
            isPulsating = true;
        }

        if (isPulsating && !tutorialDone)
        {
            timer += Time.deltaTime * 2f;
            if (timer >= 40f)
            {
                timer = 0f;
                lerpValue = 1f;
                isPulsating = false;
            }
            else
            {
                lerpValue = Mathf.PingPong(timer, pulseDuration) / pulseDuration;
            }
            GetActiveState().transform.localScale = Vector3.Lerp(defaultScale, defaultScale * pulseMagnitude, lerpValue);
        }

        if (previousState != GetActiveState())
        {
            SetTutorialText();
            previousState = GetActiveState();
            setMaterials();
        }
    }

    public void SetTutorialText()
    {
        PlayerData data = player.LoadData();
        defaultScale = GetActiveState().transform.localScale;
        tutorialTextIndex++;
        if (!data.hasDoneTutorial)
        {
            for(int i = 0; i < tutorialTexts.Length; i++)
            {
                if(i == tutorialTextIndex)
                {
                    tutorialText.text = tutorialTexts[i];
                    textAnimator.SetTrigger("FadeIn");
                }
            }

            if (tutorialTextIndex == tutorialTexts.Length - 1)
            {
                tutorialDone = true;
            }
        }
    }

    private GameObject GetActiveState()
    {
        GameObject firstActiveGameObject;

        for (int i = 0; i < currentTail.transform.childCount; i++)
        {
            if (currentTail.transform.GetChild(i).gameObject.activeSelf == true)
            {
                firstActiveGameObject = currentTail.transform.GetChild(i).gameObject;
                return firstActiveGameObject;
            }
        }

        return null;
    }

    public void setMaterials()
    {
        materialIndex++;
        if(materialIndex == 0)
        {
            plank.GetComponent<Renderer>().material = plankMaterials[0];
            tree.GetComponent<Renderer>().material = treeMaterials[1];
            plug.GetComponent<Renderer>().material = plugMaterials[1];
        }
        else if(materialIndex == 1)
        {
            tree.GetComponent<Renderer>().material = treeMaterials[0];
            plank.GetComponent<Renderer>().material = plankMaterials[1];
        }
        else if(materialIndex == 2)
        {
            tree.GetComponent<Renderer>().material = treeMaterials[1];
        }
        else if (materialIndex == 3)
        {
            plug.GetComponent<Renderer>().material = plugMaterials[0];
        }
        else if (materialIndex == 4)
        {
            plug.GetComponent<Renderer>().material = plugMaterials[1];
        }
    }

    private void OnEnable()
    {
        interactControl.action.Enable();
    }

    private void OnDisable()
    {
        interactControl.action.Disable();
    }
}
