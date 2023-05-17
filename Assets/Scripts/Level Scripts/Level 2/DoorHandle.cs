using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandle : MonoBehaviour
{
    [SerializeField] private PlayerMovement stomp;
    [SerializeField] private Door door;

    // Update is called once per frame
    void Update()
    {
        if (stomp.objectCollided == "Knob")
        {
            door.isDoorOpen = true;
            stomp.objectCollided = null;
        }
    }
}
