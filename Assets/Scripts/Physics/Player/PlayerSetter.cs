using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetter : MonoBehaviour
{
    public string character = "Mario";
    // Start is called before the first frame update
    void Awake()
    {
        Global.PlayerState.One.character = this.character;
    }
}
