using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraControl : MonoBehaviour
{
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 max;
    [SerializeField] private Vector2 yRotationRange;
    [SerializeField] [Range(0.01f, 0.1f)] private float lerpSpeed = 0.0f;

    private Vector3 newPosition;
    private Quaternion newRotation;


    private void Awake()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
    }

    private void Start()
    {
        GetNewPos();
        Time.timeScale = 1f;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * lerpSpeed);
        if(Vector3.Distance(transform.position, newPosition) < 1f)
        {
            GetNewPos();
        }
    }

    private void GetNewPos()
    {
        var xPos = Random.Range(min.x, max.x);
        var zPos = Random.Range(min.y, max.y);

        newRotation = Quaternion.Euler(0, Random.Range(yRotationRange.x, yRotationRange.y), 0);
        newPosition = new Vector3(xPos, 0, zPos);
    }
}
