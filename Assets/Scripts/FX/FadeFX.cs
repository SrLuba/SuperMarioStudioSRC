using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeFX : MonoBehaviour
{
    public SpriteRenderer target;

    public float speed;
    float a = 1f;

    void Start()
    {
        if (target==null) target=this.GetComponent<SpriteRenderer>();
        if (target==null) this.enabled=false;
    }

    void FixedUpdate()
    {
        target.color = new Color(target.color.r, target.color.g, target.color.b, a);
        a -= Time.deltaTime*speed;

        if (a <= 0f) Destroy(target.gameObject);
    }
}
