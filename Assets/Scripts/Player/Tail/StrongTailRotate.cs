using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class StrongTailRotate : MonoBehaviour
{
    [SerializeField] private InputActionReference pointer;
    [SerializeField] private InputActionReference rotate;

    [SerializeField] private CinemachineFreeLook cam;

    [SerializeField] private TailStateManager state;

    private Player playerSettings;

    private float rotationSpeed = 1f;
    private Quaternion startRotation;
    bool canRotate;

    private Transform player;

    private void Start()
    {
        player = GameObject.Find("PlayerCat").transform;
        playerSettings = player.GetComponent<Player>();

        startRotation = transform.rotation;
        SetCameraSettings(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (state.heldObj != null)
        {
            RotateObject(state.heldObj);
        }
        if (state.heldObj == null && canRotate)
        {
            SetCameraSettings(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            cam.LookAt = player;
        }
    }

    private void SetCameraSettings(bool rotating)
    {
        PlayerData data = playerSettings.LoadData();
        if (!rotating)
        {
            if (data != null)
            {
                cam.m_XAxis.m_MaxSpeed = data.sensitivity;
            }
            else
            {
                cam.m_XAxis.m_MaxSpeed = 150f;
            }
            canRotate = false;
            cam.m_YAxis.m_MaxSpeed = 0.5f;
        }
        else
        {
            canRotate = true;
            cam.m_YAxis.m_MaxSpeed = 0;
            cam.m_XAxis.m_MaxSpeed = 0f;
        }
    }

    private void RotateObject(GameObject obj)
    {
        if (state.objectPickedUp)
        {
            if (canRotate && rotate.action.triggered)
            {
                SetCameraSettings(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                cam.LookAt = player;
                return;
            }
            if (!canRotate && rotate.action.triggered)
            {
                SetCameraSettings(true);
                cam.LookAt = obj.transform;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            if(canRotate)
            {
                var delta = pointer.action.ReadValue<Vector2>();
                obj.transform.rotation = startRotation * Quaternion.Euler(delta.y * rotationSpeed, -delta.x * rotationSpeed, 0f);
            }
        }
    }
    private void OnEnable()
    {
        pointer.action.Enable();
        rotate.action.Enable();
    }

    private void OnDisable()
    {
        pointer.action.Disable();
        rotate.action.Disable();
    }
}
