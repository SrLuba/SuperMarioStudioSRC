using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyScript : MonoBehaviour
{
    public LayerMask floorLayer, playerLayer;  
    public EnemyTrigger left, right;

    public EnemyData data;
    public Animator anim;
    public Rigidbody2D rb;
    public Collider2D mainCollider;
    public SpriteRenderer rend;
    int hits;
    public Vector2 offsetRay;
    public AudioClip splashAC;
    public float rayDefaultDistance;
    public Transform TSprite;

    public float changeDirectionWait;
    public bool mainThreadHalt = false;
    public bool startingMovingSide = true;
    public int myState = 0;
    public bool movingSide = false;

    public Vector3 offsetSplash;
    public GameObject SplashGB;
    public GameObject kickedObject;
    float originalSX = 0f;
	
	public bool M_Wing;
	public bool M_Bounce;
	public bool M_Red;
	
	public float r_box;
	
	[HideInInspector] public bool Grounded;
	public float groundRayDistance;
	
	public float jumpForceLow, jumpForceHigh;
	
	float bounce_timer = 0f;
	[HideInInspector]public float bounce_haltTime = 1.1f;
	
	int bounce_index = 0;
	int bounce_bounceMaxIndex = 4;
	
	int tries = 0;
	int maxTries = 5;

	public bool playerIsBehind = false;
    float fixRed = 0f;

	public void RedMechanic() {
		if (!Grounded) return;

        if(!Physics2D.Raycast(this.transform.position+new Vector3((movingSide) ? 9f : -9f,2f,0f),Vector2.down,12f,floorLayer)) {
            ChangeSide();
        }
	}
	
    void Start()
    {
        this.anim.runtimeAnimatorController = (M_Red) ? ((M_Bounce) ? data.ACRedWings : data.ACRed) : (M_Bounce) ? data.ACWings : data.ACNormal;
        this.hits = data.hits;
        this.originalSX = TSprite.localScale.x;
        this.movingSide = this.startingMovingSide;
    }

    bool sideChange = false;
    float taimer = 0f;
    public void ChangeSide() {
        if (this.mainThreadHalt) return;
        if (this.sideChange) return;
        this.movingSide = !this.movingSide;
        this.sideChange = true;
    }
   
    [HideInInspector]public bool died = false;
    public IEnumerator AlreadyDead(int typee) { 
        SoundManager.instance.Play((typee==3) ?  data.DestroyAC : data.DieAC);  yield return new WaitForSeconds(4f); Destroy(this.gameObject);  
    }
    public void Die(int typee) {
        if (this.died) return;
    
        if (data.spawnDeath != null && typee == 0) {
            Instantiate(data.spawnDeath, this.transform.position, Quaternion.identity);
            
            ScoreController.instance.GetPoints(true, true, 1, this.transform.position);

            Destroy(this.gameObject);
        }
        if (typee==1) {
            Instantiate(kickedObject, this.transform.position, Quaternion.identity).GetComponent<KickedObject>().sprite = rend.sprite;
            
            ScoreController.instance.GetPoints(true, true, 1, this.transform.position);

            Destroy(this.gameObject);
        }
        if (hits > 0) 
        {
            ScoreController.instance.GetPoints(true, true, 1, this.transform.position);
            hits--; 
  
            anim.Play("Hit"+typee.ToString(), 0);
        }else { 
            ScoreController.instance.GetPoints(true, true, 1, this.transform.position);
            rb.isKinematic = true;
            anim.Play("Death"+typee.ToString(), 0);
            this.mainThreadHalt = true;
   
            this.died = true;
            StartCoroutine(AlreadyDead(typee));
        }
    }
    public AudioClip LavaDie;
    public void WaterCheck() {

            if (!this.died) return;
            Instantiate(SplashGB, this.transform.position+ offsetSplash, Quaternion.identity);
            slowFall = true;
        
    }
    public void OnTriggerEnter2D(Collider2D other) {
         if (other.tag == "Water") WaterCheck();
         if (this.died) return;

        if (other.tag =="Lava") {
            SoundManager.instance.Play(15229, LavaDie,1f,1f);
            SoundManager.instance.Play(100,splashAC,0.5f,Random.Range(0.8f,1.2f));
            Die(2);
        }else if (other.tag == "Enemy") {
            ChangeSide();
        }
    }
 

    float dvy = 64f;
    bool slowFall = false;

    float bTimer = 0f;
    float b2Timer = 0f;
    float tTimer = 1f;
    public IEnumerator Throw() {
        for(int i = 0; i < data.bulletCount; i++) {
            BulletScript scr= Instantiate(data.bullet, this.transform.position, Quaternion.identity).GetComponent<BulletScript>();
            rb.velocity = new Vector2(rb.velocity.x, jumpForceHigh);
            yield return new WaitForSeconds(0.5f);
        }
    }
    void FixedUpdate()
    {

        if (this.sideChange)
        {
            taimer += Time.deltaTime;
            if (taimer > .2f) { this.sideChange = false; taimer = 0f; }
        }
        if (this.died) { 
            rb.velocity = new Vector3(16f, dvy); 
            dvy -= 320f*(slowFall ? 0.5f : 1f)*Time.deltaTime; 
            TSprite.eulerAngles += new Vector3(0f,0f,200f*Time.deltaTime);
           
        }
        mainCollider.enabled = !this.died;


        if (mainThreadHalt) return;

        if (data.type.Contains("B_HAMMER")){
            bTimer+=Time.deltaTime;
            b2Timer+=Time.deltaTime;
            if(b2Timer>=tTimer){
                movingSide=!movingSide;
                tTimer=Random.Range(0.5f,2f);
                b2Timer=Time.deltaTime;
            }
            if(bTimer > data.bulletTime) {
                StartCoroutine(Throw());
                bTimer=0f;
            }
        }
        if (data.type.Contains("B_GOOMBA"))
        {
            if (Grounded)anim.Play("Move", 0);
            anim.speed = Global.Game.systemHalt ? 0f : 1f;
            if (!Global.Game.systemHalt && !M_Wing) rb.velocity = new Vector2(movingSide ? data.baseSpeed : -data.baseSpeed, rb.velocity.y);
        }
        if (Global.Game.systemHalt) {
             rb.velocity = new Vector2(0f,0f);
        }
    }
    public void BounceMechanic() {
        if (!M_Bounce) return;
        if (died) return;

        if (Grounded) {
            bounce_timer+=Time.deltaTime;
            if (bounce_timer >= bounce_haltTime) {
                rb.velocity = new Vector2(rb.velocity.x, (bounce_index < 3) ? jumpForceLow : jumpForceHigh);
                bounce_index++;
                if (bounce_index > 4) {
                    bounce_index = 0;
                    rb.velocity = new Vector2(rb.velocity.x, jumpForceLow);
                    bounce_timer=0f;
                    
                    if (playerIsBehind) movingSide=!movingSide;
                    
                    if (tries >= maxTries) {
                        M_Bounce= false;
                    }else {tries++;}
                    
                }
            }
        } else {
            anim.Play("Jump", 0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (M_Wing) {
           rb.isKinematic = true;
        }
        Grounded = Physics2D.Raycast(this.transform.position, Vector2.down, groundRayDistance, floorLayer);
        if (PlayerPhysics2D_Demo.instance != null) playerIsBehind = (PlayerPhysics2D_Demo.instance.transform.position.x > this.transform.position.x && !movingSide)||(PlayerPhysics2D_Demo.instance.transform.position.x < this.transform.position.x && movingSide);
        if (!M_Wing) {
            if (M_Red) {RedMechanic();}
            if (M_Bounce) {BounceMechanic();}
          
            if (mainThreadHalt) return;

            if (!data.type.Contains("V_NOFLIP")) TSprite.localScale = new Vector3(this.movingSide ? originalSX : -originalSX, TSprite.localScale.y, TSprite.localScale.z);
            if(movingSide && right.colliding) ChangeSide();
            if(!movingSide && left.colliding) ChangeSide();
        } 
		
    }
}
