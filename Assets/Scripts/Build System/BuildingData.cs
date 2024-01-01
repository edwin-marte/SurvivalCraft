using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Building System/Build Data")]
public class BuildingData : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public float gridSnapSize;
    public GameObject prefab;
    public Vector3 buildingSize;
    public PartType partType;
}

public enum PartType
{
    Room = 0,
    Corridor = 1,
    Decoration = 2
}

#if UNITY_EDITOR
[CustomEditor(typeof(BuildingData))]
public class BuildingDataEditor : Editor {
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height) {
        var data = (BuildingData)target;

        if (data == null || data.icon == null) return null;

        var texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(data.icon.texture, texture);
        return texture;
    }
}
#endif