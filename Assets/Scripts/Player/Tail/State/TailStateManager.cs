using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TailStateManager : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private InputActionReference interactControl;

    [Header("Tails")]
    public TailBaseState currentState;
    public TailLongState TailLong = new TailLongState();
    public TailPuffyState TailPuffy = new TailPuffyState();
    public TailStrongState TailStrong = new TailStrongState();
    [SerializeField] private Transform tails;

    private int idOrder;
    private bool canSwitchState = false;
    public bool switching = false;

    [Header("Scripts")]
    [SerializeField] private UIManager UI;
    [SerializeField] private TailSwitchOrder order;

    [Header("Actions")]
    public ActionDetection currentActions;

    [Header("Strong tail variables")]
    [SerializeField] Transform holdingArea;
    public GameObject heldObj;
    public bool objectPickedUp = false;
    private Transform closestAction;

    [Header("Puffy tail variables")]
    [SerializeField] private Material puffyTailMaterial;
    [SerializeField] private Material catMaterial;
    [SerializeField] private TMP_Text textTimer;
    public bool isPuffyActivated = false;

    private VFX playerVFX;
    [SerializeField] private ParticleSystem particleEffect;

    void Start()
    {
        currentState = TailStrong;
        currentState.EnterState(this);
        UI.UpdateUI(currentState);
        DisplayTail(currentState.name);

        playerVFX = transform.Find("CatToon").gameObject.GetComponent<VFX>();

        puffyTailMaterial.SetVector("_Base_Color", new Vector4(0.5226287f, 0.9728137f, 0.812403f, 1f) * 1f);
        puffyTailMaterial.SetColor("_Top_Color", new Vector4(0f, 0f, 0f, 1f)); ;
        catMaterial.SetFloat("_TimeOvertime", -1f);

        if (particleEffect.isPlaying) particleEffect.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
        closestAction = currentActions.getClosestAction(false);
        SpecialAbility();
    }

    public void SwitchState(TailBaseState state)
    {
        if(state != currentState)
        {
            print("is this called");
            currentState = state;
            state.EnterState(this);
            DisplayTail(state.name);
            objectPickedUp = false;
        }
        UI.UpdateUI(state);
    }

    public void SpecialAbility()
    {
        if (interactControl.action.triggered)
        {
            StartCoroutine(PlayParticleEffect(3f));
            if(currentState != TailLong)
            {
                Debug.Log("Tail State Manager: I pressed F!");

                if (objectPickedUp && currentState == TailStrong && switching)
                {
                    Debug.Log("Tail State Manager: DROPPING");
                    DropObject();
                    return;
                }

                if (!switching)
                {
                    if (currentState == TailPuffy)
                    {
                        //change this with timing of puffy state
                        PuffyAbility();
                    }
                    if (currentState == TailStrong)
                    {
                        StrongAbility();
                        if (!objectPickedUp)
                        {
                            Debug.Log("Tail State Manager: RETURN");
                            return;
                        }
                    }

                    IncreaseIDOrder();

                }
            }
        }

        canSwitchState = (switching == true) ? false : true;

        if (canSwitchState)
        {
            if(order.previousLevelOrder != order.levelOrder)
            {
                order.previousLevelOrder = order.levelOrder;
                UI.UpdateUI(GetTailByID(order.GetTailOrder()[idOrder]));
                idOrder = 0;

            }
            SwitchState(GetTailByID(order.GetTailOrder()[idOrder]));
        }
    }

    //PUFFY TAIL FUNCTIONS
    private void PuffyAbility()
    {
        switching = true;
        StartCoroutine(UndetectableTime());
    }
    private IEnumerator UndetectableTime()
    {
        puffyTailMaterial.SetVector("_Base_Color", new Vector4(0.3967553f, 0.7379107f, 0.6172068f, 1f) * 1.4f); ;
        puffyTailMaterial.SetColor("_Top_Color", new Vector4(1f, 1f, 1f, 1f));
        playerVFX.PlayPlayerSoundEffect("Transition");

        float timeOverTime = -1f;
        while (timeOverTime < 1f)
        {
            timeOverTime += Time.deltaTime;
            catMaterial.SetFloat("_TimeOvertime", timeOverTime);
            puffyTailMaterial.SetFloat("_TimeOvertime", timeOverTime);
            yield return null;
        }

        isPuffyActivated = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.mass = 0.8f;

        //Puffy time
        float counter = TailPuffy.GetPuffyDuration();
        while (counter > 0)
        {
            var integer = (int)counter;
            textTimer.text = integer.ToString();
            counter -= Time.deltaTime;
            yield return null;
        }

        textTimer.text = "";

        //Puffy time done
        playerVFX.PlayPlayerSoundEffect("TransitionReverse");

        while (timeOverTime > -1f)
        {
            timeOverTime -= Time.deltaTime;
            catMaterial.SetFloat("_TimeOvertime", timeOverTime);
            puffyTailMaterial.SetFloat("_TimeOvertime", timeOverTime);
            yield return null;
        }


        switching = false;
        isPuffyActivated = false;
        rb.mass = 1.2f;
        puffyTailMaterial.SetVector("_Base_Color", new Vector4(0.5226287f, 0.9728137f, 0.812403f, 1f) * 1f);
        puffyTailMaterial.SetColor("_Top_Color", new Vector4(0f, 0f, 0f, 1f)); ;
    }

    //STRONG TAIL FUNCTIONS
    private void StrongAbility()
    {
        if(heldObj == null)
        {
            if(closestAction != null)
            {
                PickUpObject(closestAction.gameObject);
            }
        }
    }

    private void PickUpObject(GameObject obj)
    {
        Debug.Log("Tail State Manager: PICK UP!");
        if (obj.GetComponent<Rigidbody>())
        {
            playerVFX.PlayPlayerSoundEffect("Pick Up");
            switching = true;
            heldObj = obj;
            Rigidbody rb = heldObj.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.drag = 10;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.constraints = RigidbodyConstraints.FreezePosition;

            // Calculate the offset to align the bottom of the object with the holding area
            Vector3 objSize = obj.GetComponent<Collider>().bounds.size;
            Vector3 objScale = obj.transform.localScale;
            float objHeight = objSize.y / objScale.y;
            Vector3 objOffset = new Vector3(0, -objHeight / 2, 0);

            rb.transform.position = holdingArea.position - objOffset;
            rb.transform.SetParent(holdingArea, true);

            heldObj.GetComponent<Collider>().enabled = false;
            objectPickedUp = true;
        }
    }

    private void DropObject()
    {
        playerVFX.PlayPlayerSoundEffect("Drop");
        Rigidbody rb = heldObj.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.drag = 1;
        rb.constraints = RigidbodyConstraints.None;
        heldObj.GetComponent<Collider>().enabled = true;
        heldObj.transform.parent = null;
        heldObj = null;
        objectPickedUp = false;
        switching = false;
    }

    //INCREASE ID ORDER
    public void IncreaseIDOrder()
    {
        if (idOrder == (order.GetTailOrder().Count - 1))
        {
            idOrder = 0;
        }
        else
        {
            idOrder += 1;
        }
    }

    /*GETTERS*/
    public List<float> GetPlayerSpeedAndGravity()
    {
        List<float> result = new List<float>(2);
        result = currentState.GetPlayerSpeedAndGravity();
        return result;
    }

    private TailBaseState GetTailByID(int id)
    {
        if(id == 1)
        {
            return TailLong;
        }
        if(id == 2)
        {
            return TailPuffy;
        }
        if(id == 3)
        {
            return TailStrong;
        }

        return null;
    }

    public TailBaseState GetNextTailIDs(int howManyTailsLater)
    {
        int tailOrderLength = order.GetTailOrder().Count;
        int index = (idOrder + howManyTailsLater) % tailOrderLength;
        int tailID = order.GetTailOrder()[index];
        TailBaseState nextState = GetTailByID(tailID);
        return nextState;
    }

    private void DisplayTail(string name)
    {
        for(int i = 0; i < tails.childCount; i++)
        {
            Transform child = tails.GetChild(i);
            if(child.name == name)
            {
                child.gameObject.SetActive(true);
            }
            else
            { 
                child.gameObject.SetActive(false);
            }
        }        
    }

    IEnumerator PlayParticleEffect(float duration)
    {
        if(!particleEffect.isPlaying) particleEffect.Play();
        yield return new WaitForSeconds(duration);
        if(particleEffect.isPlaying) particleEffect.Stop();
    }

    public void ResetState()
    {
        idOrder = 0;
        SwitchState(GetTailByID(order.GetTailOrder()[idOrder]));
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
