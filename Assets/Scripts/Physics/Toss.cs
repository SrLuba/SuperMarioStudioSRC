using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Toss : MonoBehaviour
{
    public AudioClip clip, destroyClip;
    public float vel;
    public bool right;
    public Rigidbody2D rb;
   
    public GameObject particle;
    public Vector3 particleOffset;
    public float particleUpdate;

    public GameObject destroyParticle;

    float timer;
    public BasicTrigger trig;
    public float distanceFactor;
    public float rayDistance, rayDistanceSides;
    public float yOffsetSides;
    public Vector3 rayOffsetSides;
    public LayerMask SolidMask;
    public float jumpForce;

    Transform sourcedst;

    bool airFix = false;
    // Start is called before the first frame update
    void Start()
    {
        sourcedst = Camera.main.transform;
        SoundManager.instance.Play(12525, clip);
    }
    private void Update()
    {

        if (Physics2D.Raycast(this.transform.position, Vector2.down, rayDistance, SolidMask).collider !=null) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            airFix=true;
        }else {
            airFix=false;
        }
        if (trig.colliding) End();
    }
    public void End() {
        Instantiate(destroyParticle, this.transform.position + particleOffset, Quaternion.identity);
        SoundManager.instance.Play(12528, destroyClip);
        Destroy(this.gameObject);
    }
    public void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Enemy") {
            col.gameObject.GetComponent<EnemyScript>().Die(2);
            End();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (sourcedst == null) {
            sourcedst = Camera.main.transform;
            return;
        }
        rb.velocity = new Vector2((right) ? vel : -vel, rb.velocity.y);
        timer += Time.deltaTime;

        if (timer > particleUpdate) {
            Instantiate(particle, this.transform.position + particleOffset, Quaternion.identity);
            timer = 0f;
        }

        if (Vector3.Distance(this.transform.position, sourcedst.position) > distanceFactor) Destroy(this.gameObject);

    }
}
