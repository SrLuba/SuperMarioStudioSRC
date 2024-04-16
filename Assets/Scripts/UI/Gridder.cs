using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Gridder : MonoBehaviour
{
   
    void Update()
    {
        this.transform.position = new Vector3(Mathf.FloorToInt(this.transform.position.x * 16f) / 16f, Mathf.FloorToInt(this.transform.position.y * 16f) / 16f, Mathf.FloorToInt(this.transform.position.z * 16f) / 16f);
    }
}
