using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float bounceFactor;
    public bool side;
    public float startBounceForce;
    public float speed;

    public Transform sprite;
    public float rotateSpeed;
    public SpriteRenderer Rend;
    public string layerA, layerB;
    public float bounceSpeedFactor;
    public GameObject splash;
    public Vector3 splashSize, splashOffset;

    public float minSJ, maxSJ, minSP, maxSP;

    float bounceRatio = 0f;

    float starterTimer = 0f;
    public void Start()
    {
        Rend.sortingLayerName = Random.Range(-1, 1) == 0 ? layerA : layerB;
        speed = Random.Range(minSP, maxSP);
        startBounceForce = Random.Range(minSJ, maxSJ);
        side = Random.Range(-1,1)==0 ? true : false;
        rb.velocity = new Vector2(rb.velocity.x, Random.Range(minSJ, maxSJ));
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (starterTimer < 1f) return;
        if (collision.gameObject.layer != LayerMask.NameToLayer("Solid")) {
            if (collision.tag == "Water") {
                Instantiate(splash, this.transform.position+ splashOffset, Quaternion.identity).transform.localScale = splashSize;
                return;
            }
        }



        rb.velocity = new Vector2(rb.velocity.x, bounceFactor * bounceRatio);
        bounceRatio = 0f;
        speed = (side) ? speed -  bounceFactor * bounceRatio * bounceSpeedFactor : speed + bounceFactor * bounceRatio * bounceSpeedFactor;
    }
    public void FixedUpdate()
    {
        starterTimer += Time.deltaTime;
        sprite.Rotate(0f, 0f, (side) ? (rotateSpeed * Time.deltaTime) : -(rotateSpeed * Time.deltaTime));
        bounceRatio += Time.deltaTime;
        rb.velocity = new Vector2(side ? speed : -speed, rb.velocity.y);
        if (Vector3.Distance(this.transform.position, Camera.main.transform.position) > 512f) Destroy(this.gameObject);

    }
}
