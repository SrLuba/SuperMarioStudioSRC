using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semisolid : MonoBehaviour
{

    public Collider2D targetCollider;
    public Transform player;
    public Rigidbody2D playerRB;
    bool en = false;
    public float yOffset = 0f;
    public void Awake()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (player == null) return;
        targetCollider.enabled = (player.position.y > this.transform.position.y + yOffset && !GlobalInput.down);
    }
}
