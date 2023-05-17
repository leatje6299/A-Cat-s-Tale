using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isDoorOpen;

    void Update()
    {
        if (isDoorOpen && transform.rotation.eulerAngles.y <=180f)
        {
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        transform.Rotate(new Vector3(0f, 0f, 1f));
        yield return new WaitForSeconds(5f);
    }
}
