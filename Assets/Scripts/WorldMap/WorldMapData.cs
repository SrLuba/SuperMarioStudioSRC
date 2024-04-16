using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] public class WorldMap_Item {
    public string Name;
    public string Description;
    [Header("Negative number for nothing")]public int levelNumber;
    [Header("Temporal for demo")] public string LevelScene;
    public Vector3 position;
    public int up = -1; public int right = -1; public int down = -1; public int left = -1;
    public int transitionNumber = 1;
}
[CreateAssetMenu]public class WorldMapData : ScriptableObject
{
    public AudioClip music;
    public List<WorldMap_Item> items;

}
