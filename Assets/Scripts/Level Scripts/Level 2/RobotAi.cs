using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class RobotAi : MonoBehaviour
{
    private NavMeshAgent agent;
    
    private Transform player;
    private PlayerMovement playerMovement;
    private VFX playerVFX;
    public bool canFindPlayer = true;

    [SerializeField] private LayerMask groundMask, playerMask, obstacleMask;
    [SerializeField] private Human human;

    [Header("Patroling")]
    private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField] private List<Transform> walkPoints;

    [Header("Materials")]
    [SerializeField] private Material robotLightMaterial;
    [SerializeField] private Light spotlight;

    [Header("Range")]
    [Range(0,60)] [SerializeField] private float sightRange;
    private bool playerInSightRange;

    [Header("Timer")]
    private float timeSinceLastSighting;
    private bool canChase;
    float timeChasingPlayer = 0f;

    private AudioSource robotMove;
    private AudioSource robotDetect;
    private bool startedChasing = false;

    private void Awake()
    {
        player = GameObject.Find("PlayerCat").transform;
        playerMovement = player.GetComponent<PlayerMovement>();
        playerVFX = player.Find("CatToon").GetComponent<VFX>();
        agent = GetComponent<NavMeshAgent>();

        playerInSightRange = false;
        canChase = true;
        timeSinceLastSighting = 4f;
        robotLightMaterial.SetColor("_Color", new Color(3f, 3f, 3f));
        spotlight.enabled = false;
    }

    private void OnEnable()
    {
        AssignAudioSources();
    }

    private void AssignAudioSources()
    {
        foreach (AudioSource source in GetComponentsInChildren<AudioSource>(true))
        {
            if (source.clip.name == "Moving")
            {
                robotMove = source;
            }
            else if (source.clip.name == "Detection")
            {
                robotDetect = source;
            }
        }
    }

    private void Update()
    {
        PlayerInSight();
        if (!playerInSightRange) Patroling();
        if (playerInSightRange)
        {
            if (!startedChasing)
            {
                robotDetect.Play();
                startedChasing = true;
            }
            ChasePlayer();
        }
        else
        {
            startedChasing = false;
        }
    }

    private void PlayerInSight()
    {
        TailStateManager state = player.gameObject.GetComponent<TailStateManager>();

        if (state.isPuffyActivated)
        {
            playerInSightRange = false;
        }
        else
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (!Physics.Raycast(transform.position, dirToPlayer, sightRange, obstacleMask))
            {
                FindPlayer();
            }
            else
            {
                playerInSightRange = false;
            }
        }

        if (timeSinceLastSighting < 5f && !playerInSightRange)
        {
            canChase = false;
            timeSinceLastSighting += Time.deltaTime;
            if (timeSinceLastSighting >= 5f)
            {
                canChase = true;
            }
        }
    }

    private void FindPlayer()
    {
        if(canFindPlayer)
        {
            Collider[] objects = Physics.OverlapSphere(transform.position, sightRange, playerMask);
        
            foreach (Collider obj in objects)
            {
                if (obj.CompareTag("Player") && canChase)
                {
                    playerInSightRange = true;
                }
            }
        }
    }

    private void Patroling()
    {
        spotlight.enabled = false;
        timeChasingPlayer = 0f;
        if (!walkPointSet) SearchWalkPoint();
        if(walkPointSet)
        {
            Vector3 direction = (walkPoint - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            if (Quaternion.Angle(transform.rotation, lookRotation) < 0.5f)
            {
                agent.SetDestination(walkPoint);
                robotMove.Play();
            }
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
        robotLightMaterial.SetColor("_Color", new Color(3f, 3f, 3f));
    }

    private void SearchWalkPoint()
    {
        int index = Random.Range(0, walkPoints.Count);
        walkPoint = walkPoints[index].position;
        walkPointSet = true;
    }

    private void ChasePlayer()
    {
        robotLightMaterial.SetColor("_Color", new Color(3f, 0f, 0f));
        spotlight.enabled = true;
        spotlight.transform.position = new Vector3(player.position.x, spotlight.transform.position.y, player.position.z);
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance > 3.5f)
        {
            Vector3 targetPosition = player.position - direction.normalized * 2f;
            robotMove.Play();
            agent.SetDestination(targetPosition);
            Renderer renderer = GetComponent<Renderer>();
        }
        timeChasingPlayer += Time.deltaTime;
        if (timeChasingPlayer > 5f)
        {
            PlayerDetected();
        }

        timeSinceLastSighting = 0f;
    }

    private void PlayerDetected()
    {
        playerVFX.PlayPlayerSoundEffect("Hissing");
        playerMovement.isScared = true;
        canFindPlayer = false;
        gameObject.GetComponent<RobotAi>().enabled = false;
        spotlight.enabled = false;
        human.Invoke("PlayerIsScared", 2f); 
    }
    
    public void CanFindPlayer(bool canFind)
    {
        canFindPlayer = canFind;
    }

    public void ResetRobot()
    {
        transform.position = walkPoints[0].position;
        canFindPlayer = true;
        gameObject.GetComponent<RobotAi>().enabled = true;
        timeSinceLastSighting = 0f;
        timeChasingPlayer = 0;

        playerInSightRange = false;
        startedChasing = false;
    }
}
