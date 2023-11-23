using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateMap))]
public class CreateMapInspector : Editor
{
    public void OnEnable()
    {
        CreateMap createMap = (CreateMap)target;
        createMap.UpdateCube();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CreateMap createMap = (CreateMap)target;
        createMap.MapSize = new Vector2Int(
            EditorGUILayout.IntSlider("X", createMap.MapSize.x, 2, 10),
            EditorGUILayout.IntSlider("Y", createMap.MapSize.y, 2, 10)
            );

        createMap.RandCount = EditorGUILayout.IntSlider(
            "RandCount", 
            createMap.RandCount, 
            3, 
            createMap.MapSize.x * createMap.MapSize.y
            );

        if (createMap.KeepMapSize != createMap.MapSize)
        {
            createMap.UpdateCube();
        }

        if (GUILayout.Button("·£´ý »ý¼º"))
        {
            createMap.CreateRandomMap();
        }
    }
}