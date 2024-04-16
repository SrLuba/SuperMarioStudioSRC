using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {
	public string path;
	public string tilesetPath;
	public Transform Player;

	public void Start() {
		LE_Res.LoadStyle (tilesetPath);
		LevelData data = new LevelData ("", "", "");
		data.Load (path);
		data.Generate ();
		Player.position = (Vector3)Utils.sv2tov2(data.playerPosition);
	}
}
