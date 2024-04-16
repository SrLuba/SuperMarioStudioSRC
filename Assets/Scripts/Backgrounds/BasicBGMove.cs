using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBGMove : MonoBehaviour
{
    public float distance;
    public float speed;
    void Update()
    {
        this.transform.Translate(-speed*Time.deltaTime, 0f, 0f);
        if (this.transform.localPosition.x < distance) {
            this.transform.localPosition = new Vector3(0f,this.transform.localPosition.y,0f);
        }
    }
}
