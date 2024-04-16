using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyOC : MonoBehaviour
{
    public float distanceFactor;
    public List<GameObject> objects;
    public Transform source;
    public float checkFactor = 2f;

    float timer = 0f;
    void Update()
    {
        this.timer += Time.deltaTime;
        if (this.timer > checkFactor) {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] == null) { objects.RemoveAt(i); break; }else { 
                objects[i].SetActive(Vector2.Distance((Vector2)objects[i].transform.position, (Vector2)source.position) < distanceFactor);
                }
            }
            this.timer = 0f;
        }
    }
}
