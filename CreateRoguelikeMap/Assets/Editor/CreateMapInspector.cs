using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateMap))]
public class CreateMapInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CreateMap createMap = (CreateMap)target;

    }
}
