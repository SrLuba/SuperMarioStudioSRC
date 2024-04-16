using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalScoreShower : MonoBehaviour
{
    public SpriteRenderer scoreRenderer;
    public Sprite S,A,B,C,D,E,F;
    public GameObject Plus;
    public GameObject SMessage;
    public TMP_Text smessagetext;


    // Start is called before the first frame update
    void Start()
    {
        int finalScore = Global.finalScore;
        int score = Global.PlayerState.score;

        bool plus = (finalScore >= 9);

        Sprite resultScore = F;

        if (score > 0 && score <= 20000) {
            resultScore=F;
        } else if (score > 20000 && score <= 40000) {
            resultScore=E;
        } else if (score > 40000 && score <= 50000) {
            resultScore=D;
        } else if (score > 50000 && score <= 75000) {
            resultScore=C;
        } else if (score > 75000 && score <= 100000) {
            resultScore=B;
        } else if (score > 100000 && score <= 150000) {
            resultScore=A;
        } else if (score > 150000) {
            resultScore=S;
        } 

        Plus.SetActive(plus);
        if (plus && resultScore==S)
        { 
            SMessage.SetActive(true);
            smessagetext.text = ((Random.Range(0,3)) > 2 ? "B" : (Random.Range(0,3)) > 2 ? "C" : "A").ToString() +Random.Range(10,99).ToString()+"-"+Random.Range(10,99).ToString()+"-"+Random.Range(10,99).ToString() + ((Random.Range(0,3)) > 2 ? "B" : (Random.Range(0,3)) > 2 ? "C" : "A").ToString();
        }


        scoreRenderer.sprite = resultScore;
    }

}
