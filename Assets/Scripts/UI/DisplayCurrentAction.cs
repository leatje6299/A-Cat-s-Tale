using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCurrentAction : MonoBehaviour
{
    [Header("Keybinds and actions")]
    [SerializeField] private Image displayActionIcon;
    [SerializeField] private Image displayFurtherActionIcon;
    [SerializeField] private TMP_Text keybindsText;
    [SerializeField] private TMP_Text keybindsIcon;
    [SerializeField] private TMP_Text secondKeybindsText;
    [SerializeField] private TMP_Text secondKeybindsIcon;

    private Animator actionAnimator;
    private bool hasPlayedAnimation;

    private Transform player;
    private TailStateManager tailState;
    private PlayerMovement playerMovement;
    private ActionDetection actions;

    Transform tMin = null;

    private void Start()
    {
        player = GameObject.Find("PlayerCat").transform;
        tailState = player.GetComponent<TailStateManager>();
        playerMovement = player.GetComponent<PlayerMovement>();
        actions = player.GetComponent<ActionDetection>();
        actionAnimator = displayActionIcon.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        DisplayAction();
        DisplayKeybinds();
    }

    public void DisplayAction()
    {
        //Transform tMin = null;
        if (tailState.TailLong == tailState.currentState)
        {
            tMin = actions.getClosestAction(true);
        }
        if(tailState.TailStrong == tailState.currentState && !tailState.objectPickedUp)
        {
            tMin = actions.getClosestAction(false);
        }
        
        if (tMin == null || actions.visibleActions == null)
        {
            displayActionIcon.enabled = false;
            displayActionIcon.transform.GetChild(0).gameObject.SetActive(false);
            hasPlayedAnimation = false;
            return;
        }
        if(!hasPlayedAnimation)
        {
            actionAnimator.SetTrigger("popUp");
            hasPlayedAnimation = true;
        }

        Vector2 screenPoint = Camera.main.WorldToScreenPoint(tMin.position);
        displayActionIcon.enabled = true;
        displayActionIcon.transform.GetChild(0).gameObject.SetActive(true);
        Vector2 localPoint;
        RectTransform canvasRectTransform = GameObject.Find("UI").GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPoint, null, out localPoint);
        displayActionIcon.transform.localPosition = localPoint;
    }

    private void DisplayKeybinds()
    {
        if(keybindsText.text == "")
        {
            keybindsIcon.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            keybindsIcon.transform.parent.gameObject.SetActive(true);
        }
        if(!tailState.objectPickedUp)
        {
            secondKeybindsIcon.transform.parent.gameObject.SetActive(false);
            secondKeybindsIcon.text = "";
            secondKeybindsText.text = "";
        }

        if(tailState.currentState == tailState.TailPuffy && !tailState.isPuffyActivated)
        {
            keybindsIcon.text = "F";
            keybindsText.text = "activate";
        }
        else if(tailState.currentState == tailState.TailStrong && playerMovement.canStomp)
        {
            keybindsIcon.text = "_";
            keybindsText.text = "stomp";
        }
        else if(tMin == null)
        {
            keybindsIcon.text = "";
            keybindsText.text = "";
        }
        else 
        {
            if (tailState.currentState == tailState.TailStrong && !tailState.objectPickedUp)
            {
                keybindsIcon.text = "F";
                keybindsText.text = "pick up";
            }
            else if (tailState.currentState == tailState.TailStrong && tailState.objectPickedUp)
            {
                keybindsIcon.text = "F";
                keybindsText.text = "drop";
                secondKeybindsIcon.transform.parent.gameObject.SetActive(true);
                secondKeybindsIcon.text = "R";
                secondKeybindsText.text = "rotate";
            }
            else if (tailState.currentState == tailState.TailLong)
            {
                keybindsIcon.text = "F";
                keybindsText.text = "swing";
            }
        }
    }
}
