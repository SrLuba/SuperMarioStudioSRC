using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpData data;
    bool side = false;
    public Rigidbody2D rb;
    public LayerMask solidLayer;

    public Sprite StylishSprite;

    public bool Bounce;
    public float BounceForce;
    public AudioClip BounceSFX;

    public AudioClip HadSFX;


    public void Awake()
    {
        side = Random.Range(0, 2) <= 0 ? true : false;
    }
    public SpriteRenderer rend;

    public void Start() {rend.sprite=data.sprite;}
    public void FixedUpdate()
    {
        rb.velocity = (!Global.Game.systemHalt) ? new Vector2(side ? data.moveSpeed : -data.moveSpeed, rb.velocity.y) : new Vector2(0f,0f);
    }
    public void Update() {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 9f, solidLayer) && Bounce) {
            if (BounceSFX!=null) SoundManager.instance.Play(251982,BounceSFX,1f,1f);
            rb.velocity = new Vector2(rb.velocity.x, BounceForce);
        }
        if (Physics2D.Raycast(this.transform.position, (side) ? Vector2.right : Vector2.left, 9f, solidLayer)) side=!side;
    }
    public void Grab() {
        if (Global.PlayerState.One.powerUp>=data.id) 
        {
            SoundManager.instance.Play(2527,HadSFX,1f,1f);
            Global.finalScore+=2;
            ScoreController.instance.GetPointsExplicit(50000, null, StylishSprite, this.transform.position);
        }
    }
}
