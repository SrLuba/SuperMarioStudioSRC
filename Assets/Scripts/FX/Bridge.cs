using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public float factor, magnitude;
    public float amplitude;
    public float realMagnitude = 16f;

    public bool fall;
    public float gravity;
    public float wait=1f;
    float waitInt=0f;
    float vy=0f;

    bool canFall=false;

    public Transform player;

    public float chMaxY, chMinY;
    float distance = 0f;
    public float maxDistanceFallPlayer;
   
    void Start()
    {
        if (PlayerPhysics2D_Demo.instance==null) this.enabled=false;
        if (player==null) player = PlayerPhysics2D_Demo.instance.gameObject.transform;
    }
    
   
    void Update()
    {
        if (PlayerPhysics2D_Demo.instance==null) this.enabled=false;
        if (player==null) player = PlayerPhysics2D_Demo.instance.gameObject.transform;
        if (player==null) return;
        distance = Vector2.Distance(player.position, this.transform.position)*realMagnitude;
        this.transform.Translate(0f,vy*Time.deltaTime,0f);
        if (fall && (PlayerPhysics2D_Demo.instance.Grounded) && Vector2.Distance(player.position, this.transform.position)<16f){canFall=true;}
        if (canFall){
            waitInt+=Time.deltaTime;
            if(waitInt>wait){
                vy-=gravity*Time.deltaTime;
            }
            if(waitInt>10f){
                Destroy(this.gameObject);
            }
        }
        //if(!(PlayerPhysics2D_Demo.instance.Grounded)) waitInt=0f;
        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, new Vector3(this.transform.localPosition.x,(PlayerPhysics2D_Demo.instance.Grounded) ?
         Mathf.Clamp(distance*magnitude, chMinY, chMaxY) : chMaxY, 0f), 15f*Time.deltaTime);
    }
}
