using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTrigger : MonoBehaviour
{
    public bool canCollide = true;
    public bool colliding = false;
    public string tagFlag = "";

    public void OnTriggerEnter2D(Collider2D col) {
        if (!canCollide){colliding=false; return;}
        if (tagFlag!="" && col.gameObject.tag!=tagFlag) return;

        colliding=true;
    }
    public void OnTriggerExit2D(Collider2D col) {
        if (!canCollide){colliding=false; return;}
        if (tagFlag!="" && col.gameObject.tag!=tagFlag) return;

        colliding=false;
    }
}
