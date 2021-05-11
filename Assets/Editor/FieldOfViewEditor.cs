using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(Entity))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        Entity entity = (Entity)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(entity.transform.position, Vector3.up, Vector3.forward, 360, entity.entityStats.DetectionRange);

        Vector3 viewAngleA = entity.DirFromAngle(-entity.entityStats.ViewAngle / 2, false);
        Vector3 viewAngleB = entity.DirFromAngle(entity.entityStats.ViewAngle / 2, false);

        Handles.DrawLine(entity.transform.position, entity.transform.position + viewAngleA * entity.entityStats.DetectionRange);
        Handles.DrawLine(entity.transform.position, entity.transform.position + viewAngleB * entity.entityStats.DetectionRange);
    }

}
