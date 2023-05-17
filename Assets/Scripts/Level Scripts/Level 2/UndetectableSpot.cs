using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndetectableSpot : MonoBehaviour
{
    [SerializeField] private RobotAi robot;

    private void OnTriggerExit(Collider other)
    {
        gameObject.SetActive(false);
        robot.canFindPlayer = true;
    }
}
