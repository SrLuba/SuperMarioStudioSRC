using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuScript : MonoBehaviour
{

    public GameObject FadeOBJ;
    public AudioClip startAC;
    public bool loadInstant = false;

    public Transform parent;
    public string targetScene;
    public AudioSource music;
    [Header("Optional")]public AudioClip startACOptional;
    [Header("Optional")]public Animator anim;

    bool w=true;
    public IEnumerator StartS() {
        w=false;
        if(anim!=null)anim.Play("Start",0,0f);

        Global.PlayerState.One.lives=5;
        SoundManager.instance.Play(9929265,startAC,1f,1f);
        if(startACOptional!=null) SoundManager.instance.Play(9257265,startACOptional,1f,1f);
        Instantiate(FadeOBJ, parent.position+new Vector3(0f,0f,25f), Quaternion.identity).transform.SetParent(parent);
        while(music.volume > 0f) {
            music.volume-=5f*Time.deltaTime;
            yield return new WaitForSeconds(0.001f);
        }
        music.Stop();

        yield return new WaitForSeconds(4f);
        if (loadInstant){
            Global.targetScene=targetScene;
            Global.LoadTargetScene();
        }
        else {Global.LoadScene(targetScene);}

    }
    public void StartSRequest() {
        StartCoroutine(StartS());
    }
    void Update()
    {
        if (w&&Touchscreen.current.primaryTouch.press.isPressed) StartCoroutine(StartS());
    }
}
