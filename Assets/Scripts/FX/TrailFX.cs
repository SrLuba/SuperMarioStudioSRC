using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFX : MonoBehaviour
{
    public bool setSprite = true;
    public bool setAngle = true;
    public bool setScale = true;
    public Vector3 offset;
    public Transform customTarget;
    public SpriteRenderer selfRend;
    public GameObject TrailFXg;
    public float time;

    float timer;
    // Start is called before the first frame update
    void Start()
    {
        if (selfRend==null && setSprite) selfRend=this.GetComponent<SpriteRenderer>();
        if (selfRend==null && setSprite) setSprite=false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer+=Time.deltaTime;

        if (timer>time) {
            GameObject dummy = Instantiate(TrailFXg,this.transform.position+offset,Quaternion.identity);
            if (setSprite) 
            {
                dummy.GetComponent<SpriteRenderer>().sprite = selfRend.sprite;
                if (setAngle) dummy.transform.eulerAngles = new Vector3(dummy.transform.eulerAngles.x,dummy.transform.eulerAngles.y,selfRend.transform.eulerAngles.z);
                if (setScale) dummy.transform.localScale = selfRend.transform.localScale;
            }
            if (customTarget!=null) {
                if (setAngle) dummy.transform.eulerAngles = new Vector3(customTarget.eulerAngles.x,customTarget.eulerAngles.y,customTarget.eulerAngles.z);
                if (setScale) dummy.transform.localScale = customTarget.localScale;
            }
            timer=0f;
        }
    }
}
