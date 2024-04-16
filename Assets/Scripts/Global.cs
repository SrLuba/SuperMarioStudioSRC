using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalInput {
	public static bool up,down,left,right,upp,downp,leftp,rightp,a,b,c,ap,bp,cp, startp, selectp, start, select, dp,d,e,ep,f,fp;
}
[System.Serializable]public class SerializedVector2{
		public float x,y;

		public SerializedVector2(float x, float y) {
			this.x = x;
			this.y = y;
		}
	}
	[System.Serializable]public class SerializedRect {
		public SerializedVector2 position;
		public SerializedVector2 size;

		public SerializedRect(SerializedVector2 position, SerializedVector2 size) {
			this.position = position;
			this.size = size;
		}
	}
	[System.Serializable]public class RP_Lump_Sprite {
		public string texturePath;
		public SerializedRect rect;
		public SerializedVector2 pivot;

		public RP_Lump_Sprite(string texturePath, SerializedRect rect, SerializedVector2 pivot) {
			this.texturePath = texturePath;
			this.rect = rect;
			this.pivot = pivot;
		}
	}
public static class LE_Res {
	public static ResourcePack style;
	public static ResourcePack general;
	/// <summary>
	/// Loads the style.
	/// </summary>
	/// <param name="path">Path.</param>
	public static void LoadStyle(string path) {
		style = new ResourcePack ();
		style.Load (path);
	}
	/// <summary>
	/// Loads the general.
	/// </summary>
	/// <param name="path">Path.</param>
	public static void LoadGeneral(string path) {
		general.Load (path);
	}
	/// <summary>
	/// Gets the sprite.
	/// </summary>
	/// <returns>The sprite.</returns>
	/// <param name="reference">resource name</param>
	/// <param name="pack">Resource Pack as Reference.</param>
	public static Sprite getSprite(string reference, ResourcePack pack) {
		ResourcePack_Lump sprite = pack.lumps.Find (x => x.lumpType == "Sprite" && x.lumpName==reference);

		if (sprite != null) {
			Debug.Log ("<color=yellow> Sprite Been Found In WAD </color>");

			ResourcePack_Lump tLump = pack.lumps.Find (x => x.lumpType == "Texture2D" && x.lumpName == sprite.objectData [0]);
			if (tLump == null) 
				return null;

			Debug.Log("<color=green> Source Texture Been Found In WAD </color>");
			Texture2D src = TextureUtils.bytesToTexture (tLump.lumpData);
			RP_Lump_Sprite spriteBinding = new RP_Lump_Sprite (sprite.objectData [0], new SerializedRect (new SerializedVector2 (int.Parse (sprite.objectData [1]), int.Parse (sprite.objectData [2])), new SerializedVector2 (int.Parse (sprite.objectData [3]), int.Parse (sprite.objectData [4]))), new SerializedVector2(0.5F,0.5F));
			Sprite spr = TextureUtils.texture2DSprite (src, spriteBinding);

			return spr;
		}
		return null;
	}

	public static Texture2D getTexture(string reference, ResourcePack pack) {
		ResourcePack_Lump texture = pack.lumps.Find (x => x.lumpType == "Texture2D" && x.lumpName==reference); 

		if (texture != null) {
			Debug.Log ("<color=yellow> Texture2D Been Found In WAD </color>");

			return TextureUtils.bytesToTexture (texture.lumpData);
		}
		return null;
	}
}
public static class TextureUtils 
{
	public static class Advanced
	{
		[System.Serializable]public class TUA_Join_Item {
			public string targetTexturePath;
			public int srcw, srch;
			public int srcx, srcy;
			public int dstx, dsty;

			public TUA_Join_Item(string targetTexturePath, int srcx, int srcy, int srcw, int srch, int dstx, int dsty) {
				this.targetTexturePath = targetTexturePath;
				this.srcx=srcx;
				this.srcy=srcy;
				this.srcw=srcw;
				this.srch=srch;
				this.dstx=dstx;
				this.dsty=dsty;
			}
		}
		public static Texture2D Join(Texture2D srcTex, Texture2D dstTex, TUA_Join_Item item) {
			Texture2D tex = dstTex;
				
			for (int x = 0; x < item.srcw; x++) {
				for (int y = 0; y < item.srch; y++) {
					tex.SetPixel (item.dstx+x,item.dsty+y,srcTex.GetPixel(item.srcx+x,item.srcy+y));
				}
			}

			tex.filterMode = FilterMode.Point;
			tex.Apply ();

			return tex;
		}

		public static Texture2D JoinBatch(string textureReference, int averageWidth, int averageHeight, List<TUA_Join_Item> items) {
			Texture2D tex = new Texture2D ((int)(averageWidth*(items.Count)), (int)(averageHeight*(items.Count)), TextureFormat.RGBA32, false);
			Texture2D mySRC = LE_Res.getTexture (textureReference, LE_Res.style);

			for (int x = 0; x < tex.width; x++) {
				for (int y = 0; y < tex.width; y++) {
					tex.SetPixel (x, y, new Color (0f, 0f, 0f, 0f));
				} 
			} 

			for (int x = 0; x < items.Count; x++) {
				tex = Join (mySRC, tex, items [x]);
			}

			tex.filterMode = FilterMode.Point;
			tex.Apply ();

			return tex;
		}
		public static List<TUA_Join_Item> makeFromTiles(ResourcePack pack, LevelData lvlData) {
			List<TUA_Join_Item> items = new List<TUA_Join_Item> ();

			for (int i = 0; i < lvlData.objs.Count; i++) {
				LevelObj obj = lvlData.objs [i];
				Sprite sprite = LE_Res.getSprite (obj.flags, LE_Res.style);
				Debug.Log ("sprite is " + (string)((sprite == null) ? "null" : "nulln't"));
				Debug.Log ("obj is " + (string)((obj == null) ? "null" : "nulln't"));
				if (sprite == null || obj == null)
					return items;
				TUA_Join_Item resultItem = new TUA_Join_Item (obj.flags, (int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height, (int)obj.position.x, (int)obj.position.y);
				items.Add (resultItem);
			}
		
			return items;
		}
	}
	public static Sprite texture2DSprite(Texture2D source, RP_Lump_Sprite spriteBind) {
		Debug.Log ("<color=yellow> TextureUtils.texture2DSprite("+"rx="+spriteBind.rect.position.x.ToString()+"|ry="+spriteBind.rect.position.y.ToString()+"|rw="+spriteBind.rect.size.x.ToString()+"rh="+spriteBind.rect.size.y.ToString()+")</color>");
		return Sprite.Create (source, new Rect(spriteBind.rect.position.x,spriteBind.rect.position.y,spriteBind.rect.size.x,spriteBind.rect.size.y), new Vector2(spriteBind.pivot.x,spriteBind.pivot.y), 1f);
	}
	public static Texture2D bytesToTexture(byte[] data){
		Texture2D tex = new Texture2D (2, 2, TextureFormat.RGBA32, false);
		tex.LoadImage (data);
		tex.filterMode = FilterMode.Point;
		tex.Apply ();
		 
		return tex;
	}

	public static List<Sprite> batchB64TSPR(string[] srcs){ 
		List<Sprite> sprites = new List<Sprite>();
		
		for (int i = 0; i < srcs.Length; i++) {
			//sprites.Add(LE.Texture.base64ToSprite(srcs[i]));
		}
		
		return sprites;
	}
}
public static class Utils {
	public static SerializedVector2 v2tosv2(Vector2 v2) {
		return new SerializedVector2 (v2.x, v2.y);
	}
	public static Vector2 sv2tov2(SerializedVector2 sv2) {
		return new Vector2 (sv2.x, sv2.y);
	}
	public static Vector2 getdir(bool right, bool left, bool up, bool down) {
		float hor = (float)(right ? 1f : 0f) - (float)(left ? 1f : 0f);
		float ver = (float)(up ? 1f : 0f) - (float)(down ? 1f : 0f);
		return new Vector2 (hor, ver);
	}
	public static Vector2 mousePosition(Camera cam) {
		return (Vector2)cam.ScreenToWorldPoint (Input.mousePosition);
	}
	public static Vector2 SnapToGrid(Vector2 src, Vector2 grid) {
		return new Vector2 (Mathf.RoundToInt(src.x/grid.x)*grid.x,Mathf.RoundToInt(src.y/grid.y)*grid.y);		
	}
}
public static class Lang {
	public static ResourcePack cClang;
	public static string lang = "ENG";

	public static void LoadLANG() {
		cClang = new ResourcePack();
		cClang.Load("Lang/" + lang);
	}

	public static string getString(string group, int id)
	{
		ResourcePack_Lump lump = cClang.lumps.Find(x => x.lumpName == group && x.lumpType == "String");

		if (lump == null) return "NOT FOUND ERR";

		return lump.objectData[id];
	}
}
public static class Internet {
	public static string mainPage = "https://studuloisreal.000webhostapp.com/";
	public static string actualToken = "?";
}
public static class Global {
	public static int finalScore = 0;
	public static PowerUpDatabase PowerUpDB;
	public static string targetScene = "Menu";
	public static int transitionTypeGlobal = 0;
   	public static void LoadScene(string name) {
   		Global.targetScene=name;
   		Global.transitionTypeGlobal=0;
   		UnityEngine.SceneManagement.SceneManager.LoadScene("Loader");
   	}
   	public static void LoadScene(string name, int type) {
   		Global.targetScene=name;
   		Global.transitionTypeGlobal=type;
   		UnityEngine.SceneManagement.SceneManager.LoadScene("Loader");
   	}
   	public static void LoadTargetScene() {
   		UnityEngine.SceneManagement.SceneManager.LoadScene(Global.targetScene);
   	}
    public static class Game {
		public static bool systemHalt = false;
		public static bool playerHalt = false;
        public static Vector2 cameraBoundsMin = new Vector2(-1000f, 1000f);
		public static Vector2 cameraBoundsMax = new Vector2(-1000f, 1000f);

		public static void ResetLevel() {
			if (Global.PlayerState.dead) return;
			if (Global.PlayerState.One.lives < 0) 
		    {
		    	Global.finalScore-=3;
		    }else {Global.finalScore--;}
			Global.PlayerState.coins = 0;
			Global.PlayerState.grandCoins=0;
			Global.PlayerState.One.powerUp=1;
			Global.PlayerState.One.speedCap=false;
			Global.PlayerState.One.vulnerable=true;
			Global.PlayerState.One.timer=0f;

			Global.Game.playerHalt=false;
			Global.Game.systemHalt=false;

			Global.PlayerState.One.lives--;
			Debug.Log(Global.PlayerState.One.lives.ToString());
			Global.PlayerState.dead=true;
			string scene = (Global.PlayerState.One.lives < 0) ? "GameOver" : UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
			UnityEngine.SceneManagement.SceneManager.LoadScene(scene);

		}
		public static class SaveData {
			public static bool playerPositionChange = false;
			public static Vector2 playerPosition = new Vector2(0f,0f);
			public static BGData bgData = null;
			public static MusicData musicData = null;
			public static int grandCoins = 0;

			public static string Scene = "";
		}

		public static void SaveState(Vector2 playerPosition, BGData bgData, MusicData musicData)
		{
			Global.Game.SaveData.Scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
			Global.Game.SaveData.grandCoins=Global.PlayerState.grandCoins;
			Global.Game.SaveData.playerPosition=playerPosition;
			Global.Game.SaveData.bgData=bgData;
			Global.Game.SaveData.musicData=musicData;
			Global.Game.SaveData.playerPositionChange = true;
		}
	}
	public static void GameStart() {
		System.IO.Directory.CreateDirectory(Application.persistentDataPath+"/Lang/");
		System.IO.Directory.CreateDirectory(Application.persistentDataPath+"/Styles/");
		System.IO.Directory.CreateDirectory(Application.persistentDataPath+"/Levels/");
	}
	
	public static string gameStyle = "3";
	public static string gameBits = "16";
	public static class PlayerState {
		public static int coins = 0;
		public static int grandCoins = 0;
		public static int score = 0;

		public static bool dead;

		
		public static class CurrentLevel {
			public static string LevelName = "???";
			public static string LevelDescription = "???";
			public static int level = 1;
			public static int world = 1;
			public static int timer = 255;
		}
		public static class One{
			public static string character = "Mario";
			public static bool speedCap = false;
			public static int powerUp = 0;
			public static int pointStreak = 0;
			public static int lives = 3; 
			public static float timer = 0f;
			public static bool vulnerable = true;
			public static bool star = false;
			public static float starTimer = 0f;
			public static bool starRunningOut = false;

			public static void UpdateStar(AudioClip runningOutAC) {
				if (!star) return;
				if (starTimer<=0f) {
					star=false;
					starRunningOut=false;
					starTimer=0f;
					MusicController.instance.RequestTransitionDefault();
				}else {
					starTimer-=Time.deltaTime;
				}

				if (starTimer < 2f && !starRunningOut) {
					SoundManager.instance.Play(522, runningOutAC, 1f, 1f);
					starRunningOut=true;
				}
			}
			public static void GetStar(AudioClip MarioClip, AudioClip GrabClip, AudioClip Music, float time) {
				MusicController.instance.RequestHaltMusicAndPlay(time, Music);
				SoundManager.instance.Play(520, MarioClip, 5f, 1f);
				SoundManager.instance.Play(521, GrabClip, 1f, 1f);
				star = true;
				starTimer=time;
			}

			public static List<SpriteConfig> getSprites(){
				return Global.PowerUpDB.data[Global.PlayerState.One.powerUp].sprites;
			}

        }
	}
}