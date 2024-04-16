using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]public class DistanceINI {
    public string tag;
    public float distance;
}
public class FinalOC : MonoBehaviour
{
    public Transform target;
    public int updateRate;
    public List<DistanceINI> tags;

    public float distance = 512f;

    int rate;

    List<GameObject> gbs = new List<GameObject>();


    void Start()
    {
        for(int i = 0; i < tags.Count; i++) {
            GameObject[] gb = GameObject.FindGameObjectsWithTag(tags[i].tag);
            for (int a = 0; a < gb.Length; a++) {
                gbs.Add(gb[a]);
            }
        }   
    }
    public void UpdateOC() {
        for(int i = 0; i < gbs.Count; i++) {
            if (gbs[i]==null) { gbs.RemoveAt(i); return;} 
            DistanceINI I = tags.Find(x => x.tag == gbs[i].tag);
            if (I==null)return;
            if (gbs[i].tag=="Player") return;
            gbs[i].SetActive(Vector2.Distance(target.position, gbs[i].transform.position)<I.distance);
        }
    }

    void Update()
    {
        rate++;

        if (rate>updateRate) {
            UpdateOC();
            rate=0;
        }
    }
}
