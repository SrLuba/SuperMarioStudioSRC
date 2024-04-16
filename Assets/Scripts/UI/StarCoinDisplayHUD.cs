using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarCoinDisplayHUD : MonoBehaviour
{
    public Image a,b,c;

    public Sprite MIS_S, GOT_S;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        a.sprite = (Global.PlayerState.grandCoins >= 1) ? GOT_S : MIS_S; 
        b.sprite = (Global.PlayerState.grandCoins >= 2) ? GOT_S : MIS_S; 
        c.sprite = (Global.PlayerState.grandCoins >= 3) ? GOT_S : MIS_S; 
    }
}
