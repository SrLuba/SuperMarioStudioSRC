using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeGeneral : MonoBehaviour
{
    public float factor, magnitude;
    public float amplitude;
    public float realMagnitude = 16f;

    public Transform player;

    public float chMaxY, chMinY;
    float distance = 0f;
   
    void Start()
    {
    }
    
   
    void Update()
    {

        distance = Vector2.Distance(player.position, this.transform.position)*realMagnitude;
        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, new Vector3(this.transform.localPosition.x,Mathf.Clamp(distance*magnitude, chMinY, chMaxY), 0f), 15f*Time.deltaTime);
    }
}
