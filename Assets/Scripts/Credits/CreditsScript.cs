using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScript : MonoBehaviour
{
    public float wait;
    public MenuScript scr;
    bool w=true;
    float t;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t+=Time.deltaTime;
        if(t>wait && w) {
            scr.StartSRequest();
            w=false;
        }
    }
}
