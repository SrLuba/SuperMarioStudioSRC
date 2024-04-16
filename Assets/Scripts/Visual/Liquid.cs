using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public float magnitude, speed, factor;

    public GameObject mainSprite;

    public int columns = 10;

    public float columnWidth = 8f;
    public float sizeY = 128f;

    public Color color;

    List<Transform> objs = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < columns; i++) {
            GameObject dummy = Instantiate(mainSprite,this.transform.position+new Vector3(columnWidth*i, 0f, 0f), Quaternion.identity);
            dummy.GetComponent<SpriteRenderer>().color = color;
            dummy.transform.SetParent(this.transform);
            objs.Add(dummy.transform);
        }   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < columns; i++) {
            objs[i].position  = this.transform.position+new Vector3(columnWidth*i, Mathf.Sin(Time.time*speed+(factor*i))*magnitude, 0f);
        }
    }
}
