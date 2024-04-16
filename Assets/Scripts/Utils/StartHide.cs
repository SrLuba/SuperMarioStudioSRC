using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHide : MonoBehaviour
{
    public bool destroy = false;
    
    void Awake()
    {
        if (destroy) Destroy(this.gameObject);
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;   
    }
}
