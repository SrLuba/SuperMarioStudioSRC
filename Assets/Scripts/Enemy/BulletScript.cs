using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public float distance;
    public bool right;

    public bool Unkillable = false;

    public Vector2 direction;

    public Sprite selfSprite;
    public GameObject kickedObject;

    public GameObject Parent;

    public AudioClip ACStart;

    public bool gravity;
    public float gravityForce;
    public float startingVX, startingVY;
    public float VX, VY;

    float ogX = 0f;
    float maxX = 2555f;
    // Start is called before the first frame update
    void Start()
    {
        ogX = this.transform.position.x;
        if (ACStart != null) SoundManager.instance.Play(2516, ACStart, 1f, 1f);
        
    }
    public void Die() {
        if(Unkillable)return;
        Instantiate(kickedObject,this.transform.position,Quaternion.identity).GetComponent<KickedObject>().sprite = selfSprite;
        if(Parent!=null) Destroy(Parent);
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        maxX = ogX + distance; 
        if (this.transform.position.x > maxX && right) Destroy(this.gameObject);
        if (this.transform.position.x < -maxX && !right) Destroy(this.gameObject);
        if (right) direction = new Vector2(Mathf.Abs(direction.x), direction.y);
        this.transform.Translate(direction*speed*Time.deltaTime);
        
        if (gravity) {
            this.transform.Translate(VX*Time.deltaTime, VY*Time.deltaTime, 0f);
            VY-= gravityForce*Time.deltaTime;
            VX = Mathf.Lerp(VX,0f,5f*Time.deltaTime);
        }



    }
}
