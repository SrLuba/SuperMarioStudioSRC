using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOclussionCulling : MonoBehaviour
{
    public int frameUpdate = 2;
    public float distance;
    public Transform ocSource;
    public List<string> targetTags;
    List<GameObject> cullTargets;

    public void Start()
    {
        
    }
    public void OclussionUpdate() {
       
    }
    int cCounter = 0;
    void Update()
    {
        cCounter++;
        if (cCounter > frameUpdate) {
            

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < enemies.Length; i++) {
                enemies[i].GetComponent<EnemyScript>().enabled = (Vector2.Distance(enemies[i].transform.position, ocSource.position) < 512f); 
            }

            cCounter = 0;
        }
    }
}
