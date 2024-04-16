using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashScript : MonoBehaviour
{
    bool enab = false;
    public SpriteRenderer rend;
    public float step;

    float timer = 0f;

    float timer2 = 0f;

    public float flashTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        if (enab)
        {
            timer += Time.deltaTime;
            timer2 += Time.deltaTime;
            if (timer >= step)
            {
                rend.enabled = !rend.enabled;
                timer = 0f;
            }

            if (timer2 >= flashTime) {
                Global.PlayerState.One.vulnerable = true;
                timer2 = 0f;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        enab = (!Global.PlayerState.One.vulnerable) || Global.PlayerState.One.star;

        if (!enab) {
            rend.enabled = true;
        }
    }
}
