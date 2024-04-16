using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarVisualSCR : MonoBehaviour
{
    public bool enab = false;
    public SpriteRenderer rend;
    public TrailFX fx1;

    public Material normalMat, starMat;
    public float step;

    float timer = 0f;

    float timer2 = 0f;

    public float flashTime;

    public int flashTimer;

    Color color;

    private void FixedUpdate()
    {
        fx1.enabled=enab;
        rend.material = (enab) ? starMat : normalMat;
        if (!enab) rend.color = Color.Lerp(rend.color, new Color(1f,1f,1f,1f), 25f*Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        enab = Global.PlayerState.One.star;
    }
}
