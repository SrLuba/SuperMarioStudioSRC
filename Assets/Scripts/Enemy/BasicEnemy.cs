using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public GameObject KickedEnemy;
    public Sprite selfSprite;

    public GameObject Parent;

    public AudioClip killSound;

    public void OnTriggerEnter2D(Collider2D col) {
        if (col.tag!="Toss")return;
        SoundManager.instance.Play(5201, killSound, 1f, 1f);
        Instantiate(KickedEnemy, this.transform.position, Quaternion.identity).GetComponent<KickedObject>().sprite = selfSprite;
        ScoreController.instance.GetPoints(true, true, 1, this.transform.position);
        col.gameObject.GetComponent<Toss>().End();


        if (Parent != null) Destroy(Parent);
        Destroy(this.gameObject);
    }
}
