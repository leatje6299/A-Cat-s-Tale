using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgress : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    [SerializeField] private TailStateManager tailState;
    float timeRemaining;
    float maxTime;
    private void OnEnable()
    {
        maxTime = tailState.TailPuffy.GetPuffyDuration();
        timeRemaining = 0f;
    }
    private void Update()
    {
        progressImage.fillAmount = 0f;
        if(tailState.isPuffyActivated)
        {
            if(timeRemaining < maxTime)
            {
                timeRemaining += Time.deltaTime;
                progressImage.fillAmount = timeRemaining / tailState.TailPuffy.GetPuffyDuration();
            }
        }
    }
}
