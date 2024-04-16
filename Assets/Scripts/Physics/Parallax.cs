using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]public class ParallaxStrip {
    public Sprite sprite;
    public Color color = new Color(1f, 1f, 1f, 1f);
    public bool automaticSet = false;
    public GameObject ifPrefab;
    
	public Vector2 parallaxForce;
	public Vector2 startPos;
	public float height;
	public int tileRepeatCount = 32;
    public float sepFactor = 4f;

    public int layerID;
    public string layer;
    public float VX, VY;
	
    [HideInInspector] public float length = 16f;
    [HideInInspector] public Vector3 targetPos = new Vector3(0f,0f,0f);
    [HideInInspector] public Transform target;

	public GameObject Spawn(Transform camera, Vector2 origin, int layerOrigin, int id, Transform parent) {
		GameObject result = (ifPrefab == null) ? new GameObject("Strip_" + id.ToString()) : MonoBehaviour.Instantiate(ifPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
		SpriteRenderer temp = (ifPrefab == null) ? result.AddComponent<SpriteRenderer>() : result.GetComponent<SpriteRenderer>();

		this.length = (int)sprite.rect.width*2;
		this.height = (this.height == 0) ? (int)sprite.rect.height : this.height;
		temp.sprite = sprite;
		temp.drawMode = SpriteDrawMode.Tiled;
		temp.size = new Vector2(length * tileRepeatCount, height);
		temp.sortingLayerName = layer;
		temp.sortingOrder = (int)((layerID < 0) ? (layerOrigin * 128) + id : layerID);
		temp.color = color;
		result.transform.SetParent(parent);

		this.target = result.transform;
		this.target.position = new Vector2(0f, camera.position.y);

		if (automaticSet) this.startPos =new Vector2(origin.x, (origin.y - (sepFactor * id)));
        this.target.localPosition = (Vector3)this.startPos;
		this.targetPos = (Vector3)this.startPos;
		
		
		return result;

    }
	public void Update(Transform camera) {

		if (this.targetPos.x > camera.position.x+this.length) {
			this.startPos -= new Vector2(this.length, 0f);
		}
        if (this.targetPos.x < camera.position.x - this.length)
        {
            this.startPos += new Vector2(this.length, 0f);
        }
    }
}

[System.Serializable] public class ParallaxGroup {
	public bool setupAuto = false;
	public float parallaxForceFactorX = 0.01f;
    public float parallaxForceFactorY = 0.01f;
	public Vector2 origin;
    public List<ParallaxStrip> targets;

	public void Setup() {
		if (!this.setupAuto) return;
	
		for (int a = 0; a < targets.Count; a++)
		{
			targets[a].parallaxForce = new Vector2(a == 0 ? parallaxForceFactorX  : parallaxForceFactorX * (a + 1), a == 0 ? parallaxForceFactorY : parallaxForceFactorY * (a + 1));
			targets[a].startPos = new Vector2(origin.x, origin.y);
		}
    }
}
public class Parallax : MonoBehaviour {
	
	public static Parallax instance;
	public List<ParallaxGroup> targets;
	public BGData defaultBG;
	public Camera camera;
	[HideInInspector]public BGData currentBG;
	List<GameObject> objectBuffer;
	float y = 0f;
	bool loaded = false;
	void Awake(){
		instance=this;
		objectBuffer = new List<GameObject>();
		if (currentBG == null) currentBG = defaultBG;
		ReloadBG();
	}
	public void Start(){
		if(Global.Game.SaveData.bgData!=null) LoadBG(Global.Game.SaveData.bgData);
	}
	[ContextMenu("Reload BG")] public void ReloadBG(){
		LoadBG(currentBG);
	}
	public void LoadBG(BGData data) {
		currentBG = data;
		if (loaded) {
			// We do a switch
			
			// first clear
			Debug.Log("[BG] SWITCH ATTEMPT");
			targets = null;
			
			for (int i = 0; i < objectBuffer.Count; i++) {
				Destroy(objectBuffer[i]);
			}
			
			Debug.Log("[BG] Destroyed object buffer");
			objectBuffer.Clear();
			Debug.Log("[BG] Cleared object buffer");
			Debug.Log("<color=green>[BG] SWITCH PREPARATION COMPLETE - PLEASE MAKE SURE THIS IS A TRANSITION</color>");
			loaded = false;
		}
		
		
		
		Debug.Log("[BG] CREATE ATTEMPT");
		targets = currentBG.targets;
		Debug.Log("[BG] TARGETS ASSIGNED");
		
		Debug.Log("<color=yellow>[BG] ATTEMPING CREATE GBs (FOR LOOP INCOMING)</color>");
		for (int a = 0; a < targets.Count; a++) {
			targets[a].Setup();
			for (int i = 0; i < targets[a].targets.Count; i++)
			{
				objectBuffer.Add(targets[a].targets[i].Spawn(this.camera.transform, targets[a].origin,a, i, this.transform));
                targets[a].targets[i].targetPos = targets[a].targets[i].target.localPosition;

				SpriteRenderer rend = targets[a].targets[i].target.GetComponent<SpriteRenderer>();
			}
        }

		camera.backgroundColor = data.color;
		Debug.Log("<color=green>[BG] SUCCESS CREATING BACKGROUND</color>");
		y = this.camera.transform.position.y;
		loaded = true;
	}
	void LateUpdate () {
		if (camera==null) camera=Camera.main;
		if (camera==null) this.enabled=false;
		if (!loaded) return;
		for (int a = 0; a < targets.Count; a++)
		{
			for (int i = 0; i < targets[a].targets.Count; i++)
			{
				Vector2 distance = new Vector2((camera.transform.position.x * targets[a].targets[i].parallaxForce.x), ((camera.transform.position.y-SceneController.instance.generalY) * targets[a].targets[i].parallaxForce.y));
                targets[a].targets[i].targetPos =  new Vector3(0f, SceneController.instance.generalY, 0f) + new Vector3(targets[a].targets[i].startPos.x - distance.x, targets[a].targets[i].startPos.y - distance.y, targets[a].targets[i].target.localPosition.z);
				targets[a].targets[i].target.localPosition = targets[a].targets[i].targetPos;
				targets[a].targets[i].Update(camera.transform);
			}
		}
	}
}
