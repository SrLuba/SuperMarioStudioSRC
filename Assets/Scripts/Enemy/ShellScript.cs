using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShellIntersectType {
    Top,
    Kick,
    Grab,
    Below,
    Drop,
    UpThrow
}

public class ShellScript : MonoBehaviour
{

    
    public SpriteRenderer selfPreview;
    public bool canGrab = false;

    public bool move;
    public float direction = 1f;
    public float constantSpeed = 75f;
    public Rigidbody2D rb;
    public Animator anim;

    public GameObject kickFX;
    public GameObject kickedObject;

    public Vector2 boundSize = new Vector2(8f, 8f);
    public Vector2 rayOffset = new Vector2(0f, 0f);

    float speed;

    RaycastHit2D rightRaySolid, leftRaySolid, groundRaySolid;

    bool rightRayIntersect, leftRayIntersect, groundRayIntersect;

    [Header("Solid Layer")] public LayerMask solidLayer; 

    [Header("Wall Hit SFX")] public AudioClip WHAC;
    [Header("Kick SFX")] public AudioClip KAC;

    public bool upThrow;


    public bool halt = false;
    public bool interactHalt = false;

    int selfTimer = 0;
    public void OnTriggerStay2D(Collider2D col) {
        if (col.tag == "Enemy") {
            if (upThrow) {
                col.gameObject.GetComponent<EnemyScript>().Die(1);
                this.Die();
            }
            if (move) col.gameObject.GetComponent<EnemyScript>().Die(1);
        }else if (col.tag == "Coin") {
            col.gameObject.GetComponent<CoinScript>().Grab();
        }else if (col.tag == "Toss") {
            this.Die();
            col.gameObject.GetComponent<Toss>().End();
            SoundManager.instance.Play(7248, WHAC, 1f, 1f);
        }else if (col.tag == "Player") {
            if (PlayerPhysics2D_Demo.instance.groundPoundAttack) {Die(); return;}
            if (col.gameObject.transform.position.y > this.transform.position.y) return;
            if (upThrow) return;

            if (move) {
            if(Global.PlayerState.One.vulnerable && PlayerPhysics2D_Demo.instance.rb.velocity.y >=0f){
                    PlayerPhysics2D_Demo.instance.Hit(this.transform.position.x);
                    direction=direction*-1f;
                    SoundManager.instance.Play(7248, KAC, 1f, 1f);
                    return;
                }else{return;}
            }

            move = true;
            SoundManager.instance.Play(7248, KAC, 1f, 1f);

        }else if (col.tag=="Death" || col.tag=="Hurt") {Die();}
    }

    public void Die() {
        Instantiate(kickFX, this.transform.position, Quaternion.identity);
        SoundManager.instance.Play(7248, KAC, 1f, 1f);
        Instantiate(kickedObject, this.transform.position, Quaternion.identity).GetComponent<KickedObject>().sprite = selfPreview.sprite;
        ScoreController.instance.GetPoints(true, true, 1, this.transform.position);
        Destroy(this.gameObject);
    }

    public void UpdateRays() {
        rightRaySolid = Physics2D.Raycast((Vector2)this.transform.position+rayOffset, Vector2.right, boundSize.x+1f, solidLayer);
        leftRaySolid = Physics2D.Raycast((Vector2)this.transform.position+rayOffset, Vector2.left, boundSize.x+1f, solidLayer);
        groundRaySolid = Physics2D.Raycast((Vector2)this.transform.position+rayOffset, Vector2.down, boundSize.y+1f, solidLayer);
        rightRayIntersect = rightRaySolid.collider!=null;
        leftRayIntersect = leftRaySolid.collider!=null;
        groundRayIntersect = groundRaySolid.collider!=null;

        if (rightRayIntersect) {
            if(interactHalt)return;

            if (rightRaySolid.collider.gameObject.tag=="Brick") {
             rightRaySolid.collider.gameObject.GetComponent<BrickScript>().Shatter();
            } 

            interactHalt=true;
            direction = -1f;
            SoundManager.instance.Play(7248, WHAC, 1f, 1f);
        }
        if (leftRayIntersect) {
            if(interactHalt)return;

       

            if (leftRaySolid.collider.gameObject.tag=="Brick") {
                leftRaySolid.collider.gameObject.GetComponent<BrickScript>().Shatter();
            }    
            interactHalt=true;
            direction = 1f;
            SoundManager.instance.Play(7248, WHAC, 1f, 1f);
        }
        if (groundRayIntersect) upThrow=false;
    }

    public void HandleMovement() {
        speed = direction*constantSpeed;

        rb.velocity = new Vector2((move) ? speed : 0f,rb.velocity.y);
        anim.speed = (move) ? 1f : 0f;
    }
    public void Interact(ShellIntersectType arg, Rigidbody2D sourceRb, Vector2 sourcePosition, bool sourceFacingRight) {

        if (interactHalt) return;

        if (arg == ShellIntersectType.Top) {
            // TopTouch or Stomp
            if (move) {
                move = false;
                SoundManager.instance.Play(7248, KAC, 1f, 1f);
                if (sourceRb!=null) sourceRb.velocity = new Vector2(sourceRb.velocity.x, 255f);
                return;
            }
            if (sourceRb!=null) sourceRb.velocity = new Vector2(sourceRb.velocity.x, 255f);
            SoundManager.instance.Play(7248, KAC, 1f, 1f);
            move = true;
            direction = (sourceFacingRight) ? 1f : -1f;
            Instantiate(kickFX, this.transform.position+new Vector3(0f,7f,0f), Quaternion.identity);
            ScoreController.instance.GetPoints(true, true, 1, this.transform.position);    
        }else if (arg == ShellIntersectType.Kick) {
            // SideTouch Or Kick
            Instantiate(kickFX, this.transform.position+ (Vector3) (new Vector3(sourceFacingRight ? -7f : 7f,0f,0f)), Quaternion.identity);
            SoundManager.instance.Play(7248, KAC, 1f, 1f);
            move = true;
            direction = (sourceFacingRight) ? 1f : -1f;
            rb.velocity = new Vector2 ((sourceFacingRight) ? 100f : -100f, 0f);
            upThrow = false;
            ScoreController.instance.GetPoints(true, false, 1, this.transform.position);
        }else if (arg == ShellIntersectType.Drop) {
            move = false;
            selfTimer=-2;
            direction = (sourceFacingRight) ? 1f : -1f;
        }else if (arg == ShellIntersectType.UpThrow) {
            Instantiate(kickFX, this.transform.position+ (Vector3) (new Vector3(sourceFacingRight ? -7f : 7f,0f,0f)), Quaternion.identity);
            move = false;
            rb.velocity = new Vector2 (0f, 300f);
            SoundManager.instance.Play(7248, KAC, 1f, 1f);
            upThrow = true;
            ScoreController.instance.GetPoints(true, false, 1, this.transform.position);
        }

        interactHalt = true;
        
    }

    public void HandleTimer() {
        if (!interactHalt)  return;
        selfTimer+=1;
        if (selfTimer > 2) {
            interactHalt=false;
            selfTimer=0;
        }
    }

    public void Prepare() {
        halt = false;
        interactHalt = true;
    }


    void Start()
    {
        Prepare();
    }

    void Update()
    {
        HandleTimer();
        UpdateRays();
    }

    void FixedUpdate() {
        HandleMovement();
        
    }
}
