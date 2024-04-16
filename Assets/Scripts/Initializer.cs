using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    public PowerUpDatabase pudb;
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1280, 720, true);
        Global.PowerUpDB=pudb;
    }
}
