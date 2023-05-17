using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    [SerializeField] private WaterRising waterRising;
    [SerializeField] private WaterPipeBath bathPipes;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            waterRising.enabled = true;
            bathPipes.enabled = true;
        }
    }
}
