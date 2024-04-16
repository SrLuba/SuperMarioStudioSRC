using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSingle : MonoBehaviour
{
    public float speed, magnitude, factor;

    float yOG, xOG;

    void Start() {
        yOG=this.transform.localPosition.y;
        xOG=this.transform.localPosition.x;
        }

    void Update()
    {
        this.transform.localPosition = new Vector3(xOG, yOG+Mathf.Sin(Time.time*speed+(this.transform.position.x*factor))*magnitude, 0f);
    }
}
