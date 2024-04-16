using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LevelEditor : MonoBehaviour
{
	public static LevelEditor instance;

	public int mode;





	public Transform playerSpawn;
	public Text layerText;
	public InputField levelNameIF;
	public Text modeText;

	public int tilesetID;
	public int rowLength;
	public string levelName, levelAuthor, levelDesc;
	public Transform grid, camera;
	public Vector2 cameraSpeed, cameraSpeedQuick;
	[HideInInspector]public LevelData test;
	[HideInInspector]public ResourcePack tileset;
	public AudioClip placeAC, saveAC;
	public bool CanDraw;
	public GameObject tileList;
	int btid = 0;
	public int cLayer = 0;

	Transform cursor;
	SpriteRenderer cursorRend;
	public void Awake() {
		if (instance == null)
			instance = this;
	}
	public void Start() {
		
		tileset = LE_Res.style;
		test = new LevelData (levelNameIF.text, levelDesc, levelAuthor);
		cursor = new GameObject ("LevelEditor_Cursor").transform;
		cursorRend = cursor.gameObject.AddComponent<SpriteRenderer> ();
		cursorRend.sortingLayerName = "L1";
		cursorRend.color = new Color (0.7f, 0.7f, 0.7f, 0.6f);
	}
	public void EraseTile(Vector2 position){
		if (!test.objs.Exists (x => x.position.x == position.x && x.position.y == position.y && x.layer == cLayer))
			return;

		LevelObj obj = test.objs.Find (x => x.position.x == position.x && x.position.y == position.y);

		Destroy (GameObject.Find (obj.name));

		test.objs.Remove (obj);
	}
	public void PrintTile(Vector2 position){
		if (position.x < 0f || position.y < 0f)
			return;
		if (test.objs.Exists (x => x.position.x == position.x && x.position.y == position.y  && x.layer == cLayer))
			return;
		
		SoundManager.instance.Play (placeAC);
		string name = "Tile_" + Random.Range (0, 9999).ToString();
		GameObject g = new GameObject( name);
		g.transform.position = (Vector3)position;
		g.AddComponent<SpriteRenderer>().sprite = cursorRend.sprite = LE_Res.getSprite("Sprites/Tileset_"+tilesetID.ToString(), LE_Res.style);
		g.GetComponent<SpriteRenderer> ().sortingLayerName = "L1";
		g.GetComponent<SpriteRenderer> ().sortingOrder = cLayer;
		test.objs.Add(new LevelObj(cLayer,name,LevelObjType.T_Backdrop,position,0f,new Vector2(1f,1f),"Sprites/Tileset_"+tilesetID.ToString(),true));

	}
	public void Update() {
		modeText.text = "Mode : " + mode.ToString ();
		layerText.text = "Layer : " + cLayer.ToString ();

		if (Input.GetKey (KeyCode.LeftShift) && Input.GetKeyDown (KeyCode.S)) {
			test.playerPosition = new SerializedVector2 (playerSpawn.position.x, playerSpawn.position.y);
			test.Save (levelNameIF.text);
		}

		if (Keyboard.current.fKey.isPressed) {
			Vector2 dir = Utils.getdir (Keyboard.current.dKey.isPressed, Keyboard.current.aKey.isPressed, Keyboard.current.wKey.isPressed, Keyboard.current.sKey.isPressed);
			Vector2 vel = (Keyboard.current.leftCtrlKey.isPressed ? cameraSpeedQuick : cameraSpeed);
			camera.position += new Vector3 (dir.x * vel.x * Time.deltaTime, dir.y * vel.y * Time.deltaTime, 0f);
		} else {
			if (Keyboard.current.dKey.wasPressedThisFrame) {
				tilesetID++;
			}
			if (Keyboard.current.aKey.wasPressedThisFrame) {
				tilesetID--;
			}
			if (Keyboard.current.sKey.wasPressedThisFrame) {
				tilesetID=0;
			}
			if (Keyboard.current.tabKey.wasPressedThisFrame) {
				CanDraw = !CanDraw;
			}
			if (Keyboard.current.qKey.wasPressedThisFrame) {
				cLayer++;
			}
			if (Keyboard.current.eKey.wasPressedThisFrame) {
				cLayer--;
			}
		}	


		cursorRend.sortingOrder = cLayer;

		tileList.SetActive (!CanDraw);
		camera.position = new Vector3 (Mathf.Clamp (camera.position.x, 0, 255000), Mathf.Clamp (camera.position.y, 0, 255000), camera.position.z);

		Vector2 mouseSnapped = Utils.SnapToGrid (Utils.mousePosition (camera.gameObject.GetComponent<Camera> ()), new Vector2 (16f, 16f));
		cursor.position = new Vector3 (mouseSnapped.x, mouseSnapped.y);
		if (btid != tilesetID) {
			cursorRend.sprite = LE_Res.getSprite("Sprites/Tileset_"+tilesetID.ToString(), LE_Res.style);
			btid = tilesetID;
		}
		if (Input.GetMouseButton (0) && CanDraw) 
			PrintTile (mouseSnapped);
		
		if (Input.GetMouseButton (1) && CanDraw)
			EraseTile (mouseSnapped);

		if (Input.GetKeyDown (KeyCode.P) && !test.objs.Exists (x => x.position.x == mouseSnapped.x && x.position.y == mouseSnapped.y))
			playerSpawn.position = (Vector3)mouseSnapped;
	}
}
