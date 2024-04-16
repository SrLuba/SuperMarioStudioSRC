using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public Sprite points;
    public Sprite pointsALLGRAND;
    public int value = 1;
    public int pointValue = 10;
    public bool grand;
    public GameObject FX;
    public AudioClip SFX;
    public bool ThirdD;
    public float ThirdSpeed;

    public Animator anim;

    public AudioClip GrabAllGrandSFX;
    bool grabbed = false;
    public void Grab() {
        if (grabbed) return;
        SoundManager.instance.Play(SFX);
        Instantiate(FX, this.transform.position, Quaternion.identity);
        Global.PlayerState.coins+=value;
        if (grand) { 
            SceneController.instance.GrandCoin();

            Global.PlayerState.grandCoins+=value; 
            if (Global.PlayerState.grandCoins >= 3) {
                ScoreController.instance.GetPointsExplicit(50000, null, pointsALLGRAND, this.transform.position);
                SoundManager.instance.Play(259188, GrabAllGrandSFX, 1f, 1f);
                Global.finalScore+=5;
            } else {
                ScoreController.instance.GetPointsExplicit(5000, null, points, this.transform.position);
                SoundManager.instance.Play(255187, SFX, 1f, 1f);
                Global.finalScore+=1;
            }
        }
        ScoreController.instance.GetPointsExplicit(pointValue, null, points, this.transform.position);
        if (!grand) {Destroy(this.gameObject);}else{anim.Play("Grab", 0, 0f);}
        grabbed=true;
    }
    private void FixedUpdate()
    {
        if(grand && Global.PlayerState.grandCoins>=3) Destroy(this.gameObject);
        if (ThirdD) {
            this.transform.Rotate(0f, ThirdSpeed*Time.deltaTime, 0f);
        }
    }
}
