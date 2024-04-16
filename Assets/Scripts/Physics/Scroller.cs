using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour {

	public Transform target;
	public Vector2 speed;

	void FixedUpdate () {
		target.position += (Vector3)speed * Time.deltaTime;
	}
}
