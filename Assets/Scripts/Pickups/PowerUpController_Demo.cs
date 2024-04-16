using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController_Demo : MonoBehaviour
{
	public Animator anim;
    public PlayerPhysics2D_Demo physics;
    public PowerUpData selfPickup;
    public AnimSpriteSetter spriteAnimator;

    public PowerUpDatabase db;



    [Header("Star Particles")]public ParticleSystem starParticles;
    bool started= false;
    public AudioClip startRunningOutAC, marioGrabStarClipA, marioGrabStarClipB, StarMusicLoop;
    public AudioClip powerUpAC;
    public bool noSetAnimator=false;
    public void Awake() {Global.PowerUpDB=db;}
    public void Update()
    {
      
        selfPickup=db.data[Global.PlayerState.One.powerUp];

        if (Global.PlayerState.One.star) {
             if (!started) {starParticles.Play(); started=true;}
        }else {
            starParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            started=false;
        }
        Global.PlayerState.One.UpdateStar(startRunningOutAC);


        spriteAnimator.sprites = selfPickup.sprites;

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PowerUp")
        {
            if (collision.GetComponent<PowerUp>().data.star) {
                Global.PlayerState.One.GetStar(marioGrabStarClipA, marioGrabStarClipB, StarMusicLoop,collision.GetComponent<PowerUp>().data.starTime);
                SoundManager.instance.Play(112324,powerUpAC,1f,1f);
            } else {
                collision.GetComponent<PowerUp>().Grab();
                this.physics.GrabPowerUp(collision.GetComponent<PowerUp>().data);    
            }
             Destroy(collision.gameObject);
        }
    }
}
