using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class Swinging : MonoBehaviour
{
    /*
     GameDevelopment, D. (2022). ADVANCED SWINGING in 9 MINUTES - Unity Tutorial. [online] www.youtube.com. Available at: https://youtu.be/HPjuTK91MA8 
    */

    [SerializeField] private InputActionReference interactControl;

    private VFX playerVFX;

    [Header("Scripts")]
    private ActionDetection action;
    private TailStateManager tailState;
    private PlayerMovement playerMovement;

    [Header("Swing")]
    public Transform gunTip;
    private Vector3 swingPoint;
    private SpringJoint joint;
    private float airForce = 9f;

    [Header ("Animations")]
    [SerializeField] private ChainIKConstraint constraint;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject speedLines;
    [SerializeField] private Animator displayActionAnimator;

    private float fallingTime = 1f;

    private void Start()
    {
        action = GetComponent<ActionDetection>();
        tailState = GetComponent<TailStateManager>();
        playerMovement = GetComponent<PlayerMovement>();
        playerVFX = transform.Find("CatToon").gameObject.GetComponent<VFX>();

        constraint.data.chainRotationWeight = 0f;
        speedLines.SetActive(false);
    }

    void Update()
    {
        Inputs();

        if(joint !=null)
        {
            DampWeight(0.8f);
            AirControl();
        }
        else
        {
            DampWeight(0f);
        }
    }
    private void DampWeight(float target)
    {
        float delta = Mathf.Abs(target - constraint.data.chainRotationWeight);
        delta *= Time.deltaTime * 2;
        if(target == 0.8f)
        {
            constraint.data.chainRotationWeight += delta;
        }
        else if(target == 0f)
        {
            constraint.data.chainRotationWeight -= delta;
        }
    }

    private void Inputs()
    {
        if (interactControl.action.triggered
            && tailState.currentState == tailState.TailLong
            && !tailState.switching)
        {
            if (playerMovement.isSwinging)
            {
                if (action.getClosestAction(true) != null)
                {
                    SwitchSwing();
                    return;
                }
                StopSwing();
                tailState.IncreaseIDOrder();
                tailState.switching = false;
                return;
            }
            if (!playerMovement.isSwinging)
            {
                if (action.getClosestAction(true) != null)
                {
                    StartSwing();
                }
            }
        }
    }

    //SWING
    private void StartSwing()
    {
        playerMovement.isSwinging = true;
        speedLines.SetActive(true);
        Swing();
    }

    private void SwitchSwing()
    {
        if (joint != null) Destroy(joint);
        displayActionAnimator.SetTrigger("popUp");
        Swing();

    }

    private void Swing()
    {
        playerVFX.PlayPlayerSoundEffect("Swing");
        swingPoint = action.getClosestAction(true).position;
        target.position = swingPoint;

        joint = gameObject.AddComponent<SpringJoint>();
        joint.anchor = new Vector3(0, 0, 0);
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

        joint.maxDistance = 1f;
        joint.minDistance = 3f;

        joint.spring = 4.5f;
        joint.damper = 5f;
        joint.massScale = 4f;
    }

    private void AirControl()
    {
        //print(playerMovement.movement);
        if(playerMovement.movement.y > 0f)
        {
            playerMovement.rb.AddForce(playerMovement.orientation.forward * airForce);
        }
        if(playerMovement.movement.y < 0f)
        {
            playerMovement.rb.AddForce(- playerMovement.orientation.forward * airForce);
        }
    }
    private void StopSwing()
    {
        playerMovement.isSwinging = false;
        speedLines.SetActive(false);
        Destroy(joint);
        fallingTime = 1f;
        float force = Mathf.Clamp(playerMovement.rb.velocity.magnitude * 6f, 1f, 18f);
        playerMovement.rb.AddRelativeForce(0, force, 0, ForceMode.Impulse);
        playerMovement.rb.AddForce(new Vector3(0, GetFallingGravity(), 0));
    }

    private float GetFallingGravity()
    {
        fallingTime += Time.deltaTime;
        fallingTime *= 2;
        return playerMovement.gravityAndSpeed[0] * fallingTime;
    }

    private void OnEnable()
    {
        interactControl.action.Enable();
    }

    private void OnDisable()
    {
        interactControl.action.Disable();
    }
}
