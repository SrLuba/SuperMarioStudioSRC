using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayMenu : MonoBehaviour
{
    
    public Animator anim;

    public string inAN, outAN;
    public string ainAN, aoutAN;

    public bool halt = false;

    public void ShowRequest(float time) {StartCoroutine(Show(time));}
    public IEnumerator Show(float time) {
        halt = true;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(aoutAN)) {
            anim.Play(inAN,0);
        }
        yield return new WaitForSeconds(time);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(ainAN)) {
            anim.Play(outAN,0);
        }
        halt = false;
    }
    void Update()
    {
        //if (!anim.GetCurrentAnimatorStateInfo(0).IsName(ainAN) || !anim.GetCurrentAnimatorStateInfo(0).IsName(aoutAN)) return;
        if (!halt) {
            if (GlobalInput.select) {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName(aoutAN)) {
                    anim.Play(inAN,0);
                }
            }else {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName(ainAN)) {
                    anim.Play(outAN,0);
                }
            }
        }
    }
}
