using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickedObject : MonoBehaviour
{
    public Sprite sprite;
    public SpriteRenderer target;
    public float gravity;
    public float initialVY;

    float vy = 0f;
    // Start is called before the first frame update
    void Start()
    {
        vy = initialVY;
        target.flipY = true;
        target.sprite=sprite;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(-16f*Time.deltaTime,vy*Time.deltaTime,0f);
        vy-=gravity*Time.deltaTime;
    }
}
