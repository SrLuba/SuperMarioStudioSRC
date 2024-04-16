using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ContainerType {
    Pickup,
    Coins
}
public class ContainerBlock : MonoBehaviour
{
    public bool upgradable;

    public ContainerType type;

    public Animator anim;


    public PowerUpData data;
    public int pickupCount = 1;
    public SpriteRenderer rend;
    public bool alreadyHit;
    public float velocityMultiplier;
    public AudioClip Bump, Power, CoinAC;

    public LayerMask enemyLayer, pickupLayer;

    public AudioClip kickAC;

    public GameObject coinJumpPrefab;

    public void EndSpawning() {
        Instantiate(data.prefab, rend.transform.position, Quaternion.identity);
    }
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
		if (this.alreadyHit) {collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, 0f);
       SoundManager.instance.Play(5, Bump); return; }
        if (collision.GetComponent<Rigidbody2D>().velocity.y <= 0f) return;
        
        collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, 0f);
        
        if (pickupCount > 1) {
            anim.Play("Hit", 0, 0f);
           
        }else {
            anim.Play("HitFinal", 0, 0f);
            
            alreadyHit = true;
           
        }
        if (type!=ContainerType.Coins) SoundManager.instance.Play(125597, Power);
        SoundManager.instance.Play(5, Bump);
         HitCheckUp();
        if (type==ContainerType.Coins) {
            //Instantiate(coinJumpPrefab, this.transform.position, Quaternion.identity);
            SoundManager.instance.Play(157, CoinAC);
            Global.PlayerState.coins++;
        }

        pickupCount--;
    }
    // Update is called once per frame
    void Update()
    {
        if (upgradable) data=Global.PowerUpDB.data[(Global.PlayerState.One.powerUp>0) ? 2 : 1];
        rend.sprite = data.sprite;
    }
}
