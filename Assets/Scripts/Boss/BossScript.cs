using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public int hits = 5;
    public BasicTrigger t_l, t_r, t_mr, t_ml, t_m;
    public Animator anim;

	public AudioClip phase2, phase1;
    public AudioClip HitAC;
    public AudioSource Asrc;

    public float waitthink1, waitthink2;
    bool dead = false;
    public List<string> actions;

    public string baseAnimation = "Idle";
    public float time = 2f;

    float timer = 0f;
    public string resultstr = "";
    public string resultanim = "";

    int hitsFire = 0;
    public void OnTriggerEnter2D(Collider2D col) {
        if (dead)return;    
        if (col.tag!="Toss") return;
        col.gameObject.GetComponent<Toss>().End();
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(resultanim+"_Hitable") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Hitable")) return;
            


        
        
        hitsFire++;
        SoundManager.instance.Play(9,HitAC,1f,1f);
        if (hitsFire>=5) {
            Hit();
            hitsFire=0;
        }
    }
    public bool Hit() {
        if (dead)return false;        
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(resultanim+"_Hitable") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Hitable")) return false;
        hits--;
        if (hits>=6) 
        {
            anim.Play("Hit", 0, 0f);
        }else if (hits<6 && hits>0) 
        {
            anim.Play("DeadBait", 0, 0f);
         
        }else if (hits<=0) {
            
            Asrc.Stop();
            anim.Play("Dead", 0, 0f);
        }
        return true;
    }
    public void DoAction(int id)  {
        hitsFire=0;
        if (t_m.colliding) {resultstr="M";}
        else if (t_mr.colliding) {resultstr="MR";}
        else if (t_r.colliding) {resultstr="R";}
        else if (t_ml.colliding) {resultstr="ML";}
        else if (t_l.colliding) {resultstr="L";}
        else {resultstr="M";}
        resultanim = actions[id]+"_"+resultstr;
        anim.Play(resultanim, 0, 0f);
    }
    public void DoRandomAction() {
         hitsFire=0;
        
        if (t_m.colliding) {resultstr="M";}
        else if (t_mr.colliding) {resultstr="MR";}
        else if (t_r.colliding) {resultstr="R";}
        else if (t_ml.colliding) {resultstr="ML";}
        else if (t_l.colliding) {resultstr="L";}
        else {resultstr="M";}
        resultanim = actions[Random.Range(0,actions.Count-1)]+"_"+resultstr;
        anim.Play(resultanim, 0, 0f);
    }
    int phase=0;
    public IEnumerator changePhase() {
        Asrc.Stop();
        yield return new WaitForSeconds(6f);
        Asrc.clip=phase2;
        Asrc.Play();
        phase=2;
    }
    public void Start() {
        Asrc.clip=phase1;
        Asrc.Play();
        hitsFire=0;
    }
    void Update()
    {
        anim.speed = (phase>1) ? 1.5f : 1f;
        time = (phase>1) ? waitthink2 : waitthink1;
        if (hits<6 && phase==0) {
            StartCoroutine(changePhase());
            phase=1;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(baseAnimation)) {
            timer+=Time.deltaTime;

            if(timer>=time) {
                DoRandomAction();
                timer=0f;
            }
        }else {
            timer=0f;
        }
        


    }
}
