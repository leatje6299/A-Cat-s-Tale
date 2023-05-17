using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private static Game instance;
    public Vector3 lastCheckPoint;

    [SerializeField] private GameObject bathroomWater;
    public Vector3 bathroomWaterYLevel;
    [SerializeField] private List<GameObject> glasses;
    private List<Vector3> glassesPosition = new List<Vector3>();
    private List<Quaternion> glassesRotation = new List<Quaternion>();
    [SerializeField] private GameObject plank;
    private Vector3 plankPosition;
    private Quaternion plankRotation;

    private WaterRising waterRisingScript;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        waterRisingScript = bathroomWater.GetComponent<WaterRising>();
        SetCheckPointObjects();
    }

    private void SetCheckPointObjects()
    {
        bathroomWaterYLevel = bathroomWater.transform.localPosition;
        plankPosition = plank.transform.position;
        plankRotation = plank.transform.rotation;

        for(int i = 0; i < glasses.Count; i++)
        {
            glassesPosition.Add(glasses[i].transform.position);
            glassesRotation.Add(glasses[i].transform.rotation);

        }
    }

    public void ResetCheckPointObjects()
    {
        bathroomWater.transform.localPosition = bathroomWaterYLevel;
        waterRisingScript.waterPosition = bathroomWaterYLevel;

        plank.transform.position = plankPosition;
        plank.transform.rotation = plankRotation;

        for (int i = 0; i < glasses.Count; i++)
        {
            glasses[i].transform.position = glassesPosition[i];
            glasses[i].transform.rotation = glassesRotation[i];
        }
    }
}
