using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InputActionReference escapeControl;

    [Header("States")]
    [SerializeField] private GameObject tailStateImg;
    private Transform states;
    [SerializeField] private List<Image> subStates;

    [SerializeField] private GameObject currentAction;

    [Header("Scripts")]
    [SerializeField] private ActionDetection actionDet;
    [SerializeField] private TailStateManager state;

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject blurEffect;
    [SerializeField] private GameObject tutorialText;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        states = tailStateImg.transform.Find("States").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(escapeControl.action.triggered)
        {
            tailStateImg.SetActive(false);
            pauseScreen.SetActive(true);
            tutorialText.SetActive(false);
            blurEffect.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            currentAction.SetActive(false);
            
        }
    }

    public void UpdateUI(TailBaseState UpdatedState)
    {
        //main state
        foreach(Transform child in states)
        {
            child.gameObject.SetActive(false);
        }
        Transform childToEnable = states.Find(UpdatedState.name);

        childToEnable.gameObject.SetActive(true);

        //substate
        for(int i = 1; i < 4; i++)
        {
            Transform sub = null;
            var nextTailId = state.GetNextTailIDs(i);

            if(nextTailId != null && nextTailId.name != null)
            {
                sub = states.Find(state.GetNextTailIDs(i).name);
            }
            if (sub != null)
            {
                subStates[i - 1].sprite = sub.gameObject.GetComponent<Image>().sprite;
            }
        }

    }

    public void Unpause()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        tailStateImg.SetActive(true);
        currentAction.SetActive(true);
        tutorialText.SetActive(true);
    }

    private void OnEnable()
    {
        escapeControl.action.Enable();
    }

    private void OnDisable()
    {
        escapeControl.action.Disable();
    }
}
