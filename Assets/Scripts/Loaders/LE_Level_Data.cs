using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]public enum LevelColType {
	T_Box,
	T_Circle,
	T_Polygon,
	T_Trigger
}
[System.Serializable]public class LevelCol {
	public LevelColType type;
	public SerializedVector2 position;
	public SerializedVector2 size = new SerializedVector2(1f,1f);
	public SerializedVector2 offset;
	public SerializedVector2 worldSize;
	public float angle;
	public string flags;

	public LevelCol(LevelColType type, Vector2 position, Vector2 size, Vector2 offset, Vector2 worldSize, float angle, string flags) {
		this.type=type;
		this.position=Utils.v2tosv2(position);
		this.size=Utils.v2tosv2(size);
		this.offset=Utils.v2tosv2(offset);
		this.worldSize=Utils.v2tosv2(worldSize);
		this.angle=angle;
		this.flags=flags;
	}

	public void Instantiate(Transform parent,int id){
		GameObject gb = new GameObject("Col_"+id.ToString());
		gb.transform.position = Utils.sv2tov2(position);
		gb.transform.localScale = Utils.sv2tov2(worldSize);
		gb.transform.eulerAngles = new Vector3(0f,0f,this.angle);

		if (type==LevelColType.T_Box) {
			BoxCollider2D col = gb.AddComponent<BoxCollider2D>();
			col.size = Utils.sv2tov2(size);
				col.offset = Utils.sv2tov2(offset);
		}else if (type==LevelColType.T_Circle) {
			CircleCollider2D col = gb.AddComponent<CircleCollider2D>();
			col.radius = size.x;
			col.offset = Utils.sv2tov2(offset);
		}

		gb.transform.SetParent(parent);
	}
}
[System.Serializable]public enum LevelObjType{
	T_Backdrop,
	T_Prefab,
	T_Player,
	T_Enemy
}
[System.Serializable]public class LevelObj {
	public string name;
	public LevelObjType type;
	public SerializedVector2 position;
	public float angle;
	public SerializedVector2 scale  = new SerializedVector2(1f,1f);
	public string flags;
	public bool solid;
	public int layer;

	public LevelObj(int layer, string name, LevelObjType type, Vector2 pos, float rot, Vector2 scale, string flags, bool solid) {
		this.name=name;
		this.type=type;
		this.position=Utils.v2tosv2(pos);
		this.angle=rot;
		this.scale=Utils.v2tosv2(scale);
		this.flags=flags;
		this.solid=solid;
		this.layer = layer;
	}
	public void Instantiate(Transform parent, int id){
		GameObject gb = new GameObject("Level_"+this.name+"_"+id.ToString());

		gb.transform.position = Utils.sv2tov2(this.position);
		gb.transform.eulerAngles = new Vector3(0f,0f,this.angle);
		gb.transform.localScale = Utils.sv2tov2(this.scale);

		if (this.type==LevelObjType.T_Backdrop) {
			SpriteRenderer rend = gb.AddComponent<SpriteRenderer>();
			rend.sprite = LE_Res.getSprite (flags, LE_Res.style);
			rend.sortingLayerName = "L1";
			rend.sortingOrder = layer;
		}else if (this.type==LevelObjType.T_Prefab) {
			GameObject p = MonoBehaviour.Instantiate(Resources.Load<GameObject>(flags), (Vector3)Utils.sv2tov2(this.position), Quaternion.identity);
			p.transform.SetParent(gb.transform);
		}


		if (this.solid) {
			BoxCollider2D col = gb.AddComponent<BoxCollider2D>();
			col.size = new Vector2(16f,16f);
			col.offset = new Vector2(0f,0f);
			gb.layer = LayerMask.NameToLayer ("Solid");
		}

		gb.transform.SetParent(parent);
	}
}
[System.Serializable]public class LevelBGData {
	public SerializedVector2 position;
	public SerializedVector2 parallax;
	public int sortingOrder;

	public LevelBGData(Vector2 position, Vector2 parallax, int so) {
		this.position=new SerializedVector2(position.x, position.y);
		this.parallax=new SerializedVector2(parallax.x, parallax.y);
		this.sortingOrder=so;
	}


}
[System.Serializable]public class LevelBG{
	public List<LevelBGData> data;

	public LevelBG(List<LevelBGData> data) {
		this.data = data;
	}
}
[System.Serializable]public class LevelData {
	public string levelName;
	public string levelDescription;
	public string levelAuthor;

	public SerializedVector2 playerPosition;

	public LevelBG bg;
	public List<LevelCol> cols;
	public List<LevelObj> objs;

	public LevelData(string levelName, string levelDescription, string levelAuthor){
		this.levelName=levelName;
		this.levelDescription=levelDescription;
		this.levelAuthor=levelAuthor;

		this.bg = new LevelBG(new List<LevelBGData>());
		this.cols = new List<LevelCol>();
		this.objs = new List<LevelObj>();
		this.playerPosition = new SerializedVector2 (0f, 0f);
	}

	public void Save(string path) {
		Directory.CreateDirectory(Application.persistentDataPath+"/Levels/");
		string rpath = Application.persistentDataPath+"/Levels/"+path+".lvl";
		string pngPath = Application.persistentDataPath+"/Levels/"+path+".png";
		Texture2D exportImage = TextureUtils.Advanced.JoinBatch ("Textures/Tileset", 16, 16, TextureUtils.Advanced.makeFromTiles (LE_Res.style, this));

		File.WriteAllBytes (pngPath, exportImage.EncodeToPNG ());
		using (Stream stream = File.Open(rpath, FileMode.Create))
		{
			var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			binaryFormatter.Serialize(stream, this);
		}
	}

	public void Load(string path) {
		Directory.CreateDirectory(Application.persistentDataPath+"/Levels/");
		string rpath = Application.persistentDataPath+"/Levels/"+path+".lvl";
		using (Stream stream = File.Open(rpath, FileMode.Open))
		{
			var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			LevelData rp = (LevelData)binaryFormatter.Deserialize(stream);

			this.levelName = rp.levelName;
			this.levelDescription = rp.levelDescription;
			this.levelAuthor = rp.levelAuthor;
			this.bg = rp.bg;
			this.cols = rp.cols;
			this.objs = rp.objs;
		}
	}

	public void Generate() {
		GameObject colsP = new GameObject(this.levelName+"_Cols");
		for (int i = 0; i < this.cols.Count; i++) {
			this.cols[i].Instantiate(colsP.transform, i);
		} 
		GameObject objsP = new GameObject(this.levelName+"_Objs");
		for (int i = 0; i < this.objs.Count; i++) {
			this.objs[i].Instantiate(objsP.transform, i);
		}
	}
}