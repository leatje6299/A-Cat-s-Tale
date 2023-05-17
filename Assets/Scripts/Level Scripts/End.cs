using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    [SerializeField] private GameObject blurEffect;
    [SerializeField] private GameObject endScreen;

    private void OnTriggerEnter(Collider other)
    {
        blurEffect.SetActive(true);
        endScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }
}
