using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ActionDetection : MonoBehaviour
{
    public float fovRadius;
    [Range(0, 360)] public float fovAngle;
    [SerializeField] private LayerMask actionMask;
    [SerializeField] private LayerMask obstacleMask;

    public List<Transform> visibleActions = new List<Transform>();
    private List<Transform> swingActions = new List<Transform>();

    public Transform orientation;

    [SerializeField] private Image displayActionIcon;

    public Transform closestPushAction;
    public void Update()
    {
        DetectVisibleAction();
        closestPushAction = getClosestAction(false);
    }

    public void DetectVisibleAction()
    {
        visibleActions.Clear();

        Collider[] actionsInFOV = Physics.OverlapSphere(orientation.position, fovRadius, actionMask);
        
        for(int i = 0; i < actionsInFOV.Length; i++)
        {
            Transform action = actionsInFOV[i].transform;
            Vector3 dirToAction = (action.position - orientation.position).normalized;
            
            if(Vector3.Angle(orientation.forward,dirToAction) < fovAngle/2)
            {
                float distToAction = Vector3.Distance(orientation.position, action.position);
                if (!Physics.Raycast(orientation.position,dirToAction,distToAction,obstacleMask))
                {
                    visibleActions.Add(action);
                }
            }
        }    
    }

    public List<Transform> GetVisibleAction()
    {
        return visibleActions;
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += orientation.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public Transform getClosestAction(bool swing)
    {
        string tagToSearch = swing ? "swingSpot" : "pushSpot";
        Transform closestAction = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform action in visibleActions)
        {
            if (action.CompareTag(tagToSearch))
            {
                float distance = Vector3.Distance(orientation.position, action.position);
                if (distance < closestDistance)
                {
                    closestAction = action;
                    closestDistance = distance;
                }
            }
        }

        return closestAction;
    }

}
