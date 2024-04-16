using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StyleLoader : MonoBehaviour {

	public string path;
	public ResourcePack pack;


	// Use this for initialization
	void Awake () {
		LE_Res.LoadStyle (path);
		pack.Load (path);
	}
}
