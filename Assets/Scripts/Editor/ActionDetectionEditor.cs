using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(ActionDetection))]
public class ActionDetectionEditor : Editor
{
    private void OnSceneGUI()
    {
        ActionDetection fov = (ActionDetection)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.orientation.position, Vector3.up, fov.orientation.forward, 360, fov.fovRadius);
        Vector3 viewAngleA = fov.DirFromAngle(-fov.fovAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.fovAngle / 2, false);

        Handles.DrawLine(fov.orientation.position, fov.orientation.position + viewAngleA * fov.fovRadius);
        Handles.DrawLine(fov.orientation.position, fov.orientation.position + viewAngleB * fov.fovRadius);

        Handles.color = Color.red;
        foreach (Transform visibleActions in fov.visibleActions)
        {
            Handles.DrawLine(fov.orientation.position, visibleActions.position);
        }
    }
}
