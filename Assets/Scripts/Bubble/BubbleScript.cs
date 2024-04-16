using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    public Animator anim;

    public SpriteRenderer rend;
    public Sprite sprite;
    public bool contains;
    public GameObject coin;
    public Sprite coinSprite;
    GameObject container;

    bool enemy = false;
    public void SelfDestroy() {
        Destroy(this.gameObject);
    }
    public void DestroyBubble() {
        
        anim.Play("Destroy");

        if (!contains) return;
        container.transform.position = rend.transform.position;
        container.SetActive(true);
        container=null;
        contains=false;
    }
    public void DestroyContainer() {
        if (!contains) return;
        if (!enemy) return;
        if (container==null)return;

        Destroy(container);
        container=Instantiate(coin, rend.transform.position, Quaternion.identity);
        container.SetActive(false);
        sprite = coinSprite;
        enemy=false;
    }

    public void DestroyRequestContainer() {
        if (!contains) return;
        if (!enemy) return;
        anim.Play("Kill");
    }
    public void OnTriggerEnter2D(Collider2D col) {
        if (container!=null){contains=true; return;} 

        if (col.tag=="Enemy"){
            container = col.gameObject;
            sprite = col.gameObject.GetComponent<EnemyScript>().rend.sprite;
            col.gameObject.SetActive(false);
            contains=true;
            enemy=true;

        }else if (col.tag=="Player") {
            if (PlayerPhysics2D_Demo.instance.rb.velocity.y>0f) return;

            PlayerPhysics2D_Demo.instance.rb.velocity = new Vector2(PlayerPhysics2D_Demo.instance.rb.velocity.x, 150f);
            DestroyBubble();
        }
    }
    void Update()
    {
        rend.enabled = contains;
        rend.sprite = sprite;
    }
}
