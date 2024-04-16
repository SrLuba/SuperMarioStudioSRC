using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : MonoBehaviour
{
    public Animator anim;
    public GameObject BrickShatter;
    public Vector3 ShatterOffset;
    public int howManyShatters = 4;
    public float velocityMultiplier;
    public AudioClip ShatterAC, BumpAC;
    public LayerMask enemyLayer, pickupLayer;
      public AudioClip kickAC;
    public void HitCheckUp() {
        RaycastHit2D mehit = Physics2D.Raycast((Vector2)this.transform.position,Vector2.up,9f,enemyLayer);
        RaycastHit2D rehit = Physics2D.Raycast((Vector2)this.transform.position+new Vector2(9f,0f),Vector2.up,9f,enemyLayer);
        RaycastHit2D lehit = Physics2D.Raycast((Vector2)this.transform.position-new Vector2(9f,0f),Vector2.up,9f,enemyLayer);
        RaycastHit2D mphit = Physics2D.Raycast((Vector2)this.transform.position,Vector2.up,9f,pickupLayer);
        RaycastHit2D rphit = Physics2D.Raycast((Vector2)this.transform.position+new Vector2(9f,0f),Vector2.up,9f,pickupLayer);
        RaycastHit2D lphit = Physics2D.Raycast((Vector2)this.transform.position-new Vector2(9f,0f),Vector2.up,9f,pickupLayer);
        
        if (mehit.collider!=null) {
            // enemy is up
            Rigidbody2D rb = mehit.collider.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 125f);

            mehit.collider.gameObject.GetComponent<EnemyScript>().Die(1);

            SoundManager.instance.Play(656,kickAC,1f,1f);
        }
         if (rehit.collider!=null) {
            // enemy is up
            Rigidbody2D rb = rehit.collider.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 125f);

            rehit.collider.gameObject.GetComponent<EnemyScript>().Die(1);

            SoundManager.instance.Play(656,kickAC,1f,1f);
        }
         if (lehit.collider!=null) {
            // enemy is up
            Rigidbody2D rb = lehit.collider.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 125f);

            lehit.collider.gameObject.GetComponent<EnemyScript>().Die(1);

            SoundManager.instance.Play(656,kickAC,1f,1f);
        }
        if (mphit.collider!=null) {
            // pickup is up
            Rigidbody2D rb = mphit.collider.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 125f);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        if (collision.GetComponent<Rigidbody2D>().velocity.y <= 0f) return;

        if (Global.PlayerState.One.powerUp > 0)
        {
			 collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, collision.GetComponent<Rigidbody2D>().velocity.y*velocityMultiplier);
            Shatter();
			
        }
        else {
			 collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, 0f);
            anim.Play("Hit", 0);
			SoundManager.instance.Play(1853, BumpAC);
            
        }
       HitCheckUp();
    }
    public void Shatter() {
        Instantiate(BrickShatter, this.transform.position + ShatterOffset + new Vector3(Random.Range(-8f,8f),Random.Range(-8f,8f),0f), Quaternion.identity);
        SoundManager.instance.Play(1852, ShatterAC);
		SoundManager.instance.Play(1853, BumpAC);
        Destroy(this.gameObject);
    }
}
