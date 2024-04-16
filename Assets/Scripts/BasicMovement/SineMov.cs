using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMov : MonoBehaviour
{
    public Vector2 amplitude, speed;
    public Transform target;
    public bool local = true;
    Vector2 ogPos;
    public float affectPlayer;
    public Vector2 offsetSin;

    public void Awake()
    {
        ogPos = (local) ? (Vector2)target.localPosition : (Vector2)target.position;
    }
    void FixedUpdate()
    {
        offsetSin = new Vector2(Mathf.Cos(Time.time * speed.x) * amplitude.x, Mathf.Sin(Time.time * speed.y) * amplitude.y);
        if (local)
        {
            target.localPosition = ogPos + offsetSin;
        }
        else {
            target.position = ogPos + offsetSin;
        }
        
    }
}
