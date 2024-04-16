using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderScreen : MonoBehaviour
{

    public GameObject FadeOBJ;
    public Transform parent;
    public Animator anim;

    float wait=0f;
    bool w=true;

    void Start() {
        anim.Play("Enter_"+Global.transitionTypeGlobal.ToString(), 0, 0f);
    }

    public IEnumerator StartS() {
        anim.Play("Exit_"+Global.transitionTypeGlobal.ToString(),0,0f);
        yield return new WaitForSeconds(2.2f);
        Global.LoadTargetScene();
    }

    void Update()
    {
       wait+=Time.deltaTime;
       if(w&&wait>6f){
        StartCoroutine(StartS());
            w=false;
       }
    }
}
