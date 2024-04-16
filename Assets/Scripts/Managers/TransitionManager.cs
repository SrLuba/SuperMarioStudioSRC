using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    public List<GameObject> transitionsIn;
    public List<GameObject> transitionsOut;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    public void Transition(Transform pivot, bool type, int id)
    {
        GameObject g = Instantiate(type ? transitionsIn[id] : transitionsOut[id], pivot);
        g.transform.position = new Vector3(0f,0f,0f);
        g.transform.localPosition = new Vector3(0f, 0f, 10f);
    } 
}
