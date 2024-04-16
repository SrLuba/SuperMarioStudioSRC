using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpriteTest : MonoBehaviour {
	public string sprite;
	public string resourcePack;
	public SpriteRenderer rend;

	// Use this for initialization
	void Start () {
		LE_Res.LoadStyle (resourcePack);
		rend.sprite = LE_Res.getSprite(sprite, LE_Res.style);
	}
}
