using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRising : MonoBehaviour
{
    public Vector3 waterPosition;
    public float waterRiseSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        waterPosition = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        waterPosition.y += waterRiseSpeed * Time.deltaTime;
        transform.localPosition = waterPosition;
    }
}
