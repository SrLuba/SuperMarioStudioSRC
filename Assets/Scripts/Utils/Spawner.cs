using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnTarget;
    public Animator anim;
    public Transform target;
    public Vector3 offset;
    public float time;
    public float interTime;
    public int count;

    float timer = 0f;

    public IEnumerator Spawn() {
        
        for (int i = 0; i < count; i++) {
            if (anim != null) anim.Play("Shoot", 0, 0f);
            Instantiate(spawnTarget, target.position+offset, Quaternion.identity);
            yield return new WaitForSeconds(interTime);
        }
        timer=0f;
    }
    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        if (timer>=time) {
            StartCoroutine(Spawn());
            timer=0f;
        }
    }
}
