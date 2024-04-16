using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BetterJump : MonoBehaviour
{
	public PlayerPhysicsData ph;
    public int type = 0;
    public Rigidbody2D rb;

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.y < 0) {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (ph.fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !(GlobalInput.a || GlobalInput.c)) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (ph.lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
