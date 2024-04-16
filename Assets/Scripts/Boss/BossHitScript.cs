using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitScript : MonoBehaviour
{
    public BossScript scr;
    public bool groundPoundMust = false;

    public bool canHit = true;
    float timer = 0f;

    public void Update() {
        if (canHit) return;

        timer+=Time.deltaTime;
        
        if (timer>1f)  {timer=0f;canHit=true; }  
          }
    public void TryHit(){
        

        if(scr.Hit()) canHit=false;

    }
}
