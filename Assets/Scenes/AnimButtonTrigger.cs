using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimButtonTrigger : MonoBehaviour
{
    public Animator anim;
    public void Play(string animSTR) {
        anim.Play(animSTR);
    }
}
