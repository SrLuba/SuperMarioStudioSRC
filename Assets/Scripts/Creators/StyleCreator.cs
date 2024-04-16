using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourcePack_Type {
	IWAD, // Game WAD
	TWAD, // Tileset WAD
	RWAD, // Resource WAD
	LWAD // Level WAD
}
public class StyleCreator : MonoBehaviour
{
	string path;
	[Header("WAD TYPE")]public ResourcePack_Type Rtype;
	[Header("Lumps")]public List<ResourcePack_Lump> lumps;
	[Header("sprties")]public List<string> sprites;
	public string wadName;
	[HideInInspector]public ResourcePack pack;


    
    void Start()
    {


		for (int i = 0; i < sprites.Count; i++) {
			Sprite[] subSprites = Resources.LoadAll<Sprite> (sprites[i]);

			this.lumps.Add (new ResourcePack_Lump ("Textures/" + sprites  [i], "Texture2D", Resources.Load<Texture2D> (sprites [i]).EncodeToPNG (), "|"));
			for (int x = 0; x < subSprites.Length; x++) {
				this.lumps.Add (new ResourcePack_Lump("Sprites/"+sprites[i]+"_"+x.ToString(), "Textures/" + sprites [i], subSprites[x]));
			}
		}

		ResourcePack rp = new ResourcePack(lumps);
		path = Rtype.ToString () + "_" + wadName;
		rp.Save(path);
		
		pack.Load(path);
    }
}
