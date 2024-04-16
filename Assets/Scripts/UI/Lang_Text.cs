using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Lang_Text : MonoBehaviour {
	public Text text;
	public string group;
	public int id;
	// Use this for initialization
	void Start () {
		if (Lang.cClang == null) Lang.LoadLANG();
		text.text = Lang.getString(this.group, this.id);
	}
}
