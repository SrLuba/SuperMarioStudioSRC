using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]public class V_Tile {
	public SerializedVector2 Position;
	public int tilesetID;
	public string GraphicReference;
	public string texturePackName;

	public V_Tile(int tilesetID, SerializedVector2 Position, string GraphicReference, string texturePackName) {
		this.Position = Position;
		this.GraphicReference = GraphicReference;
		this.texturePackName = texturePackName;
		this.tilesetID = tilesetID;
	}
}
[System.Serializable]public class V_TileGroup {
	public string displayName;
	public SerializedVector2 namePosition;
	public List<V_Tile> tiles;
	public V_TileGroup(string displayName,SerializedVector2 namePosition, List <V_Tile> tiles) {
		this.displayName = displayName;
		this.namePosition = namePosition;
		this.tiles = tiles;
	}
}
public class TilesetDef : MonoBehaviour {
	public Transform canvas;
	public List<V_TileGroup> tileGroup;
	public Font defaultFont;

	List<GameObject> gbs = new List<GameObject> ();

	public void Start() {
		Generate ();
	}

	public void Generate() {
		if (gbs.Count > 0) {
			for (int i = 0; i < gbs.Count; i++) {
				Destroy (gbs [i]);
			}
			gbs.Clear ();
		}
		gbs = new List<GameObject> ();

		for (int i = 0; i < tileGroup.Count; i++) {
			GameObject txt = new GameObject (tileGroup[i].displayName+"_Text_" + i.ToString ());
			Text Ttxt = txt.AddComponent<Text> ();
			Ttxt.text = tileGroup [i].displayName;
			Ttxt.fontSize = 16;
			Ttxt.font = defaultFont;

			txt.transform.SetParent (canvas);
			txt.transform.position = new Vector3 (0f, 0f, 0f);
			txt.transform.localPosition = (Vector3)new Vector2(tileGroup [i].namePosition.x, tileGroup [i].namePosition.y);
			for (int t = 0; t < tileGroup [i].tiles.Count; t++) {
				V_Tile tileRef = tileGroup [i].tiles[t];
				GameObject tile = new GameObject ("Tile_" + t.ToString ());
				tile.transform.SetParent (canvas);
				tile.transform.position = new Vector3 (0f, 0f, 0f);
				tile.transform.localPosition = new Vector3(tileRef.Position.x,tileRef.Position.y,0f);
				tile.AddComponent<Image> ().sprite = LE_Res.getSprite (tileRef.GraphicReference, LE_Res.style);
				tile.GetComponent<Image> ().SetNativeSize ();
				tile.transform.localScale = new Vector3 (0.025f, 0.025f, 0.025f);
				Button btn = tile.AddComponent<Button> ();
				btn.targetGraphic = tile.GetComponent<Image> ();
				int id = tileRef.tilesetID;
				btn.onClick.AddListener (delegate {
					LevelEditor.instance.tilesetID=id;
				});
				gbs.Add (tile);
			}
			gbs.Add (txt);
		}	
	}
}
