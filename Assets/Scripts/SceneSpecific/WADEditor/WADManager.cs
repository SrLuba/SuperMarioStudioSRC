using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
[System.Serializable]public class WADSection {
	public string type;
	public GameObject gb;
}
public class WADManager : MonoBehaviour {
	public static WADManager instance;
	public static ResourcePack wad;

	public List<WADSection> sections;
	public int pageSize = 16;
	public int currentPage = 0;
	public string resourceItemPB;
	public Transform canvas;
	public Transform canvasPivot;
	public float padding;
	public Vector3 startOffset;
	public int idSelect = 0;
	public string currentPath = "";


	void Awake() {
		if (instance == null)
			instance = this;
		wad = new ResourcePack ();

		Load ();
		UpdateLumps ();
	}
	public void Load() {
		OpenFileName ofn = new OpenFileName ();
		ofn.structSize = Marshal.SizeOf (ofn);
		ofn.filter = "All Files\0*.*\0\0";
		ofn.file = new string(new char[256]);
		ofn.maxFile = ofn.file.Length;
		ofn.fileTitle = new string(new char[64]);
		ofn.maxFileTitle = ofn.fileTitle.Length;
		ofn.initialDir = UnityEngine.Application.dataPath;
		ofn.title = "Open WAD!";
		ofn.defExt = ".WAD";
		ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

		if (DllTest.GetOpenFileName (ofn)) {
			wad.Load (true, ofn.file);
			this.currentPath = ofn.file;
		}

		idSelect = 0;
		currentPage = 0;
	}
	public void Save() {
		wad.Save ("out");
	}
	public void Remove(int id) {
		wad.lumps.RemoveAt (id);
		UpdateLumps ();
	}
	public void Switch(int side, int id) {
		if (wad.lumps.Count - 1 < id + side)
			return;
		
		ResourcePack_Lump a = wad.lumps [id + side];
		ResourcePack_Lump b = wad.lumps [id];

		wad.lumps [id + side] = b;
		wad.lumps [id] = a;

		UpdateLumps ();
	}
	List<GameObject> gameObjects = new List<GameObject> ();
	public void LoadLumps(int start, int end) {
		if (wad.lumps.Count <= 0)
			return; 
		if (gameObjects.Count > 0) {
			for (int i = 0; i < gameObjects.Count; i++){
				Destroy (gameObjects [i]);
			}
		}
		gameObjects = new List<GameObject> ();
		while (end > wad.lumps.Count - 1)
			end--;

		int a = 0;
		for (int i = start; i < end; i++) {
			GameObject ins = Instantiate(Resources.Load<GameObject>(resourceItemPB), new Vector3(0f,0f,0f), Quaternion.identity);
			ins.GetComponent<WADItem>().id = i;
			ins.GetComponent<WADItem> ().UpdateIFs ();

			ins.transform.SetParent (canvasPivot);

			ins.transform.localScale = new Vector3 (1f, 1f, 1f);

			ins.transform.localPosition = startOffset - new Vector3 (0f, a* padding, 0f);
			gameObjects.Add (ins);
			a++;
		}
	}

	public void UpdateLumps () {
		LoadLumps (pageSize*currentPage, (pageSize*currentPage)+pageSize);
	}
	public void movePage(int units) {
		if (currentPage + units < 0)
			return;

		currentPage += units;

		LoadLumps (pageSize*currentPage, (pageSize*currentPage)+pageSize);
	}

	public void Edit(int id) {
		WADSection section = sections.Find (x => x.type == wad.lumps [id].lumpType);
		if (section == null)
			return;

		for (int i = 0; i < sections.Count; i++) {
			sections [i].gb.SetActive (false);
		}
		section.gb.SetActive (true);
	}

	public void Replace(int id) {
		if (wad.lumps [id].lumpType == "Sprite") {
			OpenFileName ofn = new OpenFileName ();
			ofn.structSize = Marshal.SizeOf (ofn);
			ofn.filter = "All Files\0*.*\0\0";
			ofn.file = new string(new char[256]);
			ofn.maxFile = ofn.file.Length;
			ofn.fileTitle = new string(new char[64]);
			ofn.maxFileTitle = ofn.fileTitle.Length;
			ofn.initialDir = UnityEngine.Application.dataPath;
			ofn.title = "Open SPR!";
			ofn.defExt = ".SPR";
			ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

			if (DllTest.GetOpenFileName (ofn)) {
				string[] data = System.IO.File.ReadAllLines (ofn.file);

				wad.lumps [id].objectData = data;
			}
		}
	}
	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
