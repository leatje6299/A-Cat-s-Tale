using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementWind : MonoBehaviour
{
    private TailStateManager state;
    private Rigidbody rb;
    private GameObject windZone;
    private bool inWindZone = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        state = GetComponent<TailStateManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "windArea")
        {
            windZone = other.gameObject;
            inWindZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "windArea")
        {
            inWindZone = false;
        }
    }

    private void FixedUpdate()
    {
        if(inWindZone && state.currentState == state.TailPuffy && state.isPuffyActivated)
        {
            rb.AddForce(windZone.GetComponent<Wind>().direction * windZone.GetComponent<Wind>().strength);
        }
    }
}
