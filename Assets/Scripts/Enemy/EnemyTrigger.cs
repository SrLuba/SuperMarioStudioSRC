using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public EnemyScript selfEnemy;
    public bool colliding = false;
    public string tagTarget = "Solid";

    public void OnTriggerEnter2D(Collider2D col) {
        if (col.tag!=tagTarget && col.tag!="Enemy") return;
        if(col.tag=="Enemy") {
            EnemyScript en = col.gameObject.GetComponent<EnemyScript>();
            if (en!=null) { if (en.movingSide==selfEnemy.movingSide) return;}
        }
        colliding=true;
    }
    public void OnTriggerExit2D(Collider2D col) {
        if (col.tag!=tagTarget && col.tag!="Enemy") return;
        colliding=false;
    }
}
