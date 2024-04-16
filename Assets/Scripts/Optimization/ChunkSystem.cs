using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSystem : MonoBehaviour
{

    public string chunkTagTarget = "Solid";

    [Header("Chunk Size X")] public int chunkSizeX = 256;
    [Header("Chunk Size Y")] public int chunkSizeY = 256;

    int currentX = 0;

    GameObject[] targets;

    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag(chunkTagTarget);
    }

   
    void FixedUpdate()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].SetActive((targets[i].transform.position.x > currentX && targets[i].transform.position.x < currentX+chunkSizeX) ? true : false);
        }
        if (PlayerPhysics2D_Demo.instance.transform.position.x > currentX)
        {
            currentX += chunkSizeX;
        }
        else if (PlayerPhysics2D_Demo.instance.transform.position.x < currentX) {
            currentX -= chunkSizeX;
        }
    }
}
