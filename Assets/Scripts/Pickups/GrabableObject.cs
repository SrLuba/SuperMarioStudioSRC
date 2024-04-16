using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableObject : MonoBehaviour
{
    public SpriteRenderer selfRend;
    public float speed;
    public bool killEnemies;
    public Rigidbody2D rb;
    public bool stopped = true;
    public bool side = true;
    public Animator anim;
    public Vector2 rayOffset;
    public float rayDistance;
    public LayerMask walls;
    public Transform spriteT;
    public AudioClip bumpAC, kickAC;
    public GameObject kickFX;
    public Vector3 kickFXOffset;
    public float angleChangeFactor;
    float angle =0f;
    float angleTo = 0f;
    public bool upHalt = false;
    public bool revive;
    public GameObject reviveGB;
    public Vector3 reviveGBOffset;
    public float reviveTimer = 2f;

    public IEnumerator Revive() {
        anim.Play("Revive", 0);
        halt = true;
        stopped = true;
        yield return new WaitForSeconds(1f);
      
            Instantiate(reviveGB, this.transform.position + reviveGBOffset, Quaternion.identity);
            Destroy(this.gameObject);
      

    }
    public void Awake()
    {
        if (revive && reviveGB == null) revive = false;
    }
    public void Kick(float force, bool Gside, bool Up) {
        if (halt) return;
        if (Up)
        {

            rb.velocity = new Vector2(0f, force);
            stopped = true;
            Instantiate(kickFX, this.transform.position + kickFXOffset, Quaternion.identity);
            ScoreController.instance.GetPoints(true, false, 1, this.transform.position);
   
        }
        else
        {
            
            stopped = false;
            Instantiate(kickFX, this.transform.position + kickFXOffset, Quaternion.identity);
            ScoreController.instance.GetPoints(true, true, 1, this.transform.position);
        }
        if (GlobalInput.down)
        {
            rb.velocity = new Vector2(0f, 0f);
            stopped = true;
            return;
        }
     
        
        
        side = Gside;
        timerRevive = 0f;
        halt = true;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (killEnemies && !stopped) {
                collision.gameObject.GetComponent<EnemyScript>().Die(1);
                Instantiate(kickFX, this.transform.position + kickFXOffset, Quaternion.identity).transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            }
        }
        
    }
    public void Start() {halt = true;}
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Brick" && !stopped)
        {
            BrickScript brick = collision.GetComponent<BrickScript>();

            brick.Shatter();
            this.Flip();
        }
    }
    float timer = 0f;
    float timerRevive= 0f;
    private void FixedUpdate()
    {
        if (halt) { timer += Time.deltaTime; if (timer >= 1f) { halt = false; timer = 0f; } } else { timer = 0f; }
        spriteT.eulerAngles = new Vector3(0f, 0f, angle);
        angle = Mathf.LerpAngle(angle, angleTo, angleChangeFactor * Time.deltaTime);
        anim.Play(stopped ? "Idle" : "Move", 0);
        if (!stopped)
        {
            rb.velocity = new Vector2(side ? speed : -speed, rb.velocity.y);
            timerRevive = 0f;
        }
        else {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            timerRevive += Time.deltaTime;
            if (timerRevive > reviveTimer && revive) {
                StartCoroutine(Revive());
                revive = false;
            }
        }
    }
    public bool halt = true;
    public void Flip() {
        SoundManager.instance.Play(56, bumpAC, 1f, 1f);
        this.side = !this.side;
    }
    RaycastHit2D r, l, b;
    // Update is called once per frame
    void Update()
    {
        r = Physics2D.Raycast((Vector2)this.transform.position + rayOffset, Vector2.right, rayDistance, walls);
        l = Physics2D.Raycast((Vector2)this.transform.position + rayOffset, Vector2.left, rayDistance, walls);
        b = Physics2D.Raycast((Vector2)this.transform.position, Vector2.down, rayDistance+4f, walls);

        angleTo = (b.collider!=null) ? Vector2.Angle(b.normal, Vector2.up) : 0f;
        spriteT.localScale = new Vector3(side ? 1f : -1f, 1f, 1f);
        if (!stopped) {
            if (r.collider != null || l.collider != null) {
               this.Flip();
            }
        }
    }
}
