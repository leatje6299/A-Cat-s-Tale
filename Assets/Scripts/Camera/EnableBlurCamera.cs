using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBlurCamera : MonoBehaviour
{
    [SerializeField] private GameObject blurCamera;
    private void OnEnable()
    {
        blurCamera.SetActive(true);
    }
    private void OnDisable()
    {
        if(blurCamera != null)
        {
            blurCamera.SetActive(false);
        }
    }
}
