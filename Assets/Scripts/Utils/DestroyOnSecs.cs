using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnSecs : MonoBehaviour
{
	public float timer = 1f;
	float ttimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ttimer+=Time.deltaTime;
		if (ttimer>=timer) {
			Destroy(this.gameObject);
		}
    }
}
